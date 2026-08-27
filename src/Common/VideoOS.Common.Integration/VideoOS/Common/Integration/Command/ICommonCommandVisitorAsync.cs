using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public interface ICommonCommandVisitorAsync
{
	Task Visit(EventSubscriptionStateChangedCommand command);

	Task Visit(UpdateHardwareReadOnlySettingsCommand command);

	Task Visit(UpdateRecorderStatusCommand command);

	Task Visit(SendNotificationCommand command);

	Task Visit(RecorderStatusCommand command);

	Task Visit(SynchronizeDevicePresetsCommand command);

	Task Visit(UpdateUserSecurityTokensCommand command);

	Task Visit(SetMatrixViewCommand command);

	Task Visit(SetUserSecurityTokenCommand command);

	Task Visit(HardwareSetupInformationCommand command);

	Task Visit(TriggerEventAsGlobalCommand command);

	Task Visit(ReplaceHardwareCommand command);

	Task Visit(ReportMediaStatusCommand command);

	Task Visit(ReportMediaCommittedToDatabaseCommand command);

	Task Visit(ReportManualRecordingStateCommand command);

	Task Visit(ActivateArchivingCommand command);

	Task Visit(AddBookmarkCommand command);

	Task Visit(AuditLogCommand command);

	Task Visit(UpdateHardwareDriverCommand command);

	Task Visit(UpdateConfigurationManagerCommand command);

	Task Visit(UpdateSecurityConfigurationCommand command);

	Task Visit(UpdateSystemConfigurationCommand command);

	Task Visit(ReplaceHardwareUpdateSystemConfigurationCommand command);

	Task Visit(SetRecorderShutdownStateCommand command);

	Task Visit(HardwareScanCommand command);

	Task Visit(GetAbsolutePtzPositionCommand command);

	Task Visit(PtzReserveSessionCommand command);

	Task Visit(PtzReleaseSessionCommand command);

	Task Visit(PtzStopSessionCommand command);

	Task Visit(ServerFileSystemOperationCommand command);

	Task Visit(ChangeHardwarePasswordCommand command);

	Task Visit(DatabaseOperationCommand command);

	Task Visit(SendMultiNotificationCommand command);

	Task Visit(BackupConfigCommand command);

	Task Visit(ConfigureRulesCommand command);

	Task Visit(DeleteConfigurationItemCommand command);

	Task Visit(ActivateUserDefinedEventOnRemoteSiteCommand command);

	Task Visit(RecorderAttachCommand command);

	Task Visit(VideoWallCommand command);

	Task Visit(ExecuteRuleActionsCommand command);

	Task Visit(TriggerCustomEventCommand command);

	Task Visit(TriggerCustomEventBulkCommand command);

	Task Visit(UpdateSecurityManagerCommand command);

	Task Visit(EdgeJobUpdatedCommand command);

	Task Visit(UpdateRecorderTaskProgressCommand command);

	Task Visit(RecorderTaskSuccessCommand command);

	Task Visit(RecorderTaskStoppedCommand command);

	Task Visit(RecorderTaskFailedCommand command);

	Task Visit(RemoveRecorderTaskCommand command);

	Task Visit(MIPActionExecuteCommand command);

	Task Visit(MIPAction2ExecuteCommand command);

	Task Visit(PrepareMoveHardwareCommand command);

	Task Visit(SetHardwareNetworkSettingsCommand command);

	Task Visit(GetHardwareNetworkSettingsCommand command);

	Task Visit(GetHardwareUsersCommand command);

	Task Visit(PingDeviceCommand command);

	Task Visit(AddHardwareUserCommand command);

	Task Visit(DeleteHardwareUserCommand command);

	Task Visit(QueryDeviceEvidenceLockSizesCommand command);

	Task Visit(EvidenceLockOnOldRecorderCommand command);

	Task Visit(EvidenceLockOnOldRecorderCommand2 command);

	Task Visit(SetupInitialHardwareUserCommand command);

	Task Visit(FirmwareUpgradeCommand command);

	Task Visit(FileUploadCommand command);

	Task Visit(FileUploadToRecorderCommand command);

	Task Visit(UploadFileChunkCommand command);

	Task Visit(CheckFilePresentOnRecorderCommand command);

	Task Visit(CheckExternalRecorderRegistrationCommand command);

	Task Visit(CheckMsFailoverStatusCommand command);

	Task Visit(AnalyzeUpdateSecurityManagerCommand command);
}
