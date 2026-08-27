using System;

namespace VideoOS.Common.Integration.Command;

public abstract class CommonCommandVisitorBase : ICommandVisitor
{
	private Action<IVisitableCommand> _onCommandNotSupported = delegate
	{
	};

	protected CommonCommandVisitorBase()
	{
	}

	protected CommonCommandVisitorBase(Action<IVisitableCommand> onCommandNotSupported)
		: this()
	{
		if (onCommandNotSupported == null)
		{
			throw new ArgumentNullException("onCommandNotSupported");
		}
		_onCommandNotSupported = onCommandNotSupported;
	}

	public virtual void Visit(EventSubscriptionStateChangedCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(UpdateHardwareReadOnlySettingsCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(DeleteConfigurationItemCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(UpdateRecorderStatusCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(SendNotificationCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(RecorderStatusCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(SynchronizeDevicePresetsCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(UpdateUserSecurityTokensCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(SetMatrixViewCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(SetUserSecurityTokenCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(HardwareSetupInformationCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(TriggerEventAsGlobalCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(TriggerCustomEventBulkCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(ReplaceHardwareCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(ReplaceHardwareUpdateSystemConfigurationCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(ReportMediaStatusCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(ReportMediaCommittedToDatabaseCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(ReportManualRecordingStateCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(ActivateArchivingCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(AddBookmarkCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(AuditLogCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(UpdateHardwareDriverCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(UpdateConfigurationManagerCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(UpdateSecurityConfigurationCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(UpdateSystemConfigurationCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(SetRecorderShutdownStateCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(HardwareScanCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(GetAbsolutePtzPositionCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(PtzReserveSessionCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(PtzReleaseSessionCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(PtzStopSessionCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(ServerFileSystemOperationCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(DatabaseOperationCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(SendMultiNotificationCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(BackupConfigCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(ConfigureRulesCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(ActivateUserDefinedEventOnRemoteSiteCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(RecorderAttachCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(VideoWallCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(ExecuteRuleActionsCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(TriggerCustomEventCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(UpdateSecurityManagerCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(EdgeJobUpdatedCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(UpdateRecorderTaskProgressCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(RecorderTaskSuccessCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(RecorderTaskStoppedCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(RecorderTaskFailedCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(RemoveRecorderTaskCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(MIPActionExecuteCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(MIPAction2ExecuteCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(PrepareMoveHardwareCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(ChangeHardwarePasswordCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(SetHardwareNetworkSettingsCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(GetHardwareNetworkSettingsCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(GetHardwareUsersCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(PingDeviceCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(AddHardwareUserCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(DeleteHardwareUserCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(QueryDeviceEvidenceLockSizesCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(EvidenceLockOnOldRecorderCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(EvidenceLockOnOldRecorderCommand2 command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(SetupInitialHardwareUserCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(FirmwareUpgradeCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(FileUploadCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(FileUploadToRecorderCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(UploadFileChunkCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(CheckFilePresentOnRecorderCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(CheckExternalRecorderRegistrationCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(CheckMsFailoverStatusCommand command)
	{
		OnCommandNotSupported(command);
	}

	public virtual void Visit(AnalyzeUpdateSecurityManagerCommand command)
	{
		OnCommandNotSupported(command);
	}

	protected virtual void OnCommandNotSupported(IVisitableCommand command)
	{
		_onCommandNotSupported(command);
	}
}
