using System;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Security;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RecorderCommunication;

public interface IRecorderSend
{
	Guid RecorderAttachAsync(ICallContextInfo callContextInfo);

	Guid TriggerCustomEventAsync(TriggerEvent triggerEvent, ICallContextInfo callContextInfo);

	Guid TriggerEventAsGlobalAsync(Guid recorderSourceId, TriggerEvent triggerEvent, ICallContextInfo callContextInfo);

	Guid HardwareCancelScanAsync(Guid recorderId, Guid scanSessionId, ICallContextInfo callContextInfo);

	Guid UpdateSystemConfigurationAsync(SystemConfigurationDto systemConfiguration, ICallContextInfo callContextInfo);

	Guid ReplaceHardwareUpdateSystemConfigurationAsync(SystemConfigurationDto systemConfiguration, ICallContextInfo callContextInfo);

	Guid UpdateSecurityConfigurationAsync(SecurityConfigurationDto securityConfigurationDto, ICallContextInfo callContextInfo);

	Guid ExecuteRuleActionsAsync(Guid recorderId, RuleActionExecutionRequest[] ruleActionRequests, DateTime senderTimeUtc, ICallContextInfo callContextInfo);

	Guid UpdateUserSecurityTokensAsync(SecurityToken[] securityTokens, ICallContextInfo callContextInfo);

	void ProcessPendingTasks();

	void SendUpdateSystemConfigurationCommand(UpdateSystemConfigurationCommand command, ICallContextInfo callContextInfo);

	void SendHardwareSetupInformationCommand(HardwareSetupInformationCommand command, ICallContextInfo callContextInfo);

	void SendHardwareScanCommand(HardwareScanCommand command, ICallContextInfo callContextInfo);

	void SendGetAbsolutePtzPositionCommand(GetAbsolutePtzPositionCommand command, ICallContextInfo callContextInfo);

	void SendGetPtzReserveSessionCommand(PtzReserveSessionCommand command, ICallContextInfo callContextInfo);

	void SendPtzReleaseSessionCommand(PtzReleaseSessionCommand command, ICallContextInfo callContextInfo);

	void SendPtzStopSessionCommand(PtzStopSessionCommand command, ICallContextInfo callContextInfo);

	void SendSynchronizeDevicePresetsCommand(SynchronizeDevicePresetsCommand command, ICallContextInfo callContextInfo);

	void SendDatabaseOperationCommand(DatabaseOperationCommand command, ICallContextInfo callContextInfo);

	void SendServerFileSystemOperationCommand(ServerFileSystemOperationCommand command, ICallContextInfo callContextInfo);

	void SendActivateArchivingCommand(ActivateArchivingCommand command, ICallContextInfo callContextInfo);

	void SendPrepareMoveHardwareCommand(PrepareMoveHardwareCommand command, ICallContextInfo callContextInfo);

	void SendChangeHardwarePasswordCommand(ChangeHardwarePasswordCommand command, ICallContextInfo callContextInfo);

	void SendPingDeviceCommand(PingDeviceCommand command, ICallContextInfo callContextInfo);

	void SendSetHardwareNetworkSettingsCommand(SetHardwareNetworkSettingsCommand command, ICallContextInfo callContextInfo);

	void SendGetHardwareNetworkSettingsCommand(GetHardwareNetworkSettingsCommand command, ICallContextInfo callContextInfo);

	void SendGetHardwareUsersCommand(GetHardwareUsersCommand command, ICallContextInfo callContextInfo);

	void SendAddHardwareUserCommand(AddHardwareUserCommand command, ICallContextInfo callContextInfo);

	void SendQueryDeviceEvidenceLockSizesCommand(QueryDeviceEvidenceLockSizesCommand command, ICallContextInfo callContextInfo);

	void SendEvidenceLockOnOldRecorderCommand(EvidenceLockOnOldRecorderCommand command, ICallContextInfo callContextInfo);

	void SendEvidenceLockOnOldRecorderCommand2(EvidenceLockOnOldRecorderCommand2 command, ICallContextInfo callContextInfo);

	void SendDeleteHardwareUserCommand(DeleteHardwareUserCommand command, ICallContextInfo callContextInfo);

	void SendSetupInitialHardwareUserCommand(SetupInitialHardwareUserCommand command, ICallContextInfo callContextInfo);

	void SendFirmwareUpgradeCommand(FirmwareUpgradeCommand command, ICallContextInfo callContextInfo);

	void SendFileUploadCommand(FileUploadCommand command, ICallContextInfo callContextInfo);

	void SendFilePresentOnRecordingServerCommand(CheckFilePresentOnRecorderCommand command, ICallContextInfo callContextInfo);

	void SendCheckExternalRecorderRegistrationCommand(CheckExternalRecorderRegistrationCommand command, ICallContextInfo callContextInfo);

	void SendResponse(ResponseBase response);
}
