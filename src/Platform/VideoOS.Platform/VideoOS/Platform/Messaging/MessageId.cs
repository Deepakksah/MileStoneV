using System;

namespace VideoOS.Platform.Messaging;

public class MessageId
{
	public class System
	{
		public static string ApplicationLoggedOnIndication = "System.ApplicationLoggedOnIndication";

		public static string ApplicationLoggedOffIndication = "System.ApplicationLoggedOffIndication";

		public static string ApplicationExitIndication = "System.ApplicationExitIndication";

		public static string ModeChangedIndication = "System.ModeChangedIndication";

		public static string ModeChangeCommand = "System.ModeChangeCommand";

		public static string SystemConfigurationChangedIndication = "System.SystemConfigurationChangedIndication";

		public static string SystemConfigurationChangedDetailsIndication = "System.SystemConfigurationChangedDetailsIndication";

		public static string LocalConfigurationChangedIndication = "System.LocalConfigurationChangedIndication";

		public static string LoginTokenChangedIndication = "System.LoginTokenChangedIndication";

		public static string ApplicationRefreshTreeViewCommand = "System.ApplicationRefreshTreeViewCommand";

		public static string LicenseChangedIndication = "System.LicenseChangedIndication";

		public static string LicenseAutomaticUpdateRequest = "System.LicenseAutomaticUpdateRequest";

		[Obsolete("Don't listen on this message. Instead use the UserContext.PrivacyMaskLiftChanged event.")]
		public static string RemovableMasksLiftedChangedIndication = "System.RemovableMasksLiftedChangedIndication";
	}

	public class SmartClient
	{
		public const string SelectedCameraChangedIndication = "SmartClient.SelectedCameraChangedIndication";

		public const string GetSelectedCameraRequest = "SmartClient.GetSelectedCameraRequest";

		public const string PlaybackCommand = "SmartClient.PlaybackCommand";

		public const string PlaybackSkipModeCommand = "SmartClient.PlaybackSkipModeCommand";

		public const string GetCurrentPlaybackTimeRequest = "SmartClient.GetCurrentPlaybackTimeRequest";

		public const string PlaybackIndication = "SmartClient.PlaybackIndication";

		public const string PlaybackCurrentTimeIndication = "SmartClient.PlaybackCurrentTimeIndication";

		public const string PlaybackTimeInformation = "SmartClient.PlaybackDeviceInformation";

		public const string ClearIndicatorCommand = "SmartClient.ClearIndicatorCommand";

		public const string ApplicationControlCommand = "SmartClient.ApplicationControlCommand";

		public const string ChangeModeCommand = "SmartClient.ChangeModeCommand";

		public const string ShowWorkSpaceCommand = "SmartClient.ShowWorkSpaceCommand";

		public const string ChangeWorkSpaceStateCommand = "SmartClient.ChangeWorkSpaceStateCommand";

		public const string ChangeThemeCommand = "SmartClient.ChangeThemeCommand";

		public const string ViewItemControlCommand = "SmartClient.ViewItemControlCommand";

		public const string MultiWindowCommand = "SmartClient.MultiWindowCommand";

		public const string ViewGroupModifiedIndication = "SmartClient.ViewGroupModifiedIndication";

		public const string SelectedViewChangedIndication = "SmartClient.SelectedViewChangedIndication";

		public const string ViewGroupsLoadedIndication = "SmartClient.ViewGroupsLoadedIndication";

		public const string WindowStateChangedIndication = "SmartClient.WindowStateChangedIndication";

		public static string ShownWorkSpaceChangedIndication = "SmartClient.ShownWorkSpaceChangedIndication";

		public static string WorkSpaceStateChangedIndication = "SmartClient.WorkSpaceStateChangedIndication";

		public static string ThemeChangedIndication = "SmartClient.ThemeChangedIndication";

		public const string SetCameraInViewCommand = "SmartClient.SetCameraInViewCommand";

		public const string SmartClientMessageCommand = "SmartClient.SmartClientMessageCommand";

		public const string SmartClientMessageButtonClickedIndication = "SmartClient.SmartClientMessageButtonClickedIndication";

		public const string SmartClientMessageRemovedIndication = "SmartClient.SmartClientMessageRemovedIndication";

		public const string SmartClientTimeZoneChangedIndication = "SmartClient.SmartClientTimeZoneChangedIndication";

		public const string SetSelectedViewItemCommand = "SmartClient.SetSelectedViewItemCommand";

		public const string SelectedViewItemChangedIndication = "SmartClient.SelectedViewItemChangedIndication";

