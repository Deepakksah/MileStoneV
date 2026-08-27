using System;

namespace VideoOS.Mobile.Management;

public class Definitions
{
	public static readonly Guid AdministratorPluginId = new Guid("73f41533-5612-459a-8c80-2a8e9511cb24");

	public static readonly Guid AdministratorKind = new Guid("5ede108a-3b8f-47c6-8d81-e08fc695048e");

	public const string PasswordName = "Password";

	public static readonly Guid MobilePluginId = new Guid("32c415c6-6ddb-4931-ae35-e971590dd8b6");

	public static readonly Guid MobileKind = new Guid("9947f340-397c-483b-9e03-f27253f7f403");

	public static readonly Guid StatusPluginId = new Guid("2c101658-29e8-4dbc-a4e4-6556f0c8a329");

	public static readonly Guid StatusKind = new Guid("c19394f8-1b95-476d-b6cb-21d113f03e25");

	public const string ServerConfigurationId = "ConfigurationId";

	public const string EnabledTrueName = "True";

	public const string EnabledFalseName = "False";

	public const string TcpEnabledName = "TcpEnabled";

	public const string TcpMetaPortName = "TcpMetaPort";

	public const string TcpVideoPortName = "TcpVideoPort";

	public const string HttpEnabledName = "HttpEnabled";

	public const string HttpMetaPortName = "HttpMetaPort";

	public const string HttpMetaPathName = "HttpMetaPath";

	public const string HttpVideoPortName = "HttpVideoPort";

	public const string HttpVideoPathName = "HttpVideoPath";

	public const string LoggingEnabledName = "LoggingEnabled";

	public const string LoggingFilePathName = "LoggingFilePath";

	public const string LoggingDaysToKeep = "LoggingDaysToKeep";

	public const string LoggingSeparator = "LoggingSeparator";

	public const string LoggingWarningCpuLvl = "LoggingWarningCpuLvl";

	public const string LoggingWarningRsBand = "LoggingWarningRsBand";

	public const string LoggingWarningMuBand = "LoggingWarningMuBand";

	public const string WatchDogTimeOut = "WatchDogTimeOut";

	public const string AllCamerasView = "AllCamerasView";

	public const string OutputsAndEvents = "OutputsAndEvents";

	public const string Audio = "Audio";

	public const string IncomingAudio = "IncomingAudio";

	public const string IncomingAudioPlayback = "IncomingAudioPlayback";

	public const string SystemInfoUserName = "SystemInfoUserName";

	public const string SystemInfoPassword = "SystemInfoPassword";

	public const string ServerAddress = "ServerAddress";

	public const string ServerPort = "ServerPort";

	public const string Description = "Description";

	public const string ServerId = "ServerId";

	public const string ServerIdName = "ServerIdName";

	public const string RawStreamingEnabledName = "RawStreaming";

	public const string WebClientEnabledName = "WebClient";

	public const string TranscodedStreamingEnabledName = "TranscodeStreaming";

	public const string SuggestStreamingTypeToClientsName = "SuggestStreamingType";

	public const string MoSVersion = "MoSVersion";

	public const string DenyAdminLogin = "DenyAdminLogin";

	public const string Version = "Version";

	public const string DirectStreamingEnabled = "DirectStreamingEnabled";

	public const string AdaptiveStreamingEnabled = "AdaptiveStreamingEnabled";

	public const string AdaptiveStreamingMode = "AdaptiveStreamingMode";

	public const string Bookmarks = "Bookmarks";

	public const string MobileServerName = "MobileName";

	public const string MobileServerIp = "MobileIp";

	public const string UpdateIntervalS = "UpdateIntervalS";

	public const string VideoUploadEnbled = "VideoUploadEnbled";

	public const string VideoUploadCameraCount = "VideoUploadCameraCount";

	public const string VideoUploadUsername = "VideoUploadUsername";

	public const string VideoUploadPort = "VideoUploadPort";

	public const string VideoUploadMac = "VideoUploadMac";

	public const string VideoUploadCameraId = "VideoUploadCameraId";

