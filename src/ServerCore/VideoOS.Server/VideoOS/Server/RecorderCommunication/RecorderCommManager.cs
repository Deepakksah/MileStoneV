using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Security;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Management.VmoClient;
using VideoOS.Rules.Interfaces;
using VideoOS.Server.DataComponents;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.Properties;
using VideoOS.Server.ServerAPI.DataComponents;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Network;
using VideoOS.Utilities.Telemetry;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Server.RecorderCommunication;

public sealed class RecorderCommManager : IRecorderCommManager, IRecorderComm, IServerComm, IServiceConnectionStatusProvider
{
	public static readonly ILogger Logger = Telemetry.GetILogger("RecorderCommManager");

	private ICommunicationProvider _webApiCommnucationProvider;

	public int MaxSimultaniousRecorderConfigRequests { get; private set; }

	private int ConnectionAliveTimeout { get; set; }

	public IAttachedRecorders AttachedRecorders { get; } = new AttachedRecorders();

	public Server Server { get; private set; }

	public event EventHandler<ResponseEventArgs> ResponseReceived;

	public event EventHandler<RecorderCommunicationStateEventArgs> CommunicationStateChanged;

	public event EventHandler<RecorderStatusEventArgs> RecorderStatusChanged;

	private void OnCommunicationStateChange(RecorderCommunicationStateEventArgs e)
	{
		this.CommunicationStateChanged?.Invoke(this, e);
	}

	private void OnRecorderStatusChanged(RecorderStatusEventArgs e)
	{
		this.RecorderStatusChanged?.Invoke(this, e);
	}

	private void OnResponseReceived(ResponseEventArgs e)
	{
		this.ResponseReceived?.Invoke(this, e);
	}

	public RecorderCommManager(Server server)
	{
		Server = server;
	}

	public void Initialize(ServerConfig serverConfig)
	{
		Logger.LogInformation("Initialize");
		try
		{
			lock (this)
			{
				_webApiCommnucationProvider = new WebApiCommunicationProvider();
				_webApiCommnucationProvider.ConnectionChanged += CommunicationProvider_ConnectionChanged;
				_webApiCommnucationProvider.Initialize(serverConfig);
				MaxSimultaniousRecorderConfigRequests = serverConfig.RecorderCommunicationConfiguration.MaxSimultaneousConfigRequests;
				ConnectionAliveTimeout = serverConfig.RecorderCommunicationConfiguration.ConnectionAliveTimeout;
			}
		}
		catch (Exception ex)
		{
			Logger.LogError("Initialize failed: {Exception}", ex);
			throw;
		}
	}

	public bool RequestGetCompleteConfigurationWork()
	{
		return Server.RequestGetCompleteConfigurationWork();
	}

	public void ReleaseGetCompleteConfigurationWork()
	{
		Server.ReleaseGetCompleteConfigurationWork();
	}

	public bool RequestAttachRecorderWork()
	{
		return Server.RequestAttachRecorderWork();
	}

	public void ReleaseAttachRecorderWork()
	{
		Server.ReleaseAttachRecorderWork();
	}

	private static void CreateRecorder(Guid id, string name, string hostName, string webServiceUri, RecordingServerCreateOptions options)
	{
		using ServerConnection serverConnection = new ServerConnection(ServerConfig.Instance.VmoCommunicationConfiguration.ServerName);
		serverConnection.ServerPort = ServerConfig.Instance.VmoCommunicationConfiguration.Port;
		serverConnection.Connect();
		using VmoClient vmoClient = new VmoClient(serverConnection);
		ManagementServer managementServer = vmoClient.ManagementServer;
		VideoOS.Management.VmoClient.RecordingServer recordingServer = new VideoOS.Management.VmoClient.RecordingServer(managementServer, id, name, hostName, webServiceUri, options);
		recordingServer.Create();
	}

	public bool RegisterRecorder(IRecorderData recorderData, IRecorderEntryFactory factory, string displayName, string hostName, string webServerUri, int failoverPort)
	{
		if (!Server.ConfigurationLookup.RecorderExists(recorderData.RecorderId))
		{
			CreateRecorder(recorderData.RecorderId, displayName, hostName, webServerUri, new RecordingServerCreateOptions
			{
				DefaultFailoverUdpPort = failoverPort
			});
		}
		Logger.LogInformation("Register recorder - Recorder id: {RecorderId}; Service id: {ServiceId}; Service type: {ServiceType}", recorderData.RecorderId, recorderData.ServiceId, recorderData.ServiceType);
		recorderData.ConnectionStatus = RecorderConnectionStatus.Connected;
		recorderData.AttachStatus = RecorderAttachStatus.Attached;
		recorderData.RecorderStatus = RecorderStatuses.None;
		IRecorderEntry recorderEntry = factory.CreateRecorder(recorderData);
		ICallContextInfo callContextInfo = Server.CreateServerServiceCallContextUserInfo();
		AttachedRecorders.Register(recorderEntry);
		Server.HandleRecorder(recorderEntry.RecorderId, callContextInfo);
		recorderEntry.ServiceConnectionStatus.ServiceOffline += ServiceOffline;
		recorderEntry.ServiceConnectionStatus.ServiceOnline += ServiceOnline;
		recorderEntry.ServiceConnectionStatus.CommunicationBroken += CommunicationBroken;
		OnRecorderStatusChanged(new RecorderStatusEventArgs(recorderEntry.RecorderId, recorderEntry.ServiceId));
		OnCommunicationStateChange(new RecorderCommunicationStateEventArgs(recorderEntry.RecorderId, recorderEntry.ServiceId, recorderEntry.Status, recorderEntry.ConnectionStatus, RecorderAttachStatus.Attached, recorderEntry.Status, recorderEntry.ConnectionStatus, RecorderAttachStatus.NotAttached));
		Logger.LogInformation("AttachRecorder - Succeeded. Recorder id: {RecorderId}; Service id: {ServiceId}, Attach status: {AttachStatus}", recorderEntry.RecorderId, recorderEntry.ServiceId, RecorderAttachStatus.Attached);
		return recorderEntry.Reciever.IsConfigurationCompatible;
	}

