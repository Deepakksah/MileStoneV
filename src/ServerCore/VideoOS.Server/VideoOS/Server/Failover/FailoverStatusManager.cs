using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.ServiceController;
using VideoOS.Common.Integration.WebApi;
using VideoOS.Management.Common;
using VideoOS.Rules.Interfaces;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Utilities.Collections;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Telemetry;
using VideoOS.Vms.Types;

namespace VideoOS.Server.Failover;

public class FailoverStatusManager
{
	public sealed class FailoverServerMonitor : IDisposable
	{
		private static readonly FailoverStatusDto _disconnectedStatus = new RecorderFailoverServiceState
		{
			ConnectStatus = ConnectStatus.Disconnected,
			ServiceStatus = ServiceState.Indeterminate,
			WatchingState = WatchingState.Stopped
		}.ToDto();

		private static readonly FailoverStatusDto _SSLCommunicationErrorStatus = new RecorderFailoverServiceState
		{
			ConnectStatus = ConnectStatus.SslCommunicationError,
			ServiceStatus = ServiceState.Indeterminate,
			WatchingState = WatchingState.Stopped
		}.ToDto();

		private const int _failoverCheckStatusTimeoutToSeconds = 15;

		private static TimeSpan _failoverCheckStatusTimeout;

		private FailoverStatusRestClient _statusClient;

		private Guid _sessionId;

		private Task _loopTask;

		private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

		private readonly ILogger _logger;

		private readonly bool _secure;

		public FailoverStatusDto CurrentStatus { get; private set; }

		public FailoverServer FailoverServer { get; }

		public event EventHandler<FailoverStatusChangedEventArgs> CurrentStatusChanged;

		public FailoverServerMonitor(FailoverServer failoverServer, ILogger logger, bool secure)
		{
			FailoverServer = failoverServer ?? throw new ArgumentNullException("failoverServer");
			_logger = logger ?? throw new ArgumentNullException("logger");
			_secure = secure;
			string text = ConfigurationManager.AppSettings["VideoOS.Server.Failover.FailoverCheckStatusTimeout"];
			_failoverCheckStatusTimeout = ((!string.IsNullOrEmpty(text) && int.TryParse(text, out var result) && result >= 15) ? TimeSpan.FromSeconds(result) : TimeSpan.FromSeconds(15.0));
			_logger.LogInformation("FailoverServerMonitor: FailoverCheckStatusTimeout set to {FailoverCheckStatusTimeout} seconds.", _failoverCheckStatusTimeout);
		}

		public void Start()
		{
			_sessionId = Guid.NewGuid();
			_loopTask = CheckStatusAsync(_tokenSource.Token);
		}

		private async Task CheckStatusAsync(CancellationToken cancellation)
		{
			while (!cancellation.IsCancellationRequested)
			{
				if (_statusClient == null)
				{
					_statusClient = CreateStatusClient(_secure, FailoverServer.HostName, 8990);
				}
				bool ranSuccessfully = false;
				try
				{
					await CheckStatusUpdatesCoreAsync(_failoverCheckStatusTimeout);
					ranSuccessfully = true;
				}
				catch (TrustFailureException ex)
				{
					SetCurrentStatus(_SSLCommunicationErrorStatus);
					_logger.LogWarning("GetFailoverStatusAsync failed. Exception: {Exception}", ex);
				}
				catch (WebApiException ex2)
				{
					SetCurrentStatus(_disconnectedStatus);
					_logger.LogWarning("GetFailoverStatusAsync failed. Exception: {Exception}", ex2);
				}
				catch (AccessTokenException ex3)
				{
					SetCurrentStatus(_disconnectedStatus);
					_logger.LogWarning("GetFailoverStatusAsync failed. Exception: {Exception}", ex3);
				}
				catch (Exception ex4)
				{
					SetCurrentStatus(_disconnectedStatus);
					_logger.LogWarning("GetFailoverStatusAsync failed. Exception: {Exception}", ex4);
				}
				if (!ranSuccessfully)
				{
					if (_statusClient != null)
					{
						_statusClient.Dispose();
						_statusClient = null;
					}
					await Task.Factory.Delay(_failoverCheckStatusTimeout, cancellation);
				}
			}
		}

