using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.XPath;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Security;
using VideoOS.Rules.Interfaces;
using VideoOS.Utilities.Validation;

namespace VideoOS.Server.RecorderCommunication;

public abstract class RecorderEntryBase : IRecorderSend, IRecorderReceive, IRecorderEntry, IDisposable, IServerConnectionProvider
{
	private const int DriverFrameworkReserved = 50000;

	private ConfigurationVersionDto currentConfigurationVersion;

	protected IThreadPoolMessageProcessor<CommandPackage> _sendCommandSender;

	protected IThreadPoolMessageProcessor<ResponseBase> _sendResponseSender;

	private bool isDisposed;

	protected Server _server;

	protected virtual bool EntryIsCompatible => false;

	public virtual bool IsConfigurationCompatible => false;

	public virtual bool SubscriptionOnGlobalEvents
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public Guid ServiceId { get; set; }

	public Guid RecorderId { get; set; }

	public string RecorderUri { get; set; }

	public RecorderStatuses Status { get; set; }

	public RecorderConnectionStatus ConnectionStatus { get; set; }

	public RecorderAttachStatus AttachStatus { get; set; }

	public RecordingServerServiceType ServiceType { get; set; }

	public IRecorderSend Sender => this;

	public IRecorderReceive Reciever => this;

	public IServiceConnectionStatus ServiceConnectionStatus { get; set; }

	public bool IsOnline => ConnectionStatus == RecorderConnectionStatus.Connected;

	public RecorderEntryBase(Guid recorderId, Guid serviceId, RecordingServerServiceType serviceType, string recorderUri, RecorderConnectionStatus connectionStatus, RecorderAttachStatus attachStatus, RecorderStatuses recorderStatus)
	{
		SetupRecorderEntry(recorderId, serviceId, serviceType, recorderUri, connectionStatus, attachStatus, recorderStatus);
	}

	private void SetupRecorderEntry(Guid recorderId, Guid serviceId, RecordingServerServiceType serviceType, string recorderUri, RecorderConnectionStatus connectionStatus, RecorderAttachStatus attachStatus, RecorderStatuses recorderStatus)
	{
		_server = Server.Instance;
		RecorderId = recorderId;
		ServiceId = serviceId;
		ServiceType = serviceType;
		RecorderUri = recorderUri;
		ConnectionStatus = connectionStatus;
		AttachStatus = attachStatus;
		Status = recorderStatus;
		currentConfigurationVersion = new ConfigurationVersionDto(0L, 0L, 0L);
	}

