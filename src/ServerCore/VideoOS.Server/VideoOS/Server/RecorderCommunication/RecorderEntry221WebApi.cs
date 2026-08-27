using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Security;
using VideoOS.Common.Integration.WebApi;
using VideoOS.Common.Integration.WebApi.Models.Recorder;
using VideoOS.Rules.Interfaces;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server.RecorderCommunication;

public sealed class RecorderEntry221WebApi : WebApiRecorderEntryBase
{
	private ICommunicator _communicator;

	private bool _eventSubscriptionState;

	private int isBusy;

	private int keepRunning;

	protected override bool EntryIsCompatible => false;

	public override bool IsConfigurationCompatible => EntryIsCompatible;

	public override bool SubscriptionOnGlobalEvents
	{
		get
		{
			return _eventSubscriptionState;
		}
		set
		{
			_eventSubscriptionState = value;
		}
	}

	public RecorderEntry221WebApi(Guid recorderId, Uri recorderAddress, Guid serviceId, RecordingServerServiceType serviceType, RecorderConnectionStatus connectionStatus, RecorderAttachStatus attachStatus, RecorderStatuses recorderStatus)
		: base(recorderId, serviceId, serviceType, recorderAddress, connectionStatus, attachStatus, recorderStatus)
	{
		ServerAccessTokenHandler accessTokenHandler = new ServerAccessTokenHandler(Server.Instance.ServerTokenCache);
		HttpClientCommunicator communicator = new HttpClientCommunicator(new Uri(base.RecorderUri), Telemetry.GetILogger("Server"), accessTokenHandler, base.ServiceConnectionStatus);
		_communicator = new HttpClientCommunicatorWrapper(communicator);
	}

	public override Guid RecorderAttachAsync(ICallContextInfo callContextInfo)
	{
		return QueueCommand(new CommandPackage(new RecorderAttachCommand(), callContextInfo));
	}

	public override Guid TriggerCustomEventAsync(TriggerEvent triggerEvent, ICallContextInfo callContextInfo)
	{
		return QueueCommand(new CommandPackage(new TriggerCustomEventCommand(triggerEvent), callContextInfo));
	}

	public override Guid TriggerEventAsGlobalAsync(Guid recorderSourceId, TriggerEvent triggerEvent, ICallContextInfo callContextInfo)
	{
		return QueueCommand(new CommandPackage(new TriggerEventAsGlobalCommand(recorderSourceId, triggerEvent), callContextInfo));
	}

	public override Guid HardwareCancelScanAsync(Guid recorderId, Guid scanSessionId, ICallContextInfo callContextInfo)
	{
		HardwareScanCommand hardwareScanCommand = new HardwareScanCommand();
		hardwareScanCommand.CancelScanSession(recorderId, scanSessionId);
		return QueueCommand(new CommandPackage(hardwareScanCommand, callContextInfo));
	}

	public override Guid UpdateSystemConfigurationAsync(SystemConfigurationDto systemConfiguration, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		return QueueCommand(new CommandPackage(new UpdateSystemConfigurationCommand(systemConfiguration), callContextInfo));
	}

	public override Guid ReplaceHardwareUpdateSystemConfigurationAsync(SystemConfigurationDto systemConfiguration, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		return QueueCommand(new CommandPackage(new ReplaceHardwareUpdateSystemConfigurationCommand(systemConfiguration), callContextInfo));
	}

	public override Guid UpdateSecurityConfigurationAsync(SecurityConfigurationDto securityConfigurationDto, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		return QueueCommand(new CommandPackage(new UpdateSecurityConfigurationCommand(securityConfigurationDto), callContextInfo));
	}

	public override Guid ExecuteRuleActionsAsync(Guid recorderId, RuleActionExecutionRequest[] ruleActionRequests, DateTime senderTimeUtc, ICallContextInfo callContextInfo)
	{
		ExecuteRuleActionsCommand executeRuleActionsCommand = new ExecuteRuleActionsCommand(recorderId, ruleActionRequests);
		executeRuleActionsCommand.SenderUtcTime = senderTimeUtc;
		return QueueCommand(new CommandPackage(executeRuleActionsCommand, callContextInfo));
	}

	public override Guid UpdateUserSecurityTokensAsync(SecurityToken[] securityTokens, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		SecurityTokenCollectionDto2 securityTokensDto = SecurityTokenCollectionDto2.Create(securityTokens);
		return QueueCommand(new CommandPackage(new UpdateUserSecurityTokensCommand(securityTokensDto), callContextInfo));
	}

