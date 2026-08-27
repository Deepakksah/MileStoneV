using System;
using System.Linq;
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

public sealed class RecorderEntry132WebApi : WebApiRecorderEntryBase
{
	private ICommunicator _communicator;

	private bool _eventSubscriptionState;

	private int isBusy;

	private int keepRunning;

	private Type[] SupportedCommands = new Type[14]
	{
		typeof(EventSubscriptionStateChangedCommand),
		typeof(SendMultiNotificationCommand),
		typeof(SetMatrixViewCommand),
		typeof(VideoWallCommand),
		typeof(ReportMediaStatusCommand),
		typeof(ReportManualRecordingStateCommand),
		typeof(AddBookmarkCommand),
		typeof(UpdateRecorderStatusCommand),
		typeof(ExecuteRuleActionsCommand),
		typeof(TriggerEventAsGlobalCommand),
		typeof(ActivateUserDefinedEventOnRemoteSiteCommand),
		typeof(ActivateArchivingCommand),
		typeof(MIPActionExecuteCommand),
		typeof(MIPAction2ExecuteCommand)
	};

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

	public RecorderEntry132WebApi(Guid recorderId, Uri recorderAddress, Guid serviceId, RecordingServerServiceType serviceType, RecorderConnectionStatus connectionStatus, RecorderAttachStatus attachStatus, RecorderStatuses recorderStatus)
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
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override Guid ReplaceHardwareUpdateSystemConfigurationAsync(SystemConfigurationDto systemConfiguration, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override Guid UpdateSecurityConfigurationAsync(SecurityConfigurationDto securityConfigurationDto, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override Guid ExecuteRuleActionsAsync(Guid recorderId, RuleActionExecutionRequest[] ruleActionRequests, DateTime senderTimeUtc, ICallContextInfo callContextInfo)
	{
		ExecuteRuleActionsCommand executeRuleActionsCommand = new ExecuteRuleActionsCommand(recorderId, ruleActionRequests);
		executeRuleActionsCommand.SenderUtcTime = senderTimeUtc;
		return QueueCommand(new CommandPackage(executeRuleActionsCommand, callContextInfo));
	}

	public override Guid UpdateUserSecurityTokensAsync(SecurityToken[] securityTokens, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendUpdateSystemConfigurationCommand(UpdateSystemConfigurationCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendHardwareSetupInformationCommand(HardwareSetupInformationCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendGetAbsolutePtzPositionCommand(GetAbsolutePtzPositionCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendGetPtzReserveSessionCommand(PtzReserveSessionCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendPtzReleaseSessionCommand(PtzReleaseSessionCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendPtzStopSessionCommand(PtzStopSessionCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendHardwareScanCommand(HardwareScanCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendSynchronizeDevicePresetsCommand(SynchronizeDevicePresetsCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendDatabaseOperationCommand(DatabaseOperationCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
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
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendChangeHardwarePasswordCommand(ChangeHardwarePasswordCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendSetHardwareNetworkSettingsCommand(SetHardwareNetworkSettingsCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendGetHardwareNetworkSettingsCommand(GetHardwareNetworkSettingsCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendDeleteHardwareUserCommand(DeleteHardwareUserCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendSetupInitialHardwareUserCommand(SetupInitialHardwareUserCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendGetHardwareUsersCommand(GetHardwareUsersCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendPingDeviceCommand(PingDeviceCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendAddHardwareUserCommand(AddHardwareUserCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendQueryDeviceEvidenceLockSizesCommand(QueryDeviceEvidenceLockSizesCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void SendEvidenceLockOnOldRecorderCommand(EvidenceLockOnOldRecorderCommand command, ICallContextInfo callContextInfo)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
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
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override ServerTaskDto[] GetRecorderTasksWithPendingStop()
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	public override void UpdateStatusData(string infoType, string data)
	{
		throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
	}

	protected override async Task SendCommandToRecorderAsync(CommandBase command, CommunicationCallContextDto callContextInfo)
	{
		bool incompatible = true;
		if (!(await CommandTranslator132WebApi.Translate(_communicator, command, callContextInfo, incompatible)))
		{
			RecorderCommManager.Logger.LogError("An unsupported command was attempted sent to a recorder. Recorder: {RecorderId}; Type: {Type}; Token: {Token}", base.RecorderId, command.GetType().Name, command.Token);
		}
	}

	protected override async Task SendResponseToRecorderAsync(ResponseBase response)
	{
		if (!(await ResponseTranslator132WebApi.TranslateAsync(response, _communicator)))
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

	public override void ProcessReceivedCommand(CommandBase command, ICallContextInfo callContextInfo)
	{
		if (!IsConfigurationCompatible && command != null && !SupportedCommands.Contains(command.GetType()))
		{
			throw new NotSupportedByLegacyRecorderException("The operation is not supported for legacy recording servers");
		}
		base.ProcessReceivedCommand(command, callContextInfo);
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
