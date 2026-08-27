namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend;

public static class RuleConstants
{
	public const string ActionFunctionNameRecording = "VideoOS.DeviceRecording";

	public const string ActionFunctionNamePtzMoveToDefault = "VideoOS.GotoDefaultPreset";

	public const string ActionFunctionNameSetDeviceOutput = "VideoOS.SetDeviceOutput";

	public const string ActionFunctionNamePtzMoveToPreset = "VideoOS.GotoPreset";

	public const string ActionFunctionNameFramerate = "VideoOS.DeviceFramerate";

	public const string ActionFunctionNameRecordingFramerate = "VideoOS.DeviceRecordingFramerate";

	public const string ActionFunctionNameFeed = "VideoOS.DeviceStartStopFeed";

	public const string ActionFunctionNameRetrieveEdgeStorage = "VideoOS.RetrieveEdgeStorageRecording";

	public const string ActionFunctionNameRetrieveEdgeStorageRecordingTimeOrMetadata = "VideoOS.RetrieveEdgeStorageRecordingTimeOrMetadata";

	public const string ActionFunctionNameRecordAllFrames = "VideoOS.RecordAllFrames";

	public const string ActionFunctionNameSetBookmark = "VideoOS.SetBookmark";

	public const string ActionFunctionNameSetMonitorCameras = "VideoOS.SetMonitorCameras";

	public const string ActionFunctionNameSendSNMPTrap = "VideoOS.CreateSNMPTrap";

	public const string ActionFunctionNameSetMonitorText = "VideoOS.SetMonitorText";

	public const string ActionFunctionNameCreateLogEntry = "VideoOS.CreateLogEntry";

	public const string ActionFunctionNameSetVideoWallPreset = "VideoOS.SetVideoWallPreset";

	public const string ActionFunctionNameActivateArchiving = "VideoOS.ActivateArchiving";

	public const string ActionFunctionNameRemoveMonitorCameras = "VideoOS.RemoveMonitorCameras";

	public const string ActionFunctionNameStartPluginOnDevices = "VideoOS.DeviceStartPlugin";

	public const string ActionFunctionNameStopPluginOnDevices = "VideoOS.DeviceStopPlugin";

	public const string ActionFunctionNameMIPAction = "VideoOS.MIPAction";

	public const string ActionFunctionNameMIPAction2 = "VideoOS.MIPAction2";

	public const string ActionFunctionNameDeviceChangeSettings = "VideoOS.DeviceChangeSettings";

	public const string ActionFunctionNameTriggerEventOnRemoteSite = "VideoOS.TriggerUserDefinedEventOnRemoteSite";

	public const string ActionFunctionNameStartStopPatrolling = "VideoOS.DeviceStartStopPatrolling";

	public const string ActionFunctionNamePauseResumePatrolling = "VideoOS.DevicePauseResumePatrolling";

	public const string ActionFunctionNameSetMatrixView = "VideoOS.SetMatrixView";

	public const string ConditionNameWithinTimeProfile = "VideoOS.WithinTimeProfile";

	public const string ConditionNameOutsideTimeProfile = "VideoOS.NotWithinTimeProfile";

	public const string ConditionNameTimeBetween = "VideoOS.TimeOfDayBetween";

	public const string ConditionNameDayOfWeek = "VideoOS.WeekdayIsOneOf";

	public const string ConditionNameAlways = "VideoOS.Always";

	public const string ConditionNameWithinWindow = "VideoOS.MotionEventIsFromWindow";

	public const string ConditionNameFailoverActive = "VideoOS.IsFailover";

	public const string ConditionNameFailoverInactive = "VideoOS.IsNotFailover";

	public const string ConditionFunctionNameGetMetadata = "VideoOS.GetMetaData";

	public const string ParameterNameDelay = "Delay";

	public const string ParameterNameDevices = "DeviceIds";

	public const string ParameterNameDeviceGroups = "DeviceGroupIds";

	public const string ParameterNamePriority = "Priority";

	public const string ParameterNameWithinTimeProfile = "WithinTimeProfile";

	public const string ParameterNameOutsideTimeProfile = "OutsideTimeProfile";

	public const string ParameterNameTimeBetweenStartTime = "StartTime";

	public const string ParameterNameTimeBetweenEndTime = "EndTime";

	public const string ParameterNameDaysOfWeek = "DayOfWeekIsOneOf";

	public const string ParameterNameWithinWindow = "EventIsWithinWindow";

	public const string ParameterNameState = "State";

	public const string ParameterNamePreset = "PresetId";

	public const string ParameterNameDevice = "DeviceId";

	public const string ParameterNameMonthlyAbsoluteDay = "AbsoluteDay";

	public const string ParameterNameMonthlyRelativeOffset = "Offset";

	public const string ParameterNameMonthlyRelativeDay = "RelativeDay";

	public const string ParameterNameFramerate = "Framerate";

	public const string ParameterNameRetrieveSecondsBefore = "RetrieveSecondsBefore";

	public const string ParameterNameHeader = "Header";

	public const string ParameterNameText = "Text";

	public const string ParameterNameDescription = "Description";

	public const string ParameterNameTimeBefore = "TimeBefore";

	public const string ParameterNameTimeAfter = "TimeAfter";

	public const string ParameterNameVideoWallId = "VideoWallId";

	public const string ParameterNameMonitorId = "MonitorId";

	public const string ParameterNameLayoutId = "LayoutId";

	public const string ParameterNameSiteDeviceIds = "SiteDeviceIds";

	public const string ParameterNamePosition = "Position";

	public const string ParameterNameArchiveIds = "ArchiveIds";

	public const string ParameterNamePluginIds = "PluginIds";

	public const string ParameterNameMipId = "MipId";

	public const string ParameterNameMipName = "MipName";

	public const string ParameterNameItems = "Items";

	public const string ParameterNameItemGroups = "ItemGroups";

	public const string ParameterNameItems2 = "Items2";

	public const string ParameterNameItemGroups2 = "ItemGroups2";

	public const string ParameterNameSettings = "Settings";

	public const string ParameterNameSiteId = "SiteId";

	public const string ParameterNameUserDefinedEvent = "UserDefinedEvent";

	public const string ParameterNamePatrollingProfile = "PatrollingSchemaId";

	public const string ParameterNameTimeRange = "TimeRange";

	public const string ParameterNameMatrixId = "MatrixId";

	public const int DelayParameterMinValue = -2147482800;

	public const int DelayParameterMaxValue = 2147482800;

	public const int PtzPriorityParameterMinValue = 1;

	public const int PtzPriorityParameterMaxValue = 32000;

	public const double FramerateParameterMinValue = 0.01;

	public const double FramerateParameterMaxValue = 1000.0;

	public const int RetrieveSecondsBeforeMinValue = -2592000;

	public const int RetrieveSecondsBeforeMaxValue = 0;

	public const int PositionParameterMinValue = 0;

	public const int PositionParameterMaxValue = 998;

	public const string UnknownItemType = "Unknown";

	public const string DeviceFilterPtz = "Ptz";

	public const string DeviceFilterEdgeStorage = "EdgeStorage";
}