		private static FailoverStatusRestClient CreateStatusClient(bool secure, string hostName, int port)
		{
			FailoverStatusRestClient failoverStatusRestClient = null;
			try
			{
				UriBuilder uriBuilder = new UriBuilder(secure ? Uri.UriSchemeHttps : Uri.UriSchemeHttp, hostName, port);
				failoverStatusRestClient = new FailoverStatusRestClient(uriBuilder.Uri);
				return failoverStatusRestClient;
			}
			catch
			{
				failoverStatusRestClient?.Dispose();
				failoverStatusRestClient = null;
				throw;
			}
		}

		private async Task CheckStatusUpdatesCoreAsync(TimeSpan timeout)
		{
			FailoverStatusDto failoverStatusDto = await _statusClient.GetFailoverStatusUpdates(_sessionId, timeout).ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				if (failoverStatusDto != null)
				{
					SetCurrentStatus(failoverStatusDto);
				}
			}
			catch (AggregateException ex)
			{
				_logger.LogWarning("FailoverServerMonitor failed. Exception: {Exception}", ex);
				throw;
			}
		}

		private void SetCurrentStatus(FailoverStatusDto value)
		{
			if (CurrentStatus != value)
			{
				FailoverStatusDto currentStatus = CurrentStatus;
				CurrentStatus = value;
				OnCurrentStateChanged(value, currentStatus);
			}
		}

		private void OnCurrentStateChanged(FailoverStatusDto currentStatus, FailoverStatusDto previousStatus)
		{
			this.CurrentStatusChanged?.Invoke(this, new FailoverStatusChangedEventArgs(FailoverServer, currentStatus, previousStatus));
		}