	public const string VideoUploadConfigId = "VideoUploadConfigId";

	public const string VideoUploadDriverPassword = "VideoUploadDriverPassword";

	public const string LimitationsLevelCount = "LimitationsLevelCount";

	public const string LimitationsLevelCPU = "LimitationsLevelCPU";

	public const string LimitationsLevelBandwidth = "LimitationsLevelBandwidth";

	public const string LimitationsLevelFPS = "LimitationsLevelFPS";

	public const string LimitationsLevelActive = "LimitationsLevelActive";

	public const string LimitationsLevelName = "LimitationsLevelName";

	public const string LimitationsLevelSize = "LimitationsLevelSize";

	public const string ExportsEnabled = "ExportsEnabled";

	public const string ExportsIncludeTimestamps = "ExportsIncludeTimestamps";

	public const string ExportsAviCodec = "ExportsAviCodec";

	public const string ExportsAviBitrate = "ExportsAviBitrate";

	public const string ExportsLocation = "ExportsLocation";

	public const string ExportsActionNoDiskSpace = "ExportsActionNoDiskSpace";

	public const string ExportsLimitSizeEnabled = "ExportsLimitSizeEnabled";

	public const string ExportsLimitSizeMbValue = "ExportsLimitSizeMbValue";

	public const string ExportsViewOtherUsers = "ExportsViewOtherUsers";

	public const string ExportsEnableAvi = "ExportsEnableAvi";

	public const string ExportsEnableDb = "ExportsEnableDb";

	public const string ExportsEnableMkv = "ExportsEnableMkv";

	public const string ExportsRetentionPolicyEnabled = "ExportsRetentionPolicyEnabled";

	public const string ExportsRetentionTime = "ExportsRetentionTime";

	public const string ExportAutomaticEnabled = "ExportAutomaticEnabled";

	public const string ExportMultipleEventsConfig = "ExportMultipleEventsConfig";

	public const string ExportRulesCount = "ExportRulesCount";

	public const string ExportRuleName = "ExportRuleName";

	public const string ExportRuleEventId = "ExportRuleEventId";

	public const string ExportRuleEventKind = "ExportRuleEventKind";

	public const string ExportRuleEventType = "ExportRuleEventType";

	public const string ExportRuleCameraId = "ExportRuleCameraId";

	public const string ExportRuleDurationS = "ExportRuleDurationS";

	public const string ExportRuleExportType = "ExportRuleExportType";

	public static string UpnpDiscoverabilityEnabled = "UpnpDiscoverabilityEnabled";

	public static string AccessCustomConfigEnabled = "AccessCustomConfigEnabled";

	public static string AccessServerIpName = "AccessServerIp";

	public static string AccessServerPortName = "AccessServerPort";

	public static string AccessServerHttp = "AccessServerHttp";

	public static string AccessServerHttps = "AccessServerHttps";

	public static string StaticPortMappingEnabled = "StaticPortMappingEnabled";

	public static string SmartConnectEnabledName = "SmartConnectEnabled";

	public static string AccessDynamicIpEnabledName = "AccessDynamicIpEnabled";

	public static string AccessDisableDefaultAddressName = "AccessDisableDefaultAddress";

	public const string GeneralSettingsConfigGroup = "GeneralSettings";

	public const string AviExportConfigSection = "AviExport";

	public const string BrandingEnabled = "BrandingEnabled";

	public const string BrandingLogoLarge = "BrandingLogoLarge";

	public const string BrandingLogoSmall = "BrandingLogoSmall";

	public const string BrandingLogoWeb = "BrandingLogoWeb";

	public const string BrandingLink = "BrandingLink";

	public const string BrandingServerName = "BrandingServerName";

	public const string NotificationsEnabled = "NotificationsEnabled";

	public const string MaintainDeviceRegistration = "MaintainDeviceRegistration";

	public const string RegisteredDeviceDeviceId = "RegisteredDeviceDeviceId";

	public const string RegisteredDeviceDeviceName = "RegisteredDeviceDeviceId";

	public const string RegisteredDeviceLastLoggedUser = "RegisteredDeviceLastLoggedUser";