		public const string GetSelectedViewItemRequest = "SmartClient.GetSelectedViewItemRequest";

		public const string TalkToSpeakerCommand = "SmartClient.TalkToSpeakerCommand";

		public const string SelectSpeakerCommand = "SmartClient.SelectSpeakerCommand";

		public const string SelectMicrophoneCommand = "SmartClient.SelectMicrophoneCommand";

		public const string ReloadConfigurationCommand = "SmartClient.ReloadConfigurationCommand";

		public const string ShowCamerasInFloatingWindowCommand = "SmartClient.ShowCamerasInFloatingWindowCommand";

		public const string WorkSpaceReadyIndication = "SmartClient.WorkSpaceReadyIndication";

		public const string GetTimelineSelectedIntervalRequest = "SmartClient.GetTimelineSelectedIntervalRequest";

		public const string PtzCommandOccurredIndication = "SmartClient.PtzCommandOccurredIndication";

		public const string RefreshTimelineRequest = "SmartClient.RefreshTimelineRequest";

		public const string SmartMapGoToPositionCommand = "SmartClient.SmartMapGoToPositionCommand";

		public const string SmartMapGoToAreaCommand = "SmartClient.SmartMapGoToAreaCommand";

		public const string SmartMapGoToLocationCommand = "SmartClient.SmartMapGoToLocationCommand";

		public const string SmartMapSelectItemCommand = "SmartClient.SmartMapSelectItemCommand";

		public const string SmartMapGetPositionRequest = "SmartClient.SmartMapGetPositionRequest";

		public const string SmartMapPositionChangedIndication = "SmartClient.SmartMapPositionChangedIndication";

		public const string SmartMapNavigateToCamera = "SmartClient.SmartMapNavigateToCamera";

		public const string SmartMapNavigateToDevice = "SmartClient.SmartMapNavigateToDevice";

		public const string GetCurrentWorkspaceRequest = "SmartClient.GetCurrentWorkspaceRequest";

		public const string ToastShowCommand = "SmartClient.ToastShowCommand";

		public const string ToastHideCommand = "SmartClient.ToastHideCommand";

		public const string SelectAlarmCommand = "SmartClient.SelectAlarmCommand";

		public const string AddToExportCommand = "SmartClient.AddToExportCommand";

		public const string RestrictedMediaIsCreateRestrictionAllowedRequest = "SmartClient.RestrictedMediaIsCreateRestrictionAllowedRequest";

		public const string RestrictedMediaCreateRestrictionCommand = "SmartClient.RestrictedMediaCreateRestrictionCommand";

		public const string ShowSettingsDialogCommand = "SmartClient.ShowSettingsDialogCommand";

		public const string IsDirectInputHIDHandledByPlugInRequest = "SmartClient.HID.IsDirectInputHIDHandledByPluginRequest";
	}

	public class Server
	{
		public const string NewEventCommand = "Server.NewEventCommand";

		public const string NewEventsCommand = "Server.NewEventsCommand";

		public const string NewAlarmCommand = "Server.NewAlarmCommand";

		public const string NewEventIndication = "Server.NewEventIndication";

		public const string NewEventsIndication = "Server.NewEventsIndication";

		public const string NewAlarmIndication = "Server.NewAlarmIndication";

		public const string ConfigurationChangedIndication = "Server.ConfigurationChangedIndication";

		public const string RecorderFQIDChangedIndication = "Server.RecorderFQIDChangedIndication";

		public const string ChangedAlarmIndication = "Server.ChangedAlarmIndication";

		public const string ChangedAlarmHistoryIndication = "Server.ChangedAlarmHistoryIndication";

		public const string AlarmNotificationsDisabledIndication = "Server.AlarmNotificationsDisabledIndication";

		public const string AlarmNotificationsEnabledIndication = "Server.AlarmNotificationsEnabledIndication";

		[Obsolete("Do not use this message. Use NewAlarmCommand instead")]
		public const string NewEventAsAlarmHandlerCommand = "Server.NewEventAsAlarmHandlerCommand";

		[Obsolete("Do not use this message. Use NewEventCommand instead")]
		public const string NewAlertAsAlarmHandlerCommand = "Server.NewAlertAsAlarmHandlerCommand";

		[Obsolete("Do not use this message. Use NewEventCommand instead")]
		public const string NewAlarmAsEventCommand = "Server.NewAlarmAsEventCommand";

		[Obsolete("Do not use this message. Use NewEventCommand instead")]
		public const string EventCommand = "Server.EventCommand";

		public const string GetIPAddressRequest = "Server.GetIPAddresRequest";