	private void ServiceOffline(object sender, ServiceConnectionOfflineEventArgs e)
	{
		ServiceDiscoveryStateChanged(sender, e.ServiceId, online: false);
	}

	private void ServiceOnline(object sender, ServiceConnectionOnlineEventArgs e)
	{
		ServiceDiscoveryStateChanged(sender, e.ServiceId, online: true);
	}

	public void CommunicationBroken(object sender, ServerCommunicationBrokenEventArgs e)
	{
		if (e.Reason == ServerCommunicationBrokenReason.ServiceAuthentication)
		{
			string authenticationBrokenMessage = Resources.AuthenticationBrokenMessage;
			Server.Logger.LogInformation("{Reason}", authenticationBrokenMessage);
			Server.RequestShutdown(authenticationBrokenMessage);
		}
		if (e.Reason == ServerCommunicationBrokenReason.InvalidCertificate || e.Reason == ServerCommunicationBrokenReason.EndpointNotFound)
		{
			UnregisterRecorder(e.ServiceId);
		}
	}

	private void ServiceDiscoveryStateChanged(object sender, Guid serviceId, bool online)
	{
		ConnectionState connectionState = ((!online) ? ConnectionState.Lost : ConnectionState.Established);
		CommunicationProvider_ConnectionChanged(sender, new ConnectionChangedEventArgs(serviceId, connectionState));
	}

