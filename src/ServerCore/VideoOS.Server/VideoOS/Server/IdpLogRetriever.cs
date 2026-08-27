using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Logging;
using VideoOS.Common.Integration.Properties;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.IdentityServer.Common.Model.Responses;
using VideoOS.Server.DataComponents;
using VideoOS.Server.Log;
using VideoOS.Utilities.Telemetry;
using VideoOS.Utilities.Timers;

namespace VideoOS.Server;

public sealed class IdpLogRetriever : IBackgroundService, IDisposable
{
	private readonly ILogReceiver _logReceiver;

	private readonly IDataComponentFactory _componentFactory;

	private readonly SemaphoreSlim _backgroundWorkLock;

	private readonly int _numberOfLogsToRead;

	private readonly double _intervalInMilliSeconds;

	private readonly ITokenCache _cache;

	private readonly IEventTimer _timer;

	private readonly IIdpAdministrationClient _administrationClient;

	private readonly IIdpLogConverter _logModelConverter;

	private readonly IIdpLogHandler _idpLogHandler;

	private readonly ILocalizedLogger _auditAndSystemLogger;

	private volatile bool _cancelled;

	private DateTime _lastExceptionTime;

	private string _idpServerName;

	private bool _disposed;

	public bool IsStarted { get; private set; }

	public IdpLogRetriever(Uri idpAddress, ITokenCache cache, ILogReceiver logReceiver, IDataComponentFactory componentFactory, ILogConfig logConfig, IIdpLogHandler idpLogHandler, ILocalizedLogger auditAndSystemLogger)
		: this(cache, new TimerWrapper(10000.0), new IdpAdministrationClient(idpAddress), logReceiver, componentFactory, logConfig, idpAddress.Host, idpLogHandler, auditAndSystemLogger)
	{
	}

	public IdpLogRetriever(ITokenCache cache, IEventTimer timer, IIdpAdministrationClient idpClient, ILogReceiver logReceiver, IDataComponentFactory componentFactory, ILogConfig logConfig, string idpServerName, IIdpLogHandler idpLogHandler, ILocalizedLogger auditAndSystemLogger)
	{
		if (logConfig == null)
		{
			throw new ArgumentNullException("logConfig");
		}
		if (logConfig.IdpBundleMaxCount <= 0)
		{
			throw new ArgumentOutOfRangeException("logConfig", "IdpBundleMaxCount is out of range");
		}
		if (logConfig.IdpBundleInterval.TotalMilliseconds <= 0.0)
		{
			throw new ArgumentOutOfRangeException("logConfig", "IdpBundleInterval is out of range");
		}
		_timer = timer ?? throw new ArgumentNullException("timer");
		_administrationClient = idpClient ?? throw new ArgumentNullException("idpClient");
		_cache = cache ?? throw new ArgumentNullException("cache");
		_logReceiver = logReceiver ?? throw new ArgumentNullException("logReceiver");
		_componentFactory = componentFactory ?? throw new ArgumentNullException("componentFactory");
		_idpServerName = idpServerName ?? throw new ArgumentNullException("idpServerName");
		_idpLogHandler = idpLogHandler ?? throw new ArgumentNullException("idpLogHandler");
		_intervalInMilliSeconds = logConfig.IdpBundleInterval.TotalMilliseconds;
		_numberOfLogsToRead = logConfig.IdpBundleMaxCount;
		_timer.AutoReset = false;
		_timer.Elapsed += DoBackgroundWork;
		_cancelled = false;
		_backgroundWorkLock = new SemaphoreSlim(1, 1);
		_logModelConverter = new IdpLogConverter();
		_auditAndSystemLogger = auditAndSystemLogger ?? throw new ArgumentNullException("auditAndSystemLogger");
	}

	public Task Start()
	{
		return Start(_intervalInMilliSeconds);
	}

	public async Task Stop()
	{
		_cancelled = true;
		await _backgroundWorkLock.WaitAsync();
		try
		{
			_timer.StopTimer();
			IsStarted = false;
		}
		finally
		{
			_backgroundWorkLock.Release();
		}
	}

