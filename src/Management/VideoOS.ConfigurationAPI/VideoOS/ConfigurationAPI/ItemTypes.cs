using System;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI;

[DataContract]
public static class ItemTypes
{
	public const string System = "System";

	public const string SystemAddress = "SystemAddress";

	public const string SystemAddressFolder = "SystemAddressFolder";

	public const string RecordingServer = "RecordingServer";

	public const string RecordingServerFolder = "RecordingServerFolder";

	public const string Hardware = "Hardware";

	public const string HardwareFolder = "HardwareFolder";

	public const string HardwareDriver = "HardwareDriver";

	public const string HardwareDriverFolder = "HardwareDriverFolder";

	public const string HardwareDeviceEvent = "HardwareDeviceEvent";

	public const string HardwareDeviceEventFolder = "HardwareDeviceEventFolder";

	public const string Camera = "Camera";

	public const string CameraFolder = "CameraFolder";

	public const string Speaker = "Speaker";

	public const string SpeakerFolder = "SpeakerFolder";

	public const string Microphone = "Microphone";

	public const string MicrophoneFolder = "MicrophoneFolder";

	public const string InputEvent = "InputEvent";

	public const string InputEventFolder = "InputEventFolder";

	public const string Output = "Output";

	public const string OutputFolder = "OutputFolder";

	public const string Metadata = "Metadata";

	public const string MetadataFolder = "MetadataFolder";

	public const string CameraGroup = "CameraGroup";

	public const string CameraGroupFolder = "CameraGroupFolder";

	public const string SpeakerGroup = "SpeakerGroup";

	public const string SpeakerGroupFolder = "SpeakerGroupFolder";

	public const string MicrophoneGroup = "MicrophoneGroup";

	public const string MicrophoneGroupFolder = "MicrophoneGroupFolder";

	public const string InputEventGroup = "InputEventGroup";

	public const string InputEventGroupFolder = "InputEventGroupFolder";

	public const string OutputGroup = "OutputGroup";

	public const string OutputGroupFolder = "OutputGroupFolder";

	public const string MetadataGroup = "MetadataGroup";

	public const string MetadataGroupFolder = "MetadataGroupFolder";

	public const string Storage = "Storage";

	public const string StorageFolder = "StorageFolder";

	public const string PTZ = "PTZ";

	public const string PTZFolder = "PTZFolder";

	public const string PTZSettings = "PTZSettings";

	public const string FisheyeLens = "FisheyeLens";

	public const string FisheyeLensFolder = "FisheyeLensFolder";

	public const string FPS = "FPS";

	public const string FPSFolder = "FPSFolder";

	public const string ClientSettingsFolder = "ClientSettingsFolder";

	public const string ClientSettings = "ClientSettings";

	public const string Matrix = "Matrix";

	public const string MatrixFolder = "MatrixFolder";

	public const string MotionDetection = "MotionDetection";

	public const string MotionDetectionFolder = "MotionDetectionFolder";

	public const string TransactSource = "TransactSource";

	public const string TransactSourceFolder = "TransactSourceFolder";

	[Obsolete("ItemTypes.Preset is obsolete. Use ItemTypes.PtzPreset or ItemTypes.MonitorPreset instead.")]
	public const string Preset = "Preset";

	[Obsolete("ItemTypes.PresetFolder is obsolete. Use ItemTypes.PtzPresetFolder or ItemTypes.MonitorPresetFolder instead.")]
	public const string PresetFolder = "PresetFolder";

	public const string TriggerEvent = "TriggerEvent";

	public const string TriggerEventFolder = "TriggerEventFolder";

	public const string UserDefinedEvent = "UserDefinedEvent";

	public const string UserDefinedEventFolder = "UserDefinedEventFolder";

	public const string Message = "Message";

	public const string MessageFolder = "MessageFolder";

	public const string Role = "Role";

	public const string RoleFolder = "RoleFolder";

	public const string User = "User";

	public const string UserFolder = "UserFolder";

	public const string Claim = "Claim";

	public const string ClaimFolder = "ClaimFolder";

	public const string LoginProvider = "LoginProvider";

	public const string LoginProviderFolder = "LoginProviderFolder";

	public const string RegisteredClaim = "RegisteredClaim";