	public void UnregisterRecorder(Guid serviceId)
	{
		Logger.LogInformation("Unregister recorder. Service id: {ServiceId}", serviceId);
		try
		{
			if (AttachedRecorders.TryGetRecorderEntry(serviceId, out var entry))
			{
				RecorderAttachStatus attachStatus = entry.AttachStatus;
				RecorderConnectionStatus connectionStatus = entry.ConnectionStatus;
				entry.AttachStatus = RecorderAttachStatus.NotAttached;
				entry.ConnectionStatus = RecorderConnectionStatus.NotConnected;
				if (entry.ServiceConnectionStatus != null)
				{
					entry.ServiceConnectionStatus.ServiceOffline -= ServiceOffline;
					entry.ServiceConnectionStatus.ServiceOnline -= ServiceOnline;
					entry.ServiceConnectionStatus.CommunicationBroken -= CommunicationBroken;
				}
				OnRecorderStatusChanged(new RecorderStatusEventArgs(entry.RecorderId, entry.ServiceId));
				OnCommunicationStateChange(new RecorderCommunicationStateEventArgs(entry.RecorderId, entry.ServiceId, entry.Status, entry.ConnectionStatus, entry.AttachStatus, entry.Status, connectionStatus, attachStatus));
				AttachedRecorders.UnRegisterByServiceId(serviceId);
				entry.ReleaseResources();
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Logger.LogError("Unregister recorder failed: Service id: {ServiceId}\n{Exception}", serviceId, ex2);
		}
	}

	public void SetRecorderInformation(Guid serviceId, string name, string hostName, Uri webServerUri, string scanRanges, string timeZone, string devicePackInfo, ushort failoverServerPort, int databasePort, Guid failoverId, string version)
	{
		Logger.LogInformation("SetRecorderInformation - Service id: {ServiceId}", serviceId);
		IRecorderEntry entry = null;
		AttachedRecorders.TryGetRecorderEntry(serviceId, out entry);
		bool isMulticastLicensed = Server.LicenseManager.IsFeatureLicensed("Multicast");
		SetRecorderInformationInternal(serviceId, name, hostName, webServerUri, scanRanges, timeZone, devicePackInfo, failoverServerPort, databasePort, failoverId, entry, entry.RecorderId, entry.ServiceType, version, isMulticastLicensed);
	}

	private void SetRecorderInformationInternal(Guid serviceId, string name, string hostName, Uri webServerUri, string scanRanges, string timeZone, string devicePackInfo, ushort failoverServerPort, int databasePort, Guid failoverId, IRecorderEntry recEntry, Guid recorderId, RecordingServerServiceType serviceType, string version, bool isMulticastLicensed)
	{
		IRecorderEntry recorderEntry = FindMainRecorderEntry(recEntry);
		if (recorderEntry != null && recorderEntry.ConnectionStatus == RecorderConnectionStatus.Connected)
		{
			return;
		}
		Server.Data.ConfigurationDataSetCacheLock.EnterWriteLock();
		try
		{
			Validation.CheckName(name, "name");
			Validation.CheckVersionString(version, "version");
			Version version2 = new Version(version);
			Tuple<bool, bool> tuple = VideoOS.Server.DataComponents.ConfigurationObjects.RecordingServer.CreateOrUpdateRecorder(recorderId, serviceId, serviceType, name, hostName, webServerUri, scanRanges, timeZone, devicePackInfo, failoverServerPort, databasePort, failoverId, version2, isMulticastLicensed);
			if (tuple.Item1)
			{
				ServerLogManager.Instance.AddLogEntry(LogEntryId.RecorderAdded, name);
				Logger.LogInformation("SetRecorderInformation - Recorder added to configuration. Recorder ID : {RecorderId}, Name : {Name}, Host name : {Host}, Recorder type: {Type}", recorderId, name, hostName, serviceType);
			}
			else
			{
				Logger.LogInformation("SetRecorderInformation - Recorder information changed. Recorder ID : {RecorderId}, Name : {Name}, Host name : {Host}, Recorder type: {Type}", recorderId, name, hostName, serviceType);
			}
			if (tuple.Item2 && serviceType == RecordingServerServiceType.RecordingServer)
			{
				TryAllRecordersProcessPendingTasksAsync();
			}
			Server.Instance.ServiceChannelManager.SendRecordingServerActivatedMessageAsync(recorderId, tuple.Item1);
			OnRecorderStatusChanged(new RecorderStatusEventArgs(recorderId, serviceId));
		}
		finally
		{
			Server.Data.ConfigurationDataSetCacheLock.ExitWriteLock();
		}
	}

	public void SetRecorderStatus(Guid serviceId, RecorderStatuses status)
	{
		if (AttachedRecorders.TryGetRecorderEntry(serviceId, out var entry))
		{
			RecorderStatuses status2 = entry.Status;
			entry.Status = status;
			OnRecorderStatusChanged(new RecorderStatusEventArgs(entry.RecorderId, entry.ServiceId));
			OnCommunicationStateChange(new RecorderCommunicationStateEventArgs(entry.RecorderId, entry.ServiceId, entry.Status, entry.ConnectionStatus, entry.AttachStatus, status2, entry.ConnectionStatus, entry.AttachStatus));
		}
	}

	private IRecorderEntry FindMainRecorderEntry(IRecorderEntry recEntry)
	{
		IRecorderEntry mainRecorder = null;
		if (recEntry.ServiceType != RecordingServerServiceType.RecordingServer)
		{
			IEnumerable<IRecorderEntry> source = AttachedRecorders.Where((IRecorderEntry recorder) => recorder.RecorderId == recEntry.RecorderId && recorder.ServiceType == RecordingServerServiceType.RecordingServer);
			mainRecorder = source.FirstOrDefault();
			if (mainRecorder != null)
			{
				bool flag = false;
				try
				{
					Task<bool> task = Task.Factory.StartNew(() => mainRecorder.IsConnectionAlive(), TaskCreationOptions.LongRunning);
					if (task.Wait(ConnectionAliveTimeout))
					{
						flag = task.Result;
					}
					else
					{
						task.ContinueWith(delegate(Task<bool> t)
						{
							try
							{
								t.Wait();
							}
							catch (AggregateException)
							{
							}
						}, TaskContinuationOptions.OnlyOnFaulted);
					}
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
					flag = false;
				}
				if (!flag)
				{
					RecorderConnectionStatus connectionStatus = mainRecorder.ConnectionStatus;
					mainRecorder.ConnectionStatus = RecorderConnectionStatus.ConnectionBroken;
					if (connectionStatus != mainRecorder.ConnectionStatus)
					{
						OnRecorderStatusChanged(new RecorderStatusEventArgs(mainRecorder.RecorderId, mainRecorder.ServiceId));
						OnCommunicationStateChange(new RecorderCommunicationStateEventArgs(mainRecorder.RecorderId, mainRecorder.ServiceId, mainRecorder.Status, mainRecorder.ConnectionStatus, mainRecorder.AttachStatus, mainRecorder.Status, connectionStatus, mainRecorder.AttachStatus));
					}
				}
			}
		}
		return mainRecorder;
	}

	private void CommunicationProvider_ConnectionChanged(object sender, ConnectionChangedEventArgs e)
	{
		Logger.LogInformation("Connection changed event: {ConnectionState}, Url: {Url}, Service id: {ServiceId}, Exception: {ExceptionMessage}.", e.ConnectionState, (e.Uri == null) ? "<null>" : e.Uri, (e.ServiceId == default(Guid)) ? "<null>" : e.ServiceId.ToString(), (e.ExceptionMessage == null) ? "<null>" : e.ExceptionMessage);
		try
		{
			IRecorderEntry entry;
			if (e.Uri != null)
			{
				entry = AttachedRecorders.GetRecorderEntryFromUri(e.Uri);
			}
			else if (!AttachedRecorders.TryGetRecorderEntry(e.ServiceId, out entry))
			{
				return;
			}
			switch (e.ConnectionState)
			{
			case ConnectionState.Established:
				if (entry != null)
				{
					RecorderConnectionStatus recorderConnectionStatus = entry.ConnectionStatus;
					entry.ConnectionStatus = RecorderConnectionStatus.Connected;
					OnRecorderStatusChanged(new RecorderStatusEventArgs(entry.RecorderId, entry.ServiceId));
					OnCommunicationStateChange(new RecorderCommunicationStateEventArgs(entry.RecorderId, entry.ServiceId, entry.Status, entry.ConnectionStatus, entry.AttachStatus, entry.Status, recorderConnectionStatus, entry.AttachStatus));
				}
				break;
			case ConnectionState.Lost:
			{
				RecorderConnectionStatus recorderConnectionStatus = RecorderConnectionStatus.NotConnected;
				if (entry != null)
				{
					recorderConnectionStatus = entry.ConnectionStatus;
					entry.ConnectionStatus = RecorderConnectionStatus.NotConnected;
					OnRecorderStatusChanged(new RecorderStatusEventArgs(entry.RecorderId, entry.ServiceId));
					OnCommunicationStateChange(new RecorderCommunicationStateEventArgs(entry.RecorderId, entry.ServiceId, entry.Status, entry.ConnectionStatus, entry.AttachStatus, entry.Status, recorderConnectionStatus, entry.AttachStatus));
				}
				break;
			}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Logger.LogError("CommunicationProvider_ConnectionChanged exception: {Exception}", ex2);
		}
	}

	public Guid RecorderAttachAsync(Guid serviceId, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		return recorderEntry.Sender.RecorderAttachAsync(callContextInfo);
	}

	public void TriggerCustomEventAsync(Guid serviceId, TriggerEvent triggerEvent, ICallContextInfo callContextInfo, bool usedInRules)
	{
		EventTracer.Log.TraceMethodStart("TriggerCustomEventAsync");
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		if (usedInRules || recorderEntry.SubscriptionOnGlobalEvents)
		{
			recorderEntry.Sender.TriggerCustomEventAsync(triggerEvent, callContextInfo);
		}
		EventTracer.Log.TraceMethodStop("TriggerCustomEventAsync");
	}

	public void TriggerCustomEventAsync(TriggerEvent triggerEvent, ICallContextInfo callContextInfo, bool usedInRules)
	{
		foreach (IRecorderEntry attachedRecorder in AttachedRecorders)
		{
			if (usedInRules || attachedRecorder.SubscriptionOnGlobalEvents)
			{
				attachedRecorder.Sender.TriggerCustomEventAsync(triggerEvent, callContextInfo);
			}
		}
	}

	public void TriggerEventAsGlobalAsync(Guid recorderId, Guid sourceRecorderId, TriggerEvent triggerEvent, ICallContextInfo callContextInfo, bool usedInRules)
	{
		bool flag = false;
		foreach (IRecorderEntry service in AttachedRecorders.GetServices(recorderId))
		{
			if (usedInRules || service.SubscriptionOnGlobalEvents)
			{
				service.Sender.TriggerEventAsGlobalAsync(sourceRecorderId, triggerEvent, callContextInfo);
			}
			flag = true;
		}
		if (!flag)
		{
			throw new RecorderNotFoundException(RecorderNotFoundExceptionMessage(recorderId));
		}
	}

	public Guid HardwareCancelScanAsync(Guid serviceId, Guid scanSessionId, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		return recorderEntry.Sender.HardwareCancelScanAsync(recorderEntry.RecorderId, scanSessionId, callContextInfo);
	}

	public void UpdateSystemConfigurationAsync(Guid serviceId, SystemConfigurationDto systemConfigurationDto, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.UpdateSystemConfigurationAsync(systemConfigurationDto, callContextInfo);
	}

	public void ReplaceHardwareUpdateSystemConfigurationAsync(Guid serviceId, SystemConfigurationDto systemConfigurationDto, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.ReplaceHardwareUpdateSystemConfigurationAsync(systemConfigurationDto, callContextInfo);
	}

	public void UpdateSecurityConfigurationAsync(Guid serviceId, SecurityConfigurationDto securityConfigurationDto, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.UpdateSecurityConfigurationAsync(securityConfigurationDto, callContextInfo);
	}

	public void ExecuteRuleActionsAsync(Guid recorderId, RuleActionExecutionRequest[] ruleActionRequests, DateTime senderTimeUtc, ICallContextInfo callContextInfo)
	{
		bool flag = false;
		foreach (IRecorderEntry service in AttachedRecorders.GetServices(recorderId))
		{
			service.Sender.ExecuteRuleActionsAsync(recorderId, ruleActionRequests, senderTimeUtc, callContextInfo);
			flag = true;
		}
		if (!flag)
		{
			throw new RecorderNotFoundException(RecorderNotFoundExceptionMessage(recorderId));
		}
	}

	public void UpdateUserSecurityTokensAsync(SecurityToken[] securityTokens, ICallContextInfo callContextInfo)
	{
		foreach (IRecorderEntry attachedRecorder in AttachedRecorders)
		{
			try
			{
				attachedRecorder.Sender.UpdateUserSecurityTokensAsync(securityTokens, callContextInfo);
			}
			catch (NotSupportedByLegacyRecorderException)
			{
			}
		}
	}

	public void SendHardwareSetupInformationCommand(Guid serviceId, HardwareSetupInformationCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendHardwareSetupInformationCommand(command, callContextInfo);
	}

	public void SendGetAbsolutePtzPositionCommand(Guid serviceId, GetAbsolutePtzPositionCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendGetAbsolutePtzPositionCommand(command, callContextInfo);
	}

	public void SendPtzReserveSessionCommand(Guid serviceId, PtzReserveSessionCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendGetPtzReserveSessionCommand(command, callContextInfo);
	}

	public void SendPtzReleaseSessionCommand(Guid serviceId, PtzReleaseSessionCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendPtzReleaseSessionCommand(command, callContextInfo);
	}

	public void SendPtzStopSessionCommand(Guid serviceId, PtzStopSessionCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendPtzStopSessionCommand(command, callContextInfo);
	}

	public void SendHardwareScanCommand(Guid serviceId, HardwareScanCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendHardwareScanCommand(command, callContextInfo);
	}

	public void SendSynchronizeDevicePresetsCommand(Guid serviceId, SynchronizeDevicePresetsCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendSynchronizeDevicePresetsCommand(command, callContextInfo);
	}

	public void SendDatabaseOperationCommand(Guid serviceId, DatabaseOperationCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendDatabaseOperationCommand(command, callContextInfo);
	}

	public void SendServerFileSystemOperationCommand(Guid serviceId, ServerFileSystemOperationCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendServerFileSystemOperationCommand(command, callContextInfo);
	}

	public void SendActivateArchivingCommand(Guid serviceId, ActivateArchivingCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendActivateArchivingCommand(command, callContextInfo);
	}

	public void SendPrepareMoveHardwareCommand(Guid serviceId, PrepareMoveHardwareCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendPrepareMoveHardwareCommand(command, callContextInfo);
	}

	public void SendUpdateSystemConfigurationCommand(Guid serviceId, UpdateSystemConfigurationCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendUpdateSystemConfigurationCommand(command, callContextInfo);
	}

	public void SendChangeHardwarePasswordCommand(Guid serviceId, ChangeHardwarePasswordCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendChangeHardwarePasswordCommand(command, callContextInfo);
	}

	public void SendGetHardwareNetworkSettingsCommand(Guid serviceId, GetHardwareNetworkSettingsCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendGetHardwareNetworkSettingsCommand(command, callContextInfo);
	}

	public void SendSetHardwareNetworkSettingsCommand(Guid serviceId, SetHardwareNetworkSettingsCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendSetHardwareNetworkSettingsCommand(command, callContextInfo);
	}

	public void SendGetHardwareUsersCommand(Guid serviceId, GetHardwareUsersCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendGetHardwareUsersCommand(command, callContextInfo);
	}

	public void SendPingDeviceCommand(Guid serviceId, PingDeviceCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendPingDeviceCommand(command, callContextInfo);
	}

	public void SendAddHardwareUserCommand(Guid serviceId, AddHardwareUserCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendAddHardwareUserCommand(command, callContextInfo);
	}

	public void SendDeleteHardwareUserCommand(Guid serviceId, DeleteHardwareUserCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendDeleteHardwareUserCommand(command, callContextInfo);
	}

	public void SendQueryDeviceEvidenceLockSizesCommand(Guid serviceId, QueryDeviceEvidenceLockSizesCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendQueryDeviceEvidenceLockSizesCommand(command, callContextInfo);
	}

	public void SendEvidenceLockOnOldRecorderCommand(Guid serviceId, EvidenceLockOnOldRecorderCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendEvidenceLockOnOldRecorderCommand(command, callContextInfo);
	}

	public void SendEvidenceLockOnOldRecorderCommand2(Guid serviceId, EvidenceLockOnOldRecorderCommand2 command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendEvidenceLockOnOldRecorderCommand2(command, callContextInfo);
	}

	public void SendSetupInitialHardwareUserCommand(Guid serviceId, SetupInitialHardwareUserCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendSetupInitialHardwareUserCommand(command, callContextInfo);
	}

	public void SendFirmwareUpgradeCommand(Guid serviceId, FirmwareUpgradeCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendFirmwareUpgradeCommand(command, callContextInfo);
	}

	public void SendFileUploadCommand(Guid serviceId, FileUploadCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendFileUploadCommand(command, callContextInfo);
	}

	public void SendCheckExternalRecorderRegistrationCommand(Guid serviceId, CheckExternalRecorderRegistrationCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendCheckExternalRecorderRegistrationCommand(command, callContextInfo);
	}

	public void SendFilePresentOnRecorderCommand(Guid serviceId, CheckFilePresentOnRecorderCommand command, ICallContextInfo callContextInfo)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Sender.SendFilePresentOnRecordingServerCommand(command, callContextInfo);
	}

	public void ProcessPendingTasks(Guid recorderId)
	{
		bool flag = false;
		foreach (IRecorderEntry service in AttachedRecorders.GetServices(recorderId))
		{
			try
			{
				service.Sender.ProcessPendingTasks();
			}
			catch (Exception inner)
			{
				throw new RecorderCommunicationException("An error occurred when communicating with the recording server.", inner);
			}
			flag = true;
		}
		if (!flag)
		{
			throw new RecorderNotFoundException(RecorderNotFoundExceptionMessage(recorderId));
		}
	}

	public void TryProcessPendingTasksAsync(Guid recorderId)
	{
		foreach (IRecorderEntry recorderEntry in AttachedRecorders.GetServices(recorderId))
		{
			Task.Run(delegate
			{
				try
				{
					recorderEntry.Sender.ProcessPendingTasks();
				}
				catch (Exception ex)
				{
					Logger.LogInformation("ProcessPendingTasks failed. Recorder: {RecorderId} ({ServiceId}), Exception: {Exception}", recorderEntry.RecorderId, recorderEntry.ServiceId, ex);
				}
			});
		}
	}

	private static string RecorderNotFoundExceptionMessage(Guid recorderId)
	{
		return string.Format(CultureInfo.InvariantCulture, "The recorder with the id {0} was not found.", recorderId);
	}

	public void ProcessReceivedResponse(Guid serviceId, ResponseBase response)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Reciever.ProcessReceivedResponse(response);
		if (response is SuccessResponse)
		{
			Logger.LogDebug("Response received. Type: {Type}; Token: {Token}", response.GetType().Name, response.Token);
		}
		else
		{
			Logger.LogInformation("Response received. Type: {Type}; Token: {Token}", response.GetType().Name, response.Token);
		}
		OnResponseReceived(new ResponseEventArgs(response));
	}