	public async Task Start(double intervalInMilliSeconds)
	{
		if (intervalInMilliSeconds <= 0.0)
		{
			throw new ArgumentOutOfRangeException("intervalInMilliSeconds");
		}
		JwtSecurityToken jwtSecurityToken = await _cache.RefreshToken().ConfigureAwait(continueOnCapturedContext: false);
		if (jwtSecurityToken == null || !jwtSecurityToken.Claims.Any((Claim claim) => claim.Type == "scope" && claim.Value == "idp_log_event"))
		{
			throw new InvalidOperationException("The token returned from ITokenCache does not have the correct scope idp_log_event.");
		}
		_timer.Interval = intervalInMilliSeconds;
		_timer.StartTimer();
		_cancelled = false;
		IsStarted = true;
	}

	private async void DoBackgroundWork(object sender, EventArgs eventArgs)
	{
		if (_cancelled)
		{
			return;
		}
		try
		{
			await _backgroundWorkLock.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (ObjectDisposedException)
		{
			return;
		}
		if (_cancelled)
		{
			return;
		}
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				long latestId;
				using (ISequencesComponent sequencesComponent = _componentFactory.CreateSequencesComponent())
				{
					latestId = sequencesComponent.LatestIdpEventLogId();
				}
				JwtSecurityToken jwtSecurityToken = await _cache.RefreshToken().ConfigureAwait(continueOnCapturedContext: false);
				IEnumerable<EventLogModel> enumerable = await _administrationClient.GetEventLogs(jwtSecurityToken.RawData, latestId, _numberOfLogsToRead).ConfigureAwait(continueOnCapturedContext: false);
				EventLogModel[] array = (enumerable as EventLogModel[]) ?? enumerable.ToArray();
				if (!array.Any())
				{
					return;
				}
				List<LogEntry> list = new List<LogEntry>();
				long num2 = 0L;
				List<EventLogModel> list2 = new List<EventLogModel>();
				EventLogModel[] array2 = array;
				foreach (EventLogModel eventLogModel in array2)
				{
					if (eventLogModel.Id > num2)
					{
						num2 = eventLogModel.Id;
					}
					try
					{
						list2.Add(eventLogModel);
						LogEntry log = _logModelConverter.ConvertIdpLog(eventLogModel, _idpServerName.ToUpperInvariant());
						if (log != null)
						{
							list.Add(log);
							if (log is LogEntryAudit logEntryAudit)
							{
								ILocalizedLogger auditAndSystemLogger = _auditAndSystemLogger;
								int logId = log.LogId;
								Guid sourceId = logEntryAudit.SourceId;
								string resourceName = logEntryAudit.ResourceName;
								string resourceLocation = logEntryAudit.ResourceLocation;
								ResourceType resourceType = logEntryAudit.ResourceType;
								SourceType sourceType = logEntryAudit.SourceType;
								string location = logEntryAudit.Location;
								string userCredentials = logEntryAudit.UserCredentials;
								AuditType auditType = logEntryAudit.AuditType;
								AccessGranted accessGranted = logEntryAudit.AccessGranted;
								Func<string> description = () => GetLocalizedDescription((LogEntryId)log.LogId);
								object[] arguments = log.Arguments;
								auditAndSystemLogger.Log((LogEntryId)logId, sourceId, resourceName, resourceLocation, resourceType, sourceType, location, userCredentials, auditType, accessGranted, description, arguments);
							}
						}
						else
						{
							Server.Instance.Logger.LogDebug("IdpLogRetriever: Unknown event log. Event name {Event}", eventLogModel.Event);
						}
					}
					catch (ArgumentException ex2)
					{
						Server.Instance.Logger.LogError("{Exception}", ex2);
					}
				}
				_idpLogHandler.Publish(list2.ToArray());
				if (_cancelled)
				{
					return;
				}
				if (list.Any())
				{
					_logReceiver.AddLogEntries(list.ToArray());
				}
				using ISequencesComponent sequencesComponent2 = _componentFactory.CreateSequencesComponent();
				sequencesComponent2.UpdateIdpEventLogId(num2);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex4)
			{
				if (_lastExceptionTime.AddMinutes(6.0) < DateTime.UtcNow)
				{
					Server.Instance.Logger.LogError("{Exception}", ex4);
					_lastExceptionTime = DateTime.UtcNow;
				}
			}
			finally
			{
				if (!_cancelled)
				{
					_timer.StartTimer();
				}
			}
		}
		finally
		{
			try
			{
				_backgroundWorkLock.Release();
			}
			catch (ObjectDisposedException)
			{
			}
		}
	}

	private static string GetLocalizedDescription(LogEntryId logEntryId)
	{
		return logEntryId switch
		{
			LogEntryId.AuditIdentityProviderRegistrationSuccess => LogDefinitions.AuditIdentityProviderRegistrationSuccess, 
			LogEntryId.AuditIdentityProviderRegistrationChanged => LogDefinitions.AuditIdentityProviderRegistrationChanged, 
			LogEntryId.AuditIdentityProviderAdministratorLoginFailed => LogDefinitions.AuditLoginUserNotAdminPriviligesOnTheSystem, 
			LogEntryId.AuditBasicUserChangeOwnPassword => LogDefinitions.AuditBasicUserChangeOwnPassword, 
			LogEntryId.AuditUserLoggedIn => LogDefinitions.AuditUserLoggedIn, 
			LogEntryId.AuditUserLoggedOut => LogDefinitions.AuditUserLoggedOut, 
			LogEntryId.AuditUserLoginFailedPasswordChange => LogDefinitions.AuditUserLoginFailedPasswordChange, 
			LogEntryId.AuditBasicUserSystemLockedOut => LogDefinitions.AuditBasicUserSystemLockedOut, 
			LogEntryId.AuditBasicUserLockedOutCleared => LogDefinitions.AuditBasicUserLockedOutCleared, 
			LogEntryId.AuditBasicUserForcePasswordChangeIdentityProvider => LogDefinitions.AuditBasicUserForcePasswordChangeIdentityProvider, 
			LogEntryId.AuditBasicUserForcePasswordChangeCleared => LogDefinitions.AuditBasicUserForcePasswordChangeCleared, 
			LogEntryId.AuditUserLoginFailed => LogDefinitions.AuditUserLoginFailed, 
			LogEntryId.AuditUserLoginFailedLockedOutByAdmin => LogDefinitions.AuditUserLoginFailedLockedOutByAdmin, 
			LogEntryId.AuditUserLoginFailedUnknownUser => LogDefinitions.AuditUserLoginFailedUnknownUser, 
			LogEntryId.AuditBasicUserDescriptionChanged => LogDefinitions.AuditBasicUserDescriptionChanged, 
			LogEntryId.AutoProvisionedUserUpdated => LogDefinitions.AutoProvisionedUserUpdated, 
			LogEntryId.AuditBasicUserCreatedIdentityProvider => LogDefinitions.AuditBasicUserCreatedIdentityProvider, 
			LogEntryId.AuditBasicUserDeletedIdentityProvider => LogDefinitions.AuditBasicUserDeletedIdentityProvider, 
			LogEntryId.AuditBasicUserEnabledIdentityProvider => LogDefinitions.AuditBasicUserEnabledIdentityProvider, 
			LogEntryId.AuditBasicUserDisabled => LogDefinitions.AuditBasicUserDisabled, 
			_ => throw new ArgumentException("Unknown idp event type.", logEntryId.ToString()), 
		};
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public void Dispose(bool disposing)
	{
		_cancelled = true;
		if (!_disposed)
		{
			if (disposing)
			{
				_timer?.Dispose();
				_backgroundWorkLock?.Dispose();
				_administrationClient?.Dispose();
			}
			_disposed = true;
		}
	}
}