	public const string RegisteredDeviceTimestamp = "RegisteredDeviceTimestamp";

	public const string RegisteredDeviceEnabled = "RegisteredDeviceEnabled";

	public const string RegisteredDeviceCount = "RegisteredDeviceCount";

	public const string NotifiedDevicesConfigMask = "NotifiedDevicesConfig.";

	public const string NotifiedDevicesSId = "SID";

	public const string DeviceStateChaned = "StateChangedTimestamp";

	public const string TwoStepsVerificationAddedProviders = "TwoStepsVerificationAddedProviders";

	public const string TwoStepsVerificationSessionTimeout = "TwoStepsVerificationSessionTimeOut";

	public const string TwoStepsVerificationAllowLoginWithoutTwoStepsVerification = "TwoStepsVerificationAllowLoginWithoutTwoStepsVerification";

	public const string TwoStepsVerificationMaxTokenRetries = "TwoStepsVerificationSessionNumTries";

	public const string TwoStepsVerificationTokenTimeoutSec = "TwoStepsVerificationTokenTimeoutSec";

	public const string TwoStepsVerificationUsersConfigMask = "TwoStepsVerificationUserConfig.";

	public const string TwoStepsVerificationUsersConfigSelectedProvider = "ProviderName";

	public const string TwoStepsVerificationUsersConfigProviderData = "ProviderData";

	public const string TwoStepsVerificationUsersConfigSid = "Sid";

	public const string TwoStepsVerificationUsersConfigType = "UserType";

	public const string TwoStepsVerificationTokenSymbols = "TwoStepsVerificationTokenSymbols";

	public const string TwoStepsVerificationTokenLength = "TwoStepsVerificationTokenLength";

	public const string TwoStepsVerificationEnabled = "TwoStepsVerificationEnabled";

	public const string TwoStepsVerificationMask = "TwoStepsVerification";

	public const string UserTypesConfigMask = "UserTypesConfig.";

	public const string TwoStepsVerificationSMSProviderProductToken = "TwoStepsVerificationSMSProviderProductToken";

	public const string TwoStepsVerificationSMSProviderURL = "TwoStepsVerificationSMSProviderURL";

	public const string TwoStepsVerificationSMSProviderSender = "TwoStepsVerificationSMSProviderSender";

	public const string TwoStepsVerificationSMSProviderText = "TwoStepsVerificationSMSProviderText";

	public const string TwoStepsVerificationEMailProviderSMTPServer = "TwoStepsVerificationEMailProviderSMTPServer";

	public const string TwoStepsVerificationEMailProviderSMTPServerPort = "TwoStepsVerificationEMailProviderSMTPServerPort";

	public const string TwoStepsVerificationEMailProviderUserName = "TwoStepsVerificationEMailProviderUserName";

	public const string TwoStepsVerificationEMailProviderPassword = "TwoStepsVerificationEMailProviderPassword";

	public const string TwoStepsVerificationEMailProviderUseSSL = "TwoStepsVerificationEMailProviderUseSSL";

	public const string TwoStepsVerificationEMailProviderSenderEmail = "TwoStepsVerificationEMailProviderSenderEmail";

	public const string TwoStepsVerificationEMailProviderUseSPA = "TwoStepsVerificationEMailProviderUseSPA";

	public const string TwoStepsVerificationEMailProviderSubject = "TwoStepsVerificationEMailProviderSubject";

	public const string TwoStepsVerificationEMailProviderText = "TwoStepsVerificationEMailProviderText";

	public static readonly Guid UnsetValue = new Guid("A21A37D5-48AF-47CE-BC2D-8D07397A84BF");

	public const string AccessControlEnabled = "AccessControlEnabled";

	public static string GetEnabledValue(bool enabled)
	{
		if (!enabled)
		{
			return "False";
		}
		return "True";
	}

	public static string GetVideoUpParamPerChannelName(string param, int index)
	{
		return param + index;
	}

	public static string GetExportRulePerChannelName(string param, int index)
	{
		return param + index;
	}
}