	public const string RegisteredClaimFolder = "RegisteredClaimFolder";

	public const string TimeProfile = "TimeProfile";

	public const string TimeProfileFolder = "TimeProfileFolder";

	public const string TimeProfileAppointmentRoot = "TimeProfileAppointmentRoot";

	public const string TimeProfileAppointmentRecur = "TimeProfileAppointmentRecur";

	public const string TimeProfileAppointmentVariance = "TimeProfileAppointmentVariance";

	public const string TimeProfileSunclock = "TimeProfileSunclock";

	public const string Stream = "Stream";

	public const string StreamFolder = "StreamFolder";

	public const string StreamSettings = "StreamSettings";

	public const string Task = "Task";

	public const string TaskFolder = "TaskFolder";

	public const string UpdateHardwareDetail = "UpdateHardwareDetail";

	public const string DeviceDriverSettings = "DeviceDriverSettings";

	public const string DeviceDriverSettingsFolder = "DeviceDriverSettingsFolder";

	public const string HardwareDriverSettings = "HardwareDriverSettings";

	public const string HardwareDriverSettingsFolder = "HardwareDriverSettingsFolder";

	public const string GeneralSettings = "GeneralSettings";

	public const string BasicUserFolder = "BasicUserFolder";

	public const string BasicUser = "BasicUser";

	public const string PtzPresetFolder = "PtzPresetFolder";

	public const string PtzPreset = "PtzPreset";

	public const string ArchiveStorage = "ArchiveStorage";

	public const string ArchiveStorageFolder = "ArchiveStorageFolder";

	public const string StorageInformation = "StorageInformation";

	public const string ArchiveStorageInformation = "ArchiveStorageInformation";

	public const string ArchiveSchedule = "ArchiveSchedule";

	public const string PrivacyMask = "PrivacyMask";

	public const string PrivacyMaskFolder = "PrivacyMaskFolder";

	public const string PrivacyProtection = "PrivacyProtection";

	public const string PrivacyProtectionFolder = "PrivacyProtectionFolder";

	public const string Layout = "Layout";

	public const string LayoutFolder = "LayoutFolder";

	public const string LayoutGroup = "LayoutGroup";

	public const string LayoutGroupFolder = "LayoutGroupFolder";

	public const string VideoWall = "VideoWall";

	public const string VideoWallFolder = "VideoWallFolder";

	public const string Monitor = "Monitor";

	public const string MonitorFolder = "MonitorFolder";

	public const string VideoWallPreset = "VideoWallPreset";

	public const string VideoWallPresetFolder = "VideoWallPresetFolder";

	public const string MonitorPreset = "MonitorPreset";

	public const string MonitorPresetFolder = "MonitorPresetFolder";

	public const string AudioMessage = "AudioMessage";

	public const string AudioMessageFolder = "AudioMessageFolder";

	public const string AnalyticsEvent = "AnalyticsEvent";

	public const string AnalyticsEventFolder = "AnalyticsEventFolder";

	public const string GenericEvent = "GenericEvent";

	public const string GenericEventFolder = "GenericEventFolder";

	public const string GenericEventDataSource = "GenericEventDataSource";

	public const string GenericEventDataSourceFolder = "GenericEventDataSourceFolder";

	public const string GisMapLocation = "GisMapLocation";

	public const string GisMapLocationFolder = "GisMapLocationFolder";

	public const string MIPKind = "MIPKind";

	public const string MIPKindFolder = "MIPKindFolder";

	public const string MIPItem = "MIPItem";

	public const string MIPItemFolder = "MIPItemFolder";

	public const string AlarmDefinition = "AlarmDefinition";

	public const string AlarmDefinitionFolder = "AlarmDefinitionFolder";

	public const string LprMatchList = "LprMatchList";

	public const string LprMatchListFolder = "LprMatchListFolder";

	public const string LprServer = "LprServer";

	public const string LprServerFolder = "LprServerFolder";

	public const string SaveSearches = "SaveSearches";

	public const string FindSaveSearches = "FindSaveSearches";

	public const string SaveSearchesFolder = "SaveSearchesFolder";

	public const string CustomPropertiesFolder = "CustomPropertiesFolder";

	public const string CustomProperties = "CustomProperties";