	public override void SendUpdateSystemConfigurationCommand(UpdateSystemConfigurationCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendHardwareSetupInformationCommand(HardwareSetupInformationCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendGetAbsolutePtzPositionCommand(GetAbsolutePtzPositionCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendGetPtzReserveSessionCommand(PtzReserveSessionCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendPtzReleaseSessionCommand(PtzReleaseSessionCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendPtzStopSessionCommand(PtzStopSessionCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendHardwareScanCommand(HardwareScanCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendSynchronizeDevicePresetsCommand(SynchronizeDevicePresetsCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendDatabaseOperationCommand(DatabaseOperationCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendServerFileSystemOperationCommand(ServerFileSystemOperationCommand command, ICallContextInfo callContextInfo)
	{
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendActivateArchivingCommand(ActivateArchivingCommand command, ICallContextInfo callContextInfo)
	{
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendPrepareMoveHardwareCommand(PrepareMoveHardwareCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendChangeHardwarePasswordCommand(ChangeHardwarePasswordCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendSetHardwareNetworkSettingsCommand(SetHardwareNetworkSettingsCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendGetHardwareNetworkSettingsCommand(GetHardwareNetworkSettingsCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendGetHardwareUsersCommand(GetHardwareUsersCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendPingDeviceCommand(PingDeviceCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendAddHardwareUserCommand(AddHardwareUserCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendDeleteHardwareUserCommand(DeleteHardwareUserCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendSetupInitialHardwareUserCommand(SetupInitialHardwareUserCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendFirmwareUpgradeCommand(FirmwareUpgradeCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendFileUploadCommand(FileUploadCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendFilePresentOnRecordingServerCommand(CheckFilePresentOnRecorderCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendCheckExternalRecorderRegistrationCommand(CheckExternalRecorderRegistrationCommand command, ICallContextInfo callContextInfo)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendQueryDeviceEvidenceLockSizesCommand(QueryDeviceEvidenceLockSizesCommand command, ICallContextInfo callContextInfo)
	{
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendEvidenceLockOnOldRecorderCommand(EvidenceLockOnOldRecorderCommand command, ICallContextInfo callContextInfo)
	{
		QueueCommand(new CommandPackage(command, callContextInfo));
	}

	public override void SendEvidenceLockOnOldRecorderCommand2(EvidenceLockOnOldRecorderCommand2 command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void ProcessPendingTasks()
	{
		Interlocked.Exchange(ref keepRunning, 1);
		if (Interlocked.Exchange(ref isBusy, 1) != 0)
		{
			return;
		}
		try
		{
			while (Interlocked.Exchange(ref keepRunning, 0) == 1)
			{
				_communicator.Send("api/commands/recorder/processpendingtasks");
			}
		}
		finally
		{
			Interlocked.Exchange(ref isBusy, 0);
			if (Interlocked.Exchange(ref keepRunning, 0) == 1)
			{
				ProcessPendingTasks();
			}
		}
	}

	public override void SendResponse(ResponseBase response)
	{
		QueueResponse(response);
	}

	public override ServerTaskDto[] GetRecorderTasks(long previousSequenceNumber)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		return GetRecorderTasksBase(previousSequenceNumber);
	}

	public override ServerTaskDto[] GetRecorderTasksWithPendingStop()
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		return GetRecorderTasksWithPendingStopBase();
	}

	public override void UpdateStatusData(string infoType, string data)
	{
		if (!EntryIsCompatible)
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		UpdateStatusDataBase(infoType, data);
	}

	protected override async Task SendCommandToRecorderAsync(CommandBase command, CommunicationCallContextDto callContextInfo)
	{
		bool incompatible = !EntryIsCompatible;
		if (!(await CommandTranslator221WebApi.Translate(_communicator, command, callContextInfo, incompatible)))
		{
			RecorderCommManager.Logger.LogError("An unsupported command was attempted sent to a recorder. Recorder: {RecorderId}; Type: {Type}; Token: {Token}", base.RecorderId, command.GetType().Name, command.Token);
		}
	}

	protected override async Task SendResponseToRecorderAsync(ResponseBase response)
	{
		if (!(await ResponseTranslator221WebApi.TranslateAsync(response, _communicator)))
		{
			RecorderCommManager.Logger.LogError("An unsupported response was attempted sent to the recorder. Type: {Type}; Token: {Token}", response.GetType().Name, response.Token);
		}
	}

	public override bool IsConnectionAlive()
	{
		try
		{
			return _communicator.Receive<RecorderStatusModel>("api/recorder/status").IsRunning;
		}
		catch (WebApiException)
		{
			return false;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && _communicator != null)
		{
			_communicator.Dispose();
		}
		base.Dispose(disposing);
	}
}
