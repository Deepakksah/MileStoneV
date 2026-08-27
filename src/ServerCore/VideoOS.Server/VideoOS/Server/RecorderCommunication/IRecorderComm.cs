using System;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Security;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RecorderCommunication;

public interface IRecorderComm
{
	Guid RecorderAttachAsync(Guid serviceId, ICallContextInfo callContextInfo);

	void TriggerCustomEventAsync(Guid serviceId, TriggerEvent triggerEvent, ICallContextInfo callContextInfo, bool usedInRules);

	void TriggerCustomEventAsync(TriggerEvent triggerEvent, ICallContextInfo callContextInfo, bool usedInRules);

	void TriggerEventAsGlobalAsync(Guid recorderId, Guid sourceRecorderId, TriggerEvent triggerEvent, ICallContextInfo callContextInfo, bool usedInRules);

	Guid HardwareCancelScanAsync(Guid serviceId, Guid scanSessionId, ICallContextInfo callContextInfo);

	void UpdateSystemConfigurationAsync(Guid serviceId, SystemConfigurationDto systemConfigurationDto, ICallContextInfo callContextInfo);

	void ReplaceHardwareUpdateSystemConfigurationAsync(Guid serviceId, SystemConfigurationDto systemConfigurationDto, ICallContextInfo callContextInfo);

	void UpdateSecurityConfigurationAsync(Guid serviceId, SecurityConfigurationDto securityConfigurationDto, ICallContextInfo callContextInfo);

	void ExecuteRuleActionsAsync(Guid recorderId, RuleActionExecutionRequest[] ruleActionRequests, DateTime senderTimeUtc, ICallContextInfo callContextInfo);

	void UpdateUserSecurityTokensAsync(SecurityToken[] securityTokens, ICallContextInfo callContextInfo);

	void SendHardwareSetupInformationCommand(Guid serviceId, HardwareSetupInformationCommand command, ICallContextInfo callContextInfo);

	void SendGetAbsolutePtzPositionCommand(Guid serviceId, GetAbsolutePtzPositionCommand command, ICallContextInfo callContextInfo);

	void SendPtzReserveSessionCommand(Guid serviceId, PtzReserveSessionCommand command, ICallContextInfo callContextInfo);

	void SendPtzReleaseSessionCommand(Guid serviceId, PtzReleaseSessionCommand command, ICallContextInfo callContextInfo);

	void SendPtzStopSessionCommand(Guid serviceId, PtzStopSessionCommand command, ICallContextInfo callContextInfo);

	void SendHardwareScanCommand(Guid serviceId, HardwareScanCommand command, ICallContextInfo callContextInfo);

	void SendSynchronizeDevicePresetsCommand(Guid serviceId, SynchronizeDevicePresetsCommand command, ICallContextInfo callContextInfo);

	void SendDatabaseOperationCommand(Guid serviceId, DatabaseOperationCommand command, ICallContextInfo callContextInfo);

	void SendServerFileSystemOperationCommand(Guid serviceId, ServerFileSystemOperationCommand command, ICallContextInfo callContextInfo);

	void SendActivateArchivingCommand(Guid serviceId, ActivateArchivingCommand command, ICallContextInfo callContextInfo);

	void SendPrepareMoveHardwareCommand(Guid serviceId, PrepareMoveHardwareCommand command, ICallContextInfo callContextInfo);

	void SendUpdateSystemConfigurationCommand(Guid serviceId, UpdateSystemConfigurationCommand command, ICallContextInfo callContextInfo);

	void SendChangeHardwarePasswordCommand(Guid serviceId, ChangeHardwarePasswordCommand command, ICallContextInfo callContextInfo);

	void SendPingDeviceCommand(Guid serviceId, PingDeviceCommand command, ICallContextInfo callContextInfo);

	void SendAddHardwareUserCommand(Guid serviceId, AddHardwareUserCommand command, ICallContextInfo callContextInfo);

	void ProcessPendingTasks(Guid recorderId);

	void TryProcessPendingTasksAsync(Guid recorderId);

	void TryAllRecordersProcessPendingTasksAsync();

	void SendGetHardwareNetworkSettingsCommand(Guid serviceId, GetHardwareNetworkSettingsCommand command, ICallContextInfo callContextInfo);

	void SendSetHardwareNetworkSettingsCommand(Guid serviceId, SetHardwareNetworkSettingsCommand command, ICallContextInfo callContextInfo);

	void SendGetHardwareUsersCommand(Guid serviceId, GetHardwareUsersCommand command, ICallContextInfo callContextInfo);

	void SendDeleteHardwareUserCommand(Guid serviceId, DeleteHardwareUserCommand command, ICallContextInfo callContextInfo);

	void SendQueryDeviceEvidenceLockSizesCommand(Guid serviceId, QueryDeviceEvidenceLockSizesCommand command, ICallContextInfo callContextInfo);

	void SendEvidenceLockOnOldRecorderCommand(Guid serviceId, EvidenceLockOnOldRecorderCommand command, ICallContextInfo callContextInfo);

	void SendEvidenceLockOnOldRecorderCommand2(Guid serviceId, EvidenceLockOnOldRecorderCommand2 command, ICallContextInfo callContextInfo);

	void SendSetupInitialHardwareUserCommand(Guid serviceId, SetupInitialHardwareUserCommand command, ICallContextInfo callContextInfo);

	void SendFirmwareUpgradeCommand(Guid serviceId, FirmwareUpgradeCommand command, ICallContextInfo callContextInfo);

	void SendFileUploadCommand(Guid serviceId, FileUploadCommand command, ICallContextInfo callContextInfo);

	void SendFilePresentOnRecorderCommand(Guid serviceId, CheckFilePresentOnRecorderCommand command, ICallContextInfo callContextInfo);

	void SendCheckExternalRecorderRegistrationCommand(Guid serviceId, CheckExternalRecorderRegistrationCommand command, ICallContextInfo callContextInfo);
}
