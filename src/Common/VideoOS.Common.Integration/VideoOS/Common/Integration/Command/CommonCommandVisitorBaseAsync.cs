using System;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public abstract class CommonCommandVisitorBaseAsync : ICommonCommandVisitorAsync
{
	private Func<IVisitableCommandAsync, Task> _onCommandNotSupported = async delegate
	{
		await Task.CompletedTask;
	};

	protected CommonCommandVisitorBaseAsync()
	{
	}

	protected CommonCommandVisitorBaseAsync(Func<IVisitableCommandAsync, Task> onCommandNotSupported)
		: this()
	{
		if (onCommandNotSupported == null)
		{
			throw new ArgumentNullException("onCommandNotSupported");
		}
		_onCommandNotSupported = onCommandNotSupported;
	}

	public virtual async Task Visit(EventSubscriptionStateChangedCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(UpdateHardwareReadOnlySettingsCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(DeleteConfigurationItemCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(UpdateRecorderStatusCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(SendNotificationCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(RecorderStatusCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(SynchronizeDevicePresetsCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(UpdateUserSecurityTokensCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(SetMatrixViewCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(SetUserSecurityTokenCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(HardwareSetupInformationCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(TriggerEventAsGlobalCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(TriggerCustomEventBulkCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(ReplaceHardwareCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(ReplaceHardwareUpdateSystemConfigurationCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(ReportMediaStatusCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(ReportMediaCommittedToDatabaseCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(ReportManualRecordingStateCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(ActivateArchivingCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(AddBookmarkCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(AuditLogCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(UpdateHardwareDriverCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(UpdateConfigurationManagerCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(UpdateSecurityConfigurationCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(UpdateSystemConfigurationCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(SetRecorderShutdownStateCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(HardwareScanCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(GetAbsolutePtzPositionCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(PtzReserveSessionCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(PtzReleaseSessionCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(PtzStopSessionCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(ServerFileSystemOperationCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(DatabaseOperationCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(SendMultiNotificationCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(BackupConfigCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(ConfigureRulesCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(ActivateUserDefinedEventOnRemoteSiteCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(RecorderAttachCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(VideoWallCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(ExecuteRuleActionsCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(TriggerCustomEventCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(UpdateSecurityManagerCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(EdgeJobUpdatedCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(UpdateRecorderTaskProgressCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(RecorderTaskSuccessCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(RecorderTaskStoppedCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(RecorderTaskFailedCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(RemoveRecorderTaskCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(MIPActionExecuteCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(MIPAction2ExecuteCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(PrepareMoveHardwareCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(ChangeHardwarePasswordCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(SetHardwareNetworkSettingsCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(GetHardwareNetworkSettingsCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(GetHardwareUsersCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(PingDeviceCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(AddHardwareUserCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(DeleteHardwareUserCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(SetupInitialHardwareUserCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(QueryDeviceEvidenceLockSizesCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(EvidenceLockOnOldRecorderCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(EvidenceLockOnOldRecorderCommand2 command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(FirmwareUpgradeCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(FileUploadCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(FileUploadToRecorderCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(UploadFileChunkCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(CheckFilePresentOnRecorderCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(CheckExternalRecorderRegistrationCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(CheckMsFailoverStatusCommand command)
	{
		await OnCommandNotSupported(command);
	}

	public virtual async Task Visit(AnalyzeUpdateSecurityManagerCommand command)
	{
		await OnCommandNotSupported(command);
	}

	protected virtual async Task OnCommandNotSupported(IVisitableCommandAsync command)
	{
		await _onCommandNotSupported(command);
	}
}