		public const string GetIPAddressResponse = "Server.GetIPAddresResponse";

		[Obsolete("Obsolete. Do not use.")]
		public const string NewAlarmUpdatedIndication = "Server.NewAlarmUpdatedIndication";

		public const string DisabledMessage = "Disabled";

		public const string EnabledMessage = "Enabled";

		public const string NotRespondingMessage = "Not Responding";

		public const string RespondingMessage = "Responding";

		public const string MotionDetectedMessage = "Motion Detected";

		public const string MotionEndMessage = "Motion End";

		public const string ExternalEventMessage = "External Event";

		public const string OutputActivatedMessage = "Output Activated";

		public const string OutputDeactivatedMessage = "Output Deactivated";

		public const string RunningOutOfDiskSpaceMessage = "Running Out of Disk Space";

		public const string ArchiveFailureMessage = "Archive Failure";

		public const string ArchiveMoveFailureMessage = "Archive Move Failure";

		public const string ServerNotRespondingMessage = "Server Not Responding";

		public const string ServerRespondingMessage = "Server Responding";

		public const string DatabaseBeingRepairedMessage = "Database being repaired";

		public const string ArchiveFailureDiskUnavailableMessage = "Archive Failure: Disk Unavailable";

		public const string ArchiveDiskAvailableMessage = "Archive Disk Available";

		public const string SettingsChangedMessage = "Settings Changed";

		public const string SettingsChangedErrorMessage = "Settings Changed Error";

		public const string GetMapRequest = "Server.GetMapRequest";

		public const string GetMapResponse = "Server.GetMapResponse";

		public const string GetAnalyticsMessagesRequest = "Analytics.GetAnalyticsMessagesRequest";

		public const string GetAnalyticsMessagesResponse = "Analytics.GetAnalyticsMessagesResponse";

		public const string MipItemPositionChangedIndication = "Server.MipItemPositionChangedIndication";
	}

	public class LPR
	{
		public const string AddLicensePlateListIndication = "LPR.LicensePlateListAddedIndication";

		public const string RenameLicensePlateListIndication = "LPR.LicensePlateListRenamedIndication";

		public const string DeleteLicensePlateListIndication = "LPR.LicensePlateListDeletedIndication";

		public const string AddLicensePlatesIndication = "LPR.LicensePlatesAddedIndication";

		public const string DeleteLicensePlatesIndication = "LPR.LicensePlatesDeletedIndication";

		public const string ClearLicensePlatesIndication = "LPR.LicensePlatesClearedIndication";
	}

	public class Control
	{
		public static string TriggerCommand = "Control.TriggerCommand";

		public static string StartRecordingCommand = "Control.StartRecordingCommand";

		public static string StopRecordingCommand = "Control.StopRecordingCommand";

		public const string PTZMoveCommand = "Control.PTZMoveCommand";

		public const string PTZCenterCommand = "Control.PTZCenterCommand";

		public const string PTZRectangleCommand = "Control.PTZRectangleCommand";

		public const string PTZAUXCommand = "Control.PTZAUXCommand";

		public const string PTZMoveAbsoluteCommand = "Control.PTZMoveAbsoluteCommand";

		public const string PTZMoveStartCommand = "Control.PTZMoveStartCommand";

		public const string PTZMoveStopCommand = "Control.PTZMoveStopCommand";

		public const string PTZGetAbsoluteRequest = "Control.PTZGetAbsoluteRequest";

		public const string LensCommand = "Control.LensCommand";

		public const string OutputActivate = "Control.OutputActivate";

		public const string OutputDeactivate = "Control.OutputDeactivate";

		public const string RecorderErrorIndication = "Control.RecorderErrorIndication";

		public const string VideoWallPresetActivateCommand = "Control.VideoWallPresetActivateCommand";

		public const string VideoWallShowTextCommand = "Control.VideoWallShowTextCommand";

		public const string VideoWallSetLayoutCommand = "Control.VideoWallSetLayoutCommand";

		public const string VideoWallSetLayoutAndCamerasCommand = "Control.VideoWallSetLayoutAndCamerasCommand";

		public const string VideoWallSetCamerasCommand = "Control.VideoWallSetCamerasCommand";

		public const string VideoWallRemoveCamerasCommand = "Control.VideoWallRemoveCamerasCommand";

		public const string VideoWallApplyXmlCommand = "Control.VideoWallApplyXmlCommand";

		public const string VideoWallIndication = "Control.VideoWallIndication";

		public const string DriverCommand = "Control.DriverCommand";

		public const string DriverResponse = "Control.DriverResponse";
	}
}