	~RecorderEntryBase()
	{
		if (!isDisposed)
		{
			Dispose();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		isDisposed = true;
		if (disposing)
		{
			_sendCommandSender.Stop();
			_sendResponseSender.Stop();
		}
	}

	protected Guid QueueCommand(CommandPackage commandPackage)
	{
		_sendCommandSender.QueueMessage(commandPackage);
		return commandPackage.CommandBase.Token;
	}

	protected void ProcessMessageBase(CommandPackage commandPackage)
	{
		bool flag = false;
		long lastVersion = 0L;
		long nextVersion = 0L;
		lock (currentConfigurationVersion)
		{
			if (commandPackage.CommandBase is UpdateSystemConfigurationCommand)
			{
				UpdateSystemConfigurationCommand updateSystemConfigurationCommand = commandPackage.CommandBase as UpdateSystemConfigurationCommand;
				updateSystemConfigurationCommand.SystemConfigurationDto.LastUpdate = currentConfigurationVersion.ConfigVersion;
				lastVersion = currentConfigurationVersion.ConfigVersion;
				currentConfigurationVersion.ConfigVersion++;
				updateSystemConfigurationCommand.SystemConfigurationDto.NextUpdate = currentConfigurationVersion.ConfigVersion;
				nextVersion = currentConfigurationVersion.ConfigVersion;
				flag = true;
			}
			if (commandPackage.CommandBase is UpdateSecurityManagerCommand)
			{
				UpdateSecurityManagerCommand updateSecurityManagerCommand = commandPackage.CommandBase as UpdateSecurityManagerCommand;
				updateSecurityManagerCommand.SecurityManagerDto.LastUpdate = currentConfigurationVersion.SecurityVersion;
				lastVersion = currentConfigurationVersion.SecurityVersion;
				currentConfigurationVersion.SecurityVersion++;
				updateSecurityManagerCommand.SecurityManagerDto.NextUpdate = currentConfigurationVersion.SecurityVersion;
				nextVersion = currentConfigurationVersion.SecurityVersion;
				flag = true;
			}
			if (commandPackage.CommandBase is SetUserSecurityTokenCommand)
			{
				SetUserSecurityTokenCommand setUserSecurityTokenCommand = commandPackage.CommandBase as SetUserSecurityTokenCommand;
				setUserSecurityTokenCommand.SecurityTokensDto.LastUpdate = currentConfigurationVersion.TokenVersion;
				lastVersion = currentConfigurationVersion.TokenVersion;
				currentConfigurationVersion.TokenVersion++;
				setUserSecurityTokenCommand.SecurityTokensDto.NextUpdate = currentConfigurationVersion.TokenVersion;
				nextVersion = currentConfigurationVersion.TokenVersion;
				flag = true;
			}
		}
		if (flag)
		{
			LogCommand(commandPackage.CommandBase, RecorderId, lastVersion, nextVersion);
		}
		else
		{
			LogCommand(commandPackage.CommandBase, RecorderId);
		}
	}

	private static void LogCommand(CommandBase command, Guid recorderId, long lastVersion, long nextVersion)
	{
		RecorderCommManager.Logger.LogInformation("Sending command. Recorder: {RecorderId}; Type: {Type}; Token: {Token}; (ver {LastVersion}->{NextVersion})", recorderId, command.GetType().Name, command.Token, lastVersion, nextVersion);
	}

	private static void LogCommand(CommandBase command, Guid recorderId)
	{
		if (command is TriggerEventAsGlobalCommand || command is TriggerCustomEventCommand || command is TriggerCustomEventBulkCommand)
		{
			RecorderCommManager.Logger.LogDebug("Sending command. Recorder: {RecorderId}; Type: {Type}; Token: {Token}", recorderId, command.GetType().Name, command.Token);
		}
		else
		{
			RecorderCommManager.Logger.LogInformation("Sending command. Recorder: {RecorderId}; Type: {Type}; Token: {Token}", recorderId, command.GetType().Name, command.Token);
		}
	}

	public virtual void ReleaseResources()
	{
	}

	protected void QueueResponse(ResponseBase response)
	{
		_sendResponseSender.QueueMessage(response);
	}

	public abstract Guid RecorderAttachAsync(ICallContextInfo callContextInfo);

	public abstract Guid TriggerCustomEventAsync(TriggerEvent triggerEvent, ICallContextInfo callContextInfo);

	public abstract Guid TriggerEventAsGlobalAsync(Guid recorderSourceId, TriggerEvent triggerEvent, ICallContextInfo callContextInfo);

	public virtual Guid HardwareCancelScanAsync(Guid recorderId, Guid scanSessionId, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public abstract Guid UpdateSystemConfigurationAsync(SystemConfigurationDto systemConfiguration, ICallContextInfo callContextInfo);

	public abstract Guid ReplaceHardwareUpdateSystemConfigurationAsync(SystemConfigurationDto systemConfiguration, ICallContextInfo callContextInfo);

	public abstract Guid UpdateSecurityConfigurationAsync(SecurityConfigurationDto securityConfigurationDto, ICallContextInfo callContextInfo);

	public abstract Guid ExecuteRuleActionsAsync(Guid recorderId, RuleActionExecutionRequest[] ruleActionRequests, DateTime senderTimeUtc, ICallContextInfo callContextInfo);

	public abstract Guid UpdateUserSecurityTokensAsync(SecurityToken[] securityTokens, ICallContextInfo callContextInfo);

	public abstract void ProcessPendingTasks();

	public virtual void SendUpdateSystemConfigurationCommand(UpdateSystemConfigurationCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public abstract void SendHardwareSetupInformationCommand(HardwareSetupInformationCommand command, ICallContextInfo callContextInfo);

	public virtual void SendHardwareScanCommand(HardwareScanCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public virtual void SendGetAbsolutePtzPositionCommand(GetAbsolutePtzPositionCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public virtual void SendGetPtzReserveSessionCommand(PtzReserveSessionCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public virtual void SendPtzReleaseSessionCommand(PtzReleaseSessionCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public virtual void SendPtzStopSessionCommand(PtzStopSessionCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public abstract void SendSynchronizeDevicePresetsCommand(SynchronizeDevicePresetsCommand command, ICallContextInfo callContextInfo);

	public abstract void SendDatabaseOperationCommand(DatabaseOperationCommand command, ICallContextInfo callContextInfo);

	public abstract void SendServerFileSystemOperationCommand(ServerFileSystemOperationCommand command, ICallContextInfo callContextInfo);

	public abstract void SendActivateArchivingCommand(ActivateArchivingCommand command, ICallContextInfo callContextInfo);

	public abstract void SendPrepareMoveHardwareCommand(PrepareMoveHardwareCommand command, ICallContextInfo callContextInfo);

	public abstract void SendChangeHardwarePasswordCommand(ChangeHardwarePasswordCommand command, ICallContextInfo callContextInfo);

	public abstract void SendSetHardwareNetworkSettingsCommand(SetHardwareNetworkSettingsCommand command, ICallContextInfo callContextInfo);

	public abstract void SendGetHardwareNetworkSettingsCommand(GetHardwareNetworkSettingsCommand command, ICallContextInfo callContextInfo);

	public abstract void SendGetHardwareUsersCommand(GetHardwareUsersCommand command, ICallContextInfo callContextInfo);

	public abstract void SendPingDeviceCommand(PingDeviceCommand command, ICallContextInfo callContextInfo);

	public abstract void SendAddHardwareUserCommand(AddHardwareUserCommand command, ICallContextInfo callContextInfo);

	public abstract void SendDeleteHardwareUserCommand(DeleteHardwareUserCommand command, ICallContextInfo callContextInfo);

	public abstract void SendQueryDeviceEvidenceLockSizesCommand(QueryDeviceEvidenceLockSizesCommand command, ICallContextInfo callContextInfo);

	public abstract void SendEvidenceLockOnOldRecorderCommand(EvidenceLockOnOldRecorderCommand command, ICallContextInfo callContextInfo);

	public abstract void SendEvidenceLockOnOldRecorderCommand2(EvidenceLockOnOldRecorderCommand2 command, ICallContextInfo callContextInfo);

	public abstract void SendSetupInitialHardwareUserCommand(SetupInitialHardwareUserCommand command, ICallContextInfo callContextInfo);

	public virtual void SendFirmwareUpgradeCommand(FirmwareUpgradeCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public virtual void SendFileUploadCommand(FileUploadCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public virtual void SendFilePresentOnRecordingServerCommand(CheckFilePresentOnRecorderCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public virtual void SendCheckExternalRecorderRegistrationCommand(CheckExternalRecorderRegistrationCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public virtual bool IsConnectionAlive()
	{
		return true;
	}

	public abstract void SendResponse(ResponseBase response);

	public ConfigurationVersionDto GetCurrentConfigurationVersions()
	{
		lock (currentConfigurationVersion)
		{
			return currentConfigurationVersion.Clone();
		}
	}

	public abstract ServerTaskDto[] GetRecorderTasks(long previousSequenceNumber);

	protected ServerTaskDto[] GetRecorderTasksBase(long previousSequenceNumber)
	{
		ServerTaskDto[] result = null;
		try
		{
			result = _server.GetRecorderTasks(ServiceId, previousSequenceNumber);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			RecorderCommManager.Logger.LogError("{Exception}", ex2);
		}
		return result;
	}

	public abstract ServerTaskDto[] GetRecorderTasksWithPendingStop();

	protected ServerTaskDto[] GetRecorderTasksWithPendingStopBase()
	{
		ServerTaskDto[] result = null;
		try
		{
			result = Server.GetRecorderTasksWithPendingStop(ServiceId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			RecorderCommManager.Logger.LogError("{Exception}", ex2);
		}
		return result;
	}

	public abstract void UpdateStatusData(string infoType, string data);

	protected void UpdateStatusDataBase(string infoType, string data)
	{
		try
		{
			Server.UpdateRecorderStatusData(RecorderId, ServiceId, infoType, data);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			RecorderCommManager.Logger.LogError("{Exception}", ex2);
		}
	}

	public virtual void ProcessReceivedCommand(CommandBase command, ICallContextInfo callContextInfo)
	{
		CommandProcessor.ProcessAsync(command, callContextInfo, SendResponseCallback);
	}

	public void ProcessReceivedResponse(ResponseBase response)
	{
	}

	private void SendResponseCallback(CommandBase command, ResponseBase response)
	{
		SendResponse(response);
	}

	public SecurityTokenWithServerTimeDto2 GetSecurityToken(string token)
	{
		return _server.GetSecurityTokenDtoWithManagementClientToken(token);
	}

	public virtual CompleteConfigurationDto GetCompleteConfigurationDto(bool configDto, bool securityDto, bool tokensDto)
	{
		return _server.GetCompleteConfigurationDto(ServiceId, configDto, securityDto, tokensDto: false);
	}

	public ServerResponseStatusDto SetRecorderSpecificConfiguration(Guid targetRecorderId, RecorderSpecificConfigurationDto recorderSpecificConfiguration, bool forceUpdate)
	{
		return _server.SetRecorderSpecificConfiguration(targetRecorderId, recorderSpecificConfiguration, forceUpdate, _server.CreateServerServiceCallContextUserInfo());
	}

	public void Log(LogEntry[] logEntries)
	{
		_server.LogInternal(logEntries);
	}

	public void RemoveRecorderTask(long taskId)
	{
		_server.RemoveRecorderTask(taskId);
	}

	public void UpdateRecorderTaskProgress(long taskId, int percentComplete, string details, int status, DateTime endTime, ICallContextInfo callContext)
	{
		string text = _server.UpdateRecorderTaskProgress(taskId, percentComplete, details, status, endTime);
		using StringReader textReader = new StringReader(text);
		XPathDocument xPathDocument = new XPathDocument(textReader);
		XPathNavigator xPathNavigator = xPathDocument.CreateNavigator();
		xPathNavigator = xPathNavigator.SelectSingleNode("task");
		string attribute = xPathNavigator.GetAttribute("type", string.Empty);
		if (attribute.StartsWith("Edge.", StringComparison.OrdinalIgnoreCase))
		{
			EdgeTaskCommandBase edgeTaskCommandBase = EdgeTaskCommandBase.CreateCommandFromXml(text, stopping: false);
			edgeTaskCommandBase.Token = Guid.Empty;
			ProcessReceivedCommand(edgeTaskCommandBase, callContext);
		}
	}

	public void StopRecorderTask(long taskId, ICallContextInfo callContext)
	{
		string text = Server.StopRecorderTask(taskId);
		using StringReader textReader = new StringReader(text);
		XPathDocument xPathDocument = new XPathDocument(textReader);
		XPathNavigator xPathNavigator = xPathDocument.CreateNavigator();
		xPathNavigator = xPathNavigator.SelectSingleNode("task");
		string attribute = xPathNavigator.GetAttribute("type", string.Empty);
		if (attribute.StartsWith("Edge.", StringComparison.OrdinalIgnoreCase))
		{
			EdgeTaskCommandBase edgeTaskCommandBase = EdgeTaskCommandBase.CreateCommandFromXml(text, stopping: true);
			edgeTaskCommandBase.Token = Guid.Empty;
			ProcessReceivedCommand(edgeTaskCommandBase, callContext);
		}
	}

	public void PromoteNewEncryptionPassword(Guid recordingStorageId)
	{
		_server.PromoteNewEncryptionPassword(recordingStorageId);
	}

	public Guid[] FilterTrackIdsToThoseThatShouldNotHaveMediaOnRecorder(Guid recorderId, Guid[] trackIds)
	{
		return Server.FilterTrackIdsToThoseThatShouldNotHaveMediaOnRecorder(recorderId, trackIds);
	}

	public IDictionary<Guid, int> UpdateDriverNumbers(Guid serviceId, IDictionary<Guid, int> driverNumbers)
	{
		driverNumbers.NullCheck("", "UpdateDriverNumbers", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Server\\RecorderCommunication\\CommunicationProvider\\RecorderEntryBase.cs", 494);
		Dictionary<Guid, int> toUpdate = driverNumbers.Keys.Where((Guid x) => driverNumbers[x] == 50000).ToDictionary((Guid key) => key, (Guid key) => driverNumbers[key]);
		return _server.UpdateDriverNumbers(toUpdate);
	}
}