		public void Dispose()
		{
			_tokenSource.Cancel();
			_loopTask.Wait();
			_tokenSource.Dispose();
			_statusClient?.Dispose();
		}
	}

	public static class TestAccessor
	{
		public static void Trigger_Monitor_CurrentStatusChanged(FailoverStatusManager manager, object sender, FailoverStatusChangedEventArgs e)
		{
			manager.Monitor_CurrentStatusChanged(sender, e);
		}

		public static SynchronizedList<FailoverServer, FailoverServerMonitor, Guid> GetFailoverMonitorList(FailoverStatusManager manager)
		{
			return manager._failoverMonitorList;
		}
	}

	private static readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1.0);

	private readonly SynchronizedList<FailoverServer, FailoverServerMonitor, Guid> _failoverMonitorList;

	private readonly object _syncRoot = new object();

	private readonly ILogger _logger;

	private readonly IServerEnvironment _serverEnvironment;

	private IRuleEngine RuleEngine => _serverEnvironment.RuleEngine;

	public event EventHandler<FailoverStatusChangedEventArgs> FailoverStatusChanged;

	public FailoverStatusManager(IServerEnvironment serverEnvironment)
		: this(serverEnvironment, Telemetry.GetILogger("RecorderStatus"))
	{
		Task.Run(() => CheckFailoverServers(_checkInterval, CancellationToken.None));
	}

	public FailoverStatusManager(IServerEnvironment serverEnvironment, ILogger logger)
	{
		FailoverStatusManager failoverStatusManager = this;
		_serverEnvironment = serverEnvironment ?? throw new ArgumentNullException("serverEnvironment");
		_logger = logger ?? throw new ArgumentNullException("logger");
		_failoverMonitorList = new SynchronizedList<FailoverServer, FailoverServerMonitor, Guid>((FailoverServerMonitor monitor) => monitor.FailoverServer.Id, (FailoverServer failoverServer) => failoverServer.Id, delegate(FailoverServer failoverServer)
		{
			failoverStatusManager._logger.LogInformation("Adding monitor for failover server. Id: {ServerId}; Name: {ServerName}", failoverServer.Id, failoverServer.Name);
			FailoverServerMonitor failoverServerMonitor = null;
			try
			{
				failoverServerMonitor = new FailoverServerMonitor(failoverServer, failoverStatusManager._logger, serverEnvironment.ServerConfig.WebApiConfig.SecuredServer);
				failoverServerMonitor.CurrentStatusChanged += failoverStatusManager.Monitor_CurrentStatusChanged;
				failoverServerMonitor.Start();
				return failoverServerMonitor;
			}
			catch
			{
				failoverServerMonitor?.Dispose();
				throw;
			}
		}, delegate(FailoverServerMonitor monitor)
		{
			failoverStatusManager._logger.LogInformation("Removing monitor for failover server. Id: {ServerId}", monitor.FailoverServer.Id);
			monitor.CurrentStatusChanged -= failoverStatusManager.Monitor_CurrentStatusChanged;
			monitor.Dispose();
		});
	}

	public ICollection<FailoverStatusResponse> CreateFailoverStatusResponseList()
	{
		List<FailoverStatusResponse> list = new List<FailoverStatusResponse>();
		lock (_syncRoot)
		{
			foreach (FailoverServerMonitor failoverMonitor in _failoverMonitorList)
			{
				if (failoverMonitor.CurrentStatus != null)
				{
					list.Add(new FailoverStatusResponse(failoverMonitor.FailoverServer.Id, failoverMonitor.CurrentStatus));
				}
			}
			return list;
		}
	}

	private void OnFailoverStatusChanged(object sender, FailoverStatusChangedEventArgs e)
	{
		this.FailoverStatusChanged?.Invoke(this, e);
	}

	private void Monitor_CurrentStatusChanged(object sender, FailoverStatusChangedEventArgs e)
	{
		OnFailoverStatusChanged(sender, e);
		TriggerFailoverEvents(FailingRecorderId(e.CurrentStatus), FailingRecorderId(e.PreviousStatus));
		TriggerFailoverCommunicationErrorEvents(e.PreviousStatus, e.CurrentStatus);
	}

	private static Guid FailingRecorderId(FailoverStatusDto statusDto)
	{
		if (statusDto != null)
		{
			RecorderFailoverServiceState recorderFailoverServiceState = RecorderFailoverServiceState.FromDto(statusDto);
			if (recorderFailoverServiceState.WatchingState == WatchingState.Failover || recorderFailoverServiceState.WatchingState == WatchingState.HotStandbyActive)
			{
				return recorderFailoverServiceState.FailingRecorderId;
			}
		}
		return Guid.Empty;
	}

	private void TriggerFailoverEvents(Guid failedRecorderIdCurrent, Guid failedRecorderIdPrevious)
	{
		if (!(failedRecorderIdCurrent == failedRecorderIdPrevious))
		{
			if (failedRecorderIdPrevious != Guid.Empty)
			{
				RuleEngine.AddTriggerEvent(new TriggerEvent(failedRecorderIdPrevious, BuiltInEventTypes.EventIDs.FailoverStopped));
			}
			if (failedRecorderIdCurrent != Guid.Empty)
			{
				RuleEngine.AddTriggerEvent(new TriggerEvent(failedRecorderIdCurrent, BuiltInEventTypes.EventIDs.FailoverStarted));
			}
		}
	}

	private void TriggerFailoverCommunicationErrorEvents(FailoverStatusDto previousStatus, FailoverStatusDto currentStatus)
	{
		currentStatus?.RecorderWatchItems.ToList().ForEach(delegate(RecorderWatchItemDto currentRecorderWatchItem)
		{
			RecorderWatchItemDto recorderWatchItemDto = previousStatus?.RecorderWatchItems.FirstOrDefault((RecorderWatchItemDto prw) => prw.RecorderId == currentRecorderWatchItem.RecorderId);
			if (currentRecorderWatchItem.Errors.HasFlag(CommunicationErrors.SslCommunication) && (recorderWatchItemDto == null || !recorderWatchItemDto.Errors.HasFlag(CommunicationErrors.SslCommunication)))
			{
				RuleEngine.AddTriggerEvent(new TriggerEvent(currentRecorderWatchItem.RecorderId, BuiltInEventTypes.EventIDs.FailoverEncryptedCommunicationError));
			}
			if (currentRecorderWatchItem.Errors.HasFlag(CommunicationErrors.AuthorizationError) && (recorderWatchItemDto == null || !recorderWatchItemDto.Errors.HasFlag(CommunicationErrors.AuthorizationError)))
			{
				RuleEngine.AddTriggerEvent(new TriggerEvent(currentRecorderWatchItem.RecorderId, BuiltInEventTypes.EventIDs.FailoverAuthorizationError));
			}
		});
	}

	private async Task CheckFailoverServers(TimeSpan interval, CancellationToken cancellation)
	{
		while (!cancellation.IsCancellationRequested)
		{
			try
			{
				FailoverServer[] source = FailoverServer.QueryFailoverServers();
				lock (_syncRoot)
				{
					_failoverMonitorList.Update(source);
				}
			}
			catch (DatabaseException)
			{
			}
			catch (VmoServerException)
			{
			}
			catch (Exception ex3)
			{
				_logger.LogError("CheckFailoverServers failed. Exception: {Exception}", ex3);
			}
			await Task.Factory.Delay(interval, cancellation);
		}
	}
}
