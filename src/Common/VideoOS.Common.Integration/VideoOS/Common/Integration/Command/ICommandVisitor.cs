namespace VideoOS.Common.Integration.Command;

public interface ICommandVisitor
{
	void Visit(EventSubscriptionStateChangedCommand command);

	void Visit(UpdateHardwareReadOnlySettingsCommand command);

	void Visit(UpdateRecorderStatusCommand command);

	void Visit(SendNotificationCommand command);

	void Visit(RecorderStatusCommand command);

	void Visit(SynchronizeDevicePresetsCommand command);

	void Visit(UpdateUserSecurityTokensCommand command);

	void Visit(SetMatrixViewCommand command);

	void Visit(SetUserSecurityTokenCommand command);

	void Visit(HardwareSetupInformationCommand command);

	void Visit(TriggerEventAsGlobalCommand command);

	void Visit(ReplaceHardwareCommand command);

	void Visit(ReportMediaStatusCommand command);

	void Visit(ReportMediaCommittedToDatabaseCommand command);

	void Visit(ReportManualRecordingStateCommand command);

	void Visit(ActivateArchivingCommand command);

	void Visit(AddBookmarkCommand command);

	void Visit(AuditLogCommand command);

	void Visit(UpdateHardwareDriverCommand command);

	void Visit(UpdateConfigurationManagerCommand command);

	void Visit(UpdateSecurityConfigurationCommand command);

	void Visit(UpdateSystemConfigurationCommand command);

	void Visit(ReplaceHardwareUpdateSystemConfigurationCommand command);

	void Visit(SetRecorderShutdownStateCommand command);

	void Visit(HardwareScanCommand command);

	void Visit(GetAbsolutePtzPositionCommand command);

	void Visit(PtzReserveSessionCommand command);

	void Visit(PtzReleaseSessionCommand command);

	void Visit(PtzStopSessionCommand command);

	void Visit(ServerFileSystemOperationCommand command);

	void Visit(DatabaseOperationCommand command);

	void Visit(SendMultiNotificationCommand command);

	void Visit(BackupConfigCommand command);

	void Visit(ConfigureRulesCommand command);

	void Visit(DeleteConfigurationItemCommand command);

	void Visit(ActivateUserDefinedEventOnRemoteSiteCommand command);

	void Visit(RecorderAttachCommand command);

	void Visit(VideoWallCommand command);

	void Visit(ExecuteRuleActionsCommand command);

	void Visit(TriggerCustomEventCommand command);

	void Visit(TriggerCustomEventBulkCommand command);

	void Visit(UpdateSecurityManagerCommand command);

	void Visit(EdgeJobUpdatedCommand command);

	void Visit(UpdateRecorderTaskProgressCommand command);

	void Visit(RecorderTaskSuccessCommand command);

	void Visit(RecorderTaskStoppedCommand command);

	void Visit(RecorderTaskFailedCommand command);

	void Visit(RemoveRecorderTaskCommand command);

	void Visit(MIPActionExecuteCommand command);

	void Visit(MIPAction2ExecuteCommand command);

	void Visit(PrepareMoveHardwareCommand command);

	void Visit(ChangeHardwarePasswordCommand command);

	void Visit(SetHardwareNetworkSettingsCommand command);

	void Visit(GetHardwareNetworkSettingsCommand command);

	void Visit(GetHardwareUsersCommand command);

	void Visit(DeleteHardwareUserCommand command);

	void Visit(PingDeviceCommand command);

	void Visit(AddHardwareUserCommand command);

	void Visit(QueryDeviceEvidenceLockSizesCommand command);

	void Visit(EvidenceLockOnOldRecorderCommand command);

	void Visit(EvidenceLockOnOldRecorderCommand2 command);

	void Visit(SetupInitialHardwareUserCommand command);

	void Visit(FirmwareUpgradeCommand command);

	void Visit(FileUploadCommand command);

	void Visit(FileUploadToRecorderCommand command);

	void Visit(UploadFileChunkCommand command);

	void Visit(CheckFilePresentOnRecorderCommand command);

	void Visit(CheckExternalRecorderRegistrationCommand command);

	void Visit(CheckMsFailoverStatusCommand command);

	void Visit(AnalyzeUpdateSecurityManagerCommand command);
}