	public void TryAllRecordersProcessPendingTasksAsync()
	{
		foreach (IRecorderEntry recorderEntry in AttachedRecorders)
		{
			Task.Factory.StartNew(delegate
			{
				try
				{
					recorderEntry.Sender.ProcessPendingTasks();
				}
				catch (Exception ex)
				{
					Logger.LogInformation("ProcessPendingTasks failed. Recorder: {RecorderId} ({ServiceId}), Exception: {Exception}", recorderEntry.RecorderId, recorderEntry.ServiceId, ex);
				}
			}, TaskCreationOptions.LongRunning);
		}
	}

	public void ProcessReceivedCommand(Guid serviceId, CommandBase command)
	{
		Logger.LogInformation("Command received. Type: {Type}; Token: {Token}", command.GetType().Name, command.Token);
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Reciever.ProcessReceivedCommand(command, CreateRecorderCallingInfo());
	}

	public bool IsConfigurationCompatible(Guid serviceId)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		return recorderEntry.Reciever.IsConfigurationCompatible;
	}

	public ConfigurationVersionDto GetCurrentConfigurationVersions(Guid serviceId)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		return recorderEntry.Reciever.GetCurrentConfigurationVersions();
	}

	public CompleteConfigurationDto GetCompleteConfigurationDto(Guid serviceId, bool configDto, bool securityDto, bool tokensDto)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		return recorderEntry.Reciever.GetCompleteConfigurationDto(configDto, securityDto, tokensDto);
	}

	public SecurityTokenWithServerTimeDto2 GetSecurityToken(Guid serviceId, string token)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		return recorderEntry.Reciever.GetSecurityToken(token);
	}

	public ServerResponseStatusDto SetRecorderSpecificConfiguration(Guid serviceId, Guid targetRecorderId, RecorderSpecificConfigurationDto recorderSpecificConfiguration, bool forceUpdate)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		return recorderEntry.Reciever.SetRecorderSpecificConfiguration(targetRecorderId, recorderSpecificConfiguration, forceUpdate);
	}

	public void Log(Guid serviceId, LogEntry[] logEntries)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Reciever.Log(logEntries);
	}

	public ServerTaskDto[] GetRecorderTasks(Guid serviceId, long previousSequenceNumber)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		return recorderEntry.Reciever.GetRecorderTasks(previousSequenceNumber);
	}

	public ServerTaskDto[] GetRecorderTasksWithPendingStop(Guid serviceId)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		return recorderEntry.Reciever.GetRecorderTasksWithPendingStop();
	}

	public void UpdateRecorderStatusData(Guid serviceId, string infoType, string data)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.UpdateStatusData(infoType, data);
	}

	public void UpdateEventSubscriptionState(Guid serviceId, bool state)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.SubscriptionOnGlobalEvents = state;
	}

	public void RemoveRecorderTask(Guid serviceId, long taskId)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		try
		{
			recorderEntry.Reciever.RemoveRecorderTask(taskId);
		}
		catch (DatabaseException ex)
		{
			Logger.LogWarning("RemoveRecorderTask failed. Recorder: {RecorderId} ({ServiceId}), Exception: {Exception}", recorderEntry.RecorderId, recorderEntry.ServiceId, ex);
			throw;
		}
	}

	public void UpdateRecorderTaskProgress(Guid serviceId, long taskId, int percentComplete, string details, int status, DateTime endTime)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		try
		{
			recorderEntry.Reciever.UpdateRecorderTaskProgress(taskId, percentComplete, details, status, endTime, CreateRecorderCallingInfo());
		}
		catch (DatabaseException ex)
		{
			Logger.LogWarning("UpdateRecorderTaskProgress failed. Recorder: {RecorderId} ({ServiceId}), Exception: {Exception}", recorderEntry.RecorderId, recorderEntry.ServiceId, ex);
			throw;
		}
	}

	public void StopRecorderTask(Guid serviceId, long taskId)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		try
		{
			recorderEntry.Reciever.StopRecorderTask(taskId, CreateRecorderCallingInfo());
		}
		catch (DatabaseException ex)
		{
			Logger.LogWarning("StopRecorderTask failed. Recorder: {RecorderId} ({ServiceId}), Exception: {Exception}", recorderEntry.RecorderId, recorderEntry.ServiceId, ex);
			throw;
		}
	}

	public void PromoteNewEncryptionPassword(Guid serviceId, Guid recordingStorageId)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		recorderEntry.Reciever.PromoteNewEncryptionPassword(recordingStorageId);
	}

	public Guid[] FilterTrackIdsToThoseThatShouldNotHaveMediaOnRecorder(Guid serviceId, Guid targetRecorderId, Guid[] trackIds)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		return recorderEntry.Reciever.FilterTrackIdsToThoseThatShouldNotHaveMediaOnRecorder(targetRecorderId, trackIds);
	}

	public IDictionary<Guid, int> UpdateDrivers(Guid serviceId, IDictionary<Guid, int> drivers)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		return recorderEntry.Reciever.UpdateDriverNumbers(serviceId, drivers);
	}

	public RecorderStatusInfo[] GetRecorderStatus(Guid recorderId, Guid serviceId)
	{
		IEnumerable<RecorderStatusInfo> source = (from r in AttachedRecorders
			where r.RecorderId == recorderId || r.ServiceId == serviceId
			select new RecorderStatusInfo(r.RecorderId, r.ServiceId, r.Status, r.ConnectionStatus, r.AttachStatus)).DefaultIfEmpty(new RecorderStatusInfo(recorderId, serviceId, RecorderStatuses.None, RecorderConnectionStatus.NotConnected, RecorderAttachStatus.NotAttached));
		return source.ToArray();
	}

	public RecorderCompatibilityInfo GetRecorderCompatibilityStatus(Guid serviceId)
	{
		return AttachedRecorders.GetRecorderCompatibilityStatus(serviceId);
	}

	public bool IsRecorderAttachedAndConnected(Guid serviceId)
	{
		RecorderStatusInfo recorderStatus = AttachedRecorders.GetRecorderStatus(serviceId);
		if (recorderStatus != null && recorderStatus.AttachStatus == RecorderAttachStatus.Attached)
		{
			return recorderStatus.ConnectionStatus == RecorderConnectionStatus.Connected;
		}
		return false;
	}

	public bool IsRecorderOnline(Guid recorderId)
	{
		return AttachedRecorders.Any((IRecorderEntry r) => r.RecorderId == recorderId && r.AttachStatus == RecorderAttachStatus.Attached && r.ConnectionStatus == RecorderConnectionStatus.Connected);
	}

	public bool IsRecorderNotDisconnected(Guid recorderId)
	{
		return AttachedRecorders.Any((IRecorderEntry r) => r.RecorderId == recorderId && r.ConnectionStatus != RecorderConnectionStatus.ConnectionBroken && r.ConnectionStatus != RecorderConnectionStatus.NotConnected);
	}

	public bool IsRecorderAvailable(Guid recorderId)
	{
		return AttachedRecorders.Any((IRecorderEntry r) => r.RecorderId == recorderId);
	}

	public bool IsRecorderServiceAvailable(Guid serviceId)
	{
		return AttachedRecorders.Any((IRecorderEntry r) => r.ServiceId == serviceId);
	}

	public bool RecorderServerServiceExists(Guid serviceId)
	{
		return ConfigurationDataManager.FindRecorderIdFromServiceId(serviceId) != Guid.Empty;
	}

	public bool IsRecorderHotStandByFailoverType(Guid serviceId)
	{
		AttachedRecorders.TryGetRecorderEntry(serviceId, out var entry);
		if (entry != null)
		{
			return entry.ServiceType == RecordingServerServiceType.HotStandbyServer;
		}
		return false;
	}

	public bool IsRecorderIncompatible(Guid recorderId)
	{
		return AttachedRecorders.Any((IRecorderEntry r) => r.RecorderId == recorderId && !r.IsConfigurationCompatible);
	}

	public bool IsRecorderAttachedAndConnectedNotInFailover(Guid recorderId)
	{
		foreach (IRecorderEntry attachedRecorder in AttachedRecorders)
		{
			if (attachedRecorder.ServiceType == RecordingServerServiceType.RecordingServer && attachedRecorder.RecorderId == recorderId && attachedRecorder.ConnectionStatus == RecorderConnectionStatus.Connected && attachedRecorder.AttachStatus == RecorderAttachStatus.Attached)
			{
				return true;
			}
		}
		return false;
	}

	public RecorderStatusInfo[] GetRecorderStatusAllRecorders()
	{
		RecorderStatusInfo[] recorderStatus = AttachedRecorders.GetRecorderStatus();
		RecorderData[] outer;
		try
		{
			outer = RecorderData.QueryAllRecorders();
		}
		catch (DatabaseException ex)
		{
			Logger.LogWarning("Failover to query recorders from the database. Exception: {Exception}", ex);
			throw new DatabaseCommunicationException("Failover to query recorders from the database.", ex);
		}
		IEnumerable<RecorderStatusInfo> source = from r in outer
			join s in recorderStatus on r.ServiceId equals s.ServiceId into rs
			from result in rs.DefaultIfEmpty(new RecorderStatusInfo(r.Id, r.ServiceId, RecorderStatuses.None, RecorderConnectionStatus.NotConnected, RecorderAttachStatus.NotAttached))
			select result;
		return source.ToArray();
	}

	public Guid GetRecorderId(Guid serviceId)
	{
		IRecorderEntry recorderEntry = AttachedRecorders.GetRecorderEntry(serviceId);
		return recorderEntry.RecorderId;
	}

	public Guid QueryRecorderServiceId(Guid recorderId)
	{
		return AttachedRecorders.FirstOrDefault((IRecorderEntry r) => r.RecorderId == recorderId && r.ServiceType == RecordingServerServiceType.RecordingServer)?.ServiceId ?? default(Guid);
	}

	public Guid[] QueryConnectedRecorderIds()
	{
		IEnumerable<Guid> source = from recorderEntry in AttachedRecorders
			where recorderEntry.ServiceType == RecordingServerServiceType.RecordingServer && recorderEntry.AttachStatus == RecorderAttachStatus.Attached && recorderEntry.ConnectionStatus == RecorderConnectionStatus.Connected
			select recorderEntry.RecorderId;
		return source.ToArray();
	}

	public Guid[] QueryHotstandbyServiceIds(Guid recorderId)
	{
		IEnumerable<Guid> source = from recorderEntry in AttachedRecorders
			where recorderEntry.RecorderId == recorderId && recorderEntry.ServiceType == RecordingServerServiceType.HotStandbyServer
			select recorderEntry.ServiceId;
		return source.ToArray();
	}

	public Guid[] QueryRecorderServiceIds(Guid recorderId)
	{
		IEnumerable<Guid> source = from recorderEntry in AttachedRecorders.GetServices(recorderId)
			select recorderEntry.ServiceId;
		return source.ToArray();
	}

	public Guid[] QueryRecorderServiceIds()
	{
		IEnumerable<Guid> source = AttachedRecorders.Select((IRecorderEntry recorderEntry) => recorderEntry.ServiceId);
		return source.ToArray();
	}

	public Guid QueryActiveRecorderServiceId(Guid recorderId)
	{
		IEnumerable<IRecorderEntry> source = AttachedRecorders.Where((IRecorderEntry r) => r.AttachStatus == RecorderAttachStatus.Attached && r.ConnectionStatus == RecorderConnectionStatus.Connected && r.RecorderId == recorderId);
		if (!source.Any())
		{
			return Guid.Empty;
		}
		return ((source.Count() > 1) ? source.SingleOrDefault((IRecorderEntry r) => r.ServiceType == RecordingServerServiceType.RecordingServer) : source.Single())?.ServiceId ?? Guid.Empty;
	}

	public Guid[] GetConnectedRecordingServerServiceIds()
	{
		return (from r in AttachedRecorders
			where r.ServiceType == RecordingServerServiceType.RecordingServer && IsRecorderAttachedAndConnected(r.ServiceId)
			select r.ServiceId).ToList().ToArray();
	}

	private static ICallContextInfo CreateRecorderCallingInfo()
	{
		string locationIPAddress = Dns.GetHostName();
		try
		{
			locationIPAddress = IPHelper.DnsGetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
		return new BasicCallContextInfo(Server.Instance.ProcessIdentity.User.ToString(), Server.Instance.ProcessIdentity.Name, locationIPAddress);
	}

	public void UpdateServerRecorderIPv6Comm(bool useIPv6)
	{
		try
		{
			using ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(Server.Data));
			if (configurationManager.SystemSettings.TryGetSetting(SettingList.ServerRecorderIPv6Comm, out var setting))
			{
				Server.Data.ConfigurationDataSetCacheLock.EnterWriteLock();
				try
				{
					setting.Value = useIPv6.ToString();
					ICallContextInfo callContextInfo = Server.CreateServerServiceCallContextUserInfo();
					ConfigurationDataSet changes = configurationManager.Dataset.GetChanges();
					Server.Data.WriteConfigurationDataSetChangesToDatabase(changes, callContextInfo);
					ILicenseManagerCache requiredService = ServiceContainer.Instance.GetRequiredService<ILicenseManagerCache>();
					requiredService.ClearAllCaches();
					return;
				}
				finally
				{
					Server.Data.ConfigurationDataSetCacheLock.ExitWriteLock();
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}

	public bool IsIPv6Supported()
	{
		bool result = false;
		using (ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(Server.Data)))
		{
			if (configurationManager.SystemSettings.TryGetSetting(SettingList.ServerRecorderIPv6Comm, out var setting))
			{
				try
				{
					result = setting.ValueAsBool;
				}
				catch (FormatException)
				{
				}
			}
		}
		return result;
	}

	public void Close(ServerConfig serverConfig)
	{
		if (_webApiCommnucationProvider != null)
		{
			_webApiCommnucationProvider.Close();
		}
	}

	public IServiceConnectionStatus GetService(Guid serviceId)
	{
		if (AttachedRecorders.TryGetRecorderEntry(serviceId, out var entry))
		{
			return entry.ServiceConnectionStatus;
		}
		return null;
	}

	public IServiceConnectionStatus[] GetServices()
	{
		List<IServiceConnectionStatus> list = new List<IServiceConnectionStatus>();
		IEnumerator<IRecorderEntry> enumerator = AttachedRecorders.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.ServiceConnectionStatus != null)
			{
				list.Add(enumerator.Current.ServiceConnectionStatus);
			}
		}
		return list.ToArray();
	}
}
