using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.DeviceDiscovery;
using VideoOS.Server.DataComponents;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.RecorderCommunication;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.CommandHandler;

public sealed class HardwareScanCommandHandler : ICommandHandler
{
	private class HardwareScanMessageManagerCallback : IMessageManagerCallback, IDisposable
	{
		private IResponseSink responseSink;

		private bool completedByResponse;

		private ManualResetEvent completedEvent;

		public HardwareScanMessageManagerCallback(IResponseSink responseSink)
		{
			this.responseSink = responseSink;
			completedEvent = new ManualResetEvent(initialState: false);
			completedByResponse = false;
		}

		public bool WaitForCompletion()
		{
			completedEvent.WaitOne();
			return completedByResponse;
		}

		public void Stop()
		{
			completedEvent.Set();
		}

		void IMessageManagerCallback.HandleTimeout(CommandBase command)
		{
			responseSink.AddResponse(new TimeoutResponse());
			completedEvent.Set();
		}

		void IMessageManagerCallback.HandleResponse(CommandBase command, ResponseBase response)
		{
			if (response is HardwareScanProgressResponse hardwareScanProgressResponse)
			{
				ScanResult scanResult = ScanResult.FromXml(hardwareScanProgressResponse.ScanResultXml);
				if (scanResult.DiscoveredHardware != null)
				{
					string[] array = (from h in scanResult.DiscoveredHardware
						where !string.IsNullOrEmpty(h.MacAddress)
						select h.MacAddress).ToArray();
					if (array.Length != 0)
					{
						bool flag = false;
						try
						{
							string[] array2 = ConfigurationDataManager.QueryExistingMacAddresses(array);
							string[] array3 = array2;
							foreach (string address in array3)
							{
								scanResult.DiscoveredHardware.First((Discovery h) => string.Equals(h.MacAddress, address, StringComparison.OrdinalIgnoreCase)).MacExistsGlobal = true;
								flag = true;
							}
						}
						catch (ConfigurationDataSubsystemException ex)
						{
							Server.Instance.Logger.LogWarning("An error occurred during hardware scanning while checking if returned mac addresses already exists in the database.. Exception\n{Exception}", ex);
						}
						catch (DatabaseException ex2)
						{
							Server.Instance.Logger.LogWarning("An error occurred during hardware scanning while checking if returned mac addresses already exists in the database.. Exception\n{Exception}", ex2);
						}
						string keyName = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Milestone\\XProtect Management Server";
						string valueName = "AllowIcpXpcoSelfAdd";
						if (!Convert.ToBoolean(Registry.GetValue(keyName, valueName, 0) ?? ((object)0), CultureInfo.InvariantCulture))
						{
							string[] array4 = array;
							foreach (string queryAddress in array4)
							{
								if (Guid.TryParse(queryAddress, out var result) && ConfigurationDataManager.FindHomeSiteInfo().Id.Equals(result))
								{
									scanResult.DiscoveredHardware.First((Discovery h) => string.Equals(h.MacAddress, queryAddress, StringComparison.OrdinalIgnoreCase)).IsHomeSite = true;
									flag = true;
								}
							}
						}
						if (flag)
						{
							hardwareScanProgressResponse.ScanResultXml = scanResult.ToXml();
						}
					}
				}
			}
			responseSink.AddResponse(response);
		}

		bool IMessageManagerCallback.IsLastExpectedResponse(CommandBase command, ResponseBase response)
		{
			if (response is HardwareScanProgressResponse hardwareScanProgressResponse)
			{
				ScanResult scanResult = ScanResult.FromXml(hardwareScanProgressResponse.ScanResultXml);
				if (!scanResult.Completed)
				{
					return false;
				}
			}
			completedByResponse = true;
			completedEvent.Set();
			return true;
		}

		public void Dispose()
		{
			completedEvent.Dispose();
		}
	}

	private Server server;

	private ConcurrentDictionary<Guid, HardwareScanMessageManagerCallback> tokenToCallback = new ConcurrentDictionary<Guid, HardwareScanMessageManagerCallback>();

	private ConcurrentDictionary<Guid, Guid> sessionToServiceId = new ConcurrentDictionary<Guid, Guid>();

	public HardwareScanCommandHandler(Server server)
	{
		this.server = server;
		this.server.ProxyResponseManager.ResponseSendTimeout += ProxyResponseManager_ReponseSendTimeout;
	}

	private void ProxyResponseManager_ReponseSendTimeout(object sender, ResponseSendTimeoutEventArgs e)
	{
		if (tokenToCallback.TryGetValue(e.Responses[0].Token, out var value))
		{
			value.Stop();
		}
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is HardwareScanCommand hardwareScanCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo callContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		Guid value = Guid.Empty;
		if (hardwareScanCommand.Operation == HardwareScanOperation.CancelScan)
		{
			if (!sessionToServiceId.TryGetValue(hardwareScanCommand.SessionId, out value))
			{
				Server.Instance.Logger.LogWarning("Could not find recorder with id '{RecorderId}' for session '{SessionId}'.", hardwareScanCommand.RecorderId, hardwareScanCommand.SessionId);
				responseSink.AddResponse(new RecorderNotFoundResponse(hardwareScanCommand.RecorderId));
				return;
			}
		}
		else
		{
			RecordingServer recordingServer = RecordingServer.QueryActiveRecordingServer(hardwareScanCommand.RecorderId);
			if (recordingServer == null)
			{
				Server.Instance.Logger.LogWarning("Could not find recorder with id = {RecorderId}.", hardwareScanCommand.RecorderId);
				responseSink.AddResponse(new RecorderNotFoundResponse(hardwareScanCommand.RecorderId));
				return;
			}
			value = recordingServer.ServiceId;
			sessionToServiceId.TryAdd(hardwareScanCommand.SessionId, value);
		}
		if (!server.RecorderCommManager.IsRecorderAttachedAndConnected(value))
		{
			responseSink.AddResponse(new RecorderUnavailableResponse(hardwareScanCommand.RecorderId));
			return;
		}
		HardwareScanMessageManagerCallback value2 = new HardwareScanMessageManagerCallback(responseSink);
		tokenToCallback.TryAdd(hardwareScanCommand.Token, value2);
		try
		{
			try
			{
				server.MessageManager.SendCommand(value, hardwareScanCommand, value2, TimeSpan.FromMinutes(3.0), callContextInfo);
			}
			catch (RecorderNotFoundException)
			{
				responseSink.AddResponse(new RecorderUnavailableResponse(hardwareScanCommand.RecorderId));
				return;
			}
			catch (NotSupportedByLegacyRecorderException)
			{
				responseSink.AddResponse(new RecorderNotSupportedResponse(hardwareScanCommand.RecorderId));
				return;
			}
			if (!value2.WaitForCompletion())
			{
				try
				{
					server.RecorderCommManager.HardwareCancelScanAsync(value, hardwareScanCommand.SessionId, callContextInfo);
					return;
				}
				catch (RecorderNotFoundException)
				{
					return;
				}
				catch (NotSupportedByLegacyRecorderException)
				{
					return;
				}
			}
		}
		finally
		{
			if (tokenToCallback.TryRemove(hardwareScanCommand.Token, out value2))
			{
				value2.Dispose();
			}
			sessionToServiceId.TryRemove(hardwareScanCommand.SessionId, out var _);
		}
	}
}