	public const string CompressorFolder = "CompressorFolder";

	public const string Compressor = "Compressor";

	public const string MailNotificationProfile = "MailNotificationProfile";

	public const string MailNotificationProfileFolder = "MailNotificationProfileFolder";

	public const string InvokeInfo = "InvokeInfo";

	public const string InvokeResult = "InvokeResult";

	public const string Rule = "Rule";

	public const string RuleFolder = "RuleFolder";

	public const string AccessControlSystem = "AccessControlSystem";

	public const string AccessControlSystemFolder = "AccessControlSystemFolder";

	public const string AccessControlUnit = "AccessControlUnit";

	public const string AccessControlUnitFolder = "AccessControlUnitFolder";

	public const string AccessControlGroup = "AccessControlGroup";

	public const string AccessControlGroupFolder = "AccessControlGroupFolder";

	public const string Site = "Site";

	public const string SiteFolder = "SiteFolder";

	public const string SiteAddress = "SiteAddress";

	public const string SiteAddressFolder = "SiteAddressFolder";

	public const string LicenseInformationFolder = "LicenseInformationFolder";

	public const string LicenseInformation = "LicenseInformation";

	public const string LicenseInstalledProductFolder = "LicenseInstalledProductFolder";

	public const string LicenseInstalledProduct = "LicenseInstalledProduct";

	public const string LicenseOverviewAllFolder = "LicenseOverviewAllFolder";

	public const string LicenseOverviewAll = "LicenseOverviewAll";

	public const string LicenseDetailFolder = "LicenseDetailFolder";

	public const string LicenseDetail = "LicenseDetail";

	public const string LicenseFeature = "LicenseFeature";

	public const string BasicOwnerInformationFolder = "BasicOwnerInformationFolder";

	public const string BasicOwnerInformation = "BasicOwnerInformation";

	public const string External = "External";

	public const string ToolOptionFolder = "ToolOptionFolder";

	public const string ToolOption = "ToolOption";

	public const string ViewGroupFolder = "ViewGroupFolder";

	public const string ViewGroup = "ViewGroup";

	public const string ViewFolder = "ViewFolder";

	public const string View = "View";

	public const string ViewItem = "ViewItem";

	public const string RecordingServerMulticastFolder = "RecordingServerMulticastFolder";

	public const string RecordingServerMulticast = "RecordingServerMulticast";

	public const string HardwarePtzSettingsFolder = "HardwarePtzSettingsFolder";

	public const string HardwarePtzSettings = "HardwarePtzSettings";

	public const string HardwarePtzDeviceSetting = "HardwarePtzDeviceSetting";

	public const string PTZSessionTimeout = "PTZSessionTimeout";

	public const string PatrollingProfileFolder = "PatrollingProfileFolder";

	public const string PatrollingProfile = "PatrollingProfile";

	public const string PatrollingEntry = "PatrollingEntry";

	public const string FailoverRecorderFolder = "FailoverRecorderFolder";

	public const string FailoverRecorder = "FailoverRecorder";

	public const string FailoverGroupFolder = "FailoverGroupFolder";

	public const string FailoverGroup = "FailoverGroup";

	public const string ClientProfileFolder = "ClientProfileFolder";

	public const string ClientProfile = "ClientProfile";

	public const string EvidenceLockProfileFolder = "EvidenceLockProfileFolder";

	public const string EvidenceLockProfile = "EvidenceLockProfile";

	public const string RecordingServerFailoverFolder = "RecordingServerFailoverFolder";

	public const string RecordingServerFailover = "RecordingServerFailover";

	public const string CustomSettingGlobal = "CustomSettingGlobal";

	public const string CustomSettingUser = "CustomSettingUser";

	public const string EventTypeGroupFolder = "EventTypeGroupFolder";

	public const string EventTypeGroup = "EventTypeGroup";

	public const string EventTypeFolder = "EventTypeFolder";

	public const string EventType = "EventType";

	public const string StateGroupFolder = "StateGroupFolder";

	public const string StateGroup = "StateGroup";

	public const string InfoItemFolder = "InfoItemFolder";

	public const string InfoItem = "InfoItem";

	public const string ServerFolder = "ServerFolder";

	public const string Server = "Server";
}
