using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public static class SystemSettingsManager
{
	public const int MinLogLevel = 0;

	public const int MaxLogLevel = 3;

	public const int AuditPlaybackSequenceLogLengthMin = 30;

	public const int AuditPlaybackSequenceLogLengthMax = 3600;

	public const int AuditRecordsSeenBeforeLoggingMin = 0;

	public const int AuditRecordsSeenBeforeLoggingMax = 20;

	public const int BookmarkPreTimeMax = 99;

	public const int BookmarkPostTimeMax = 999;

	private static Dictionary<string, ISystemSettingManager> _settingsDictionary = new Dictionary<string, ISystemSettingManager>
	{
		{
			SupportedSystemSettings.ActivationCredentials,
			new ActivationCredentialsManager()
		},
		{
			SupportedSystemSettings.EnableAutomaticActivation,
			new EnableAutomaticActivationManager()
		},
		{
			SupportedSystemSettings.PrivacyMaskColor,
			new GenericMinMaxIntegerManager(SupportedSystemSettings.PrivacyMaskColor, int.MinValue, int.MaxValue)
		},
		{
			SupportedSystemSettings.LogAuditTimeToKeepLogEntries,
			new GenericReadonlyStringManager(SupportedSystemSettings.LogAuditTimeToKeepLogEntries)
		},
		{
			SupportedSystemSettings.LogRuleTimeToKeepLogEntries,
			new GenericReadonlyStringManager(SupportedSystemSettings.LogRuleTimeToKeepLogEntries)
		},
		{
			SupportedSystemSettings.LogSystemTimeToKeepLogEntries,
			new GenericReadonlyStringManager(SupportedSystemSettings.LogSystemTimeToKeepLogEntries)
		},
		{
			"LicenseRsaKey",
			new LicenseRsaKeyManager()
		},
		{
			SupportedSystemSettings.ServerLanAddress,
			new GenericUrlManager(SupportedSystemSettings.ServerLanAddress, allowBlank: true)
		},
		{
			SupportedSystemSettings.ServerWanAddress,
			new GenericUrlManager(SupportedSystemSettings.ServerWanAddress, allowBlank: true)
		},
		{
			"DashboardHistoryEnabled",
			new DashboardHistoryEnabledManager()
		},
		{
			"DashboardHistorySamplingIntervalSeconds",
			new DashboardHistorySamplingIntervalSecondsManager()
		},
		{
			"PtzManualSessionTimeoutSec",
			new GenericMinMaxIntegerManager("PtzManualSessionTimeoutSec", 1, 3596400)
		},
		{
			"PausePatrollingTimeoutSec",
			new GenericMinMaxIntegerManager("PausePatrollingTimeoutSec", 1, 3596400)
		},
		{
			"PtzReservedSessionTimeoutSec",
			new GenericMinMaxIntegerManager("PtzReservedSessionTimeoutSec", 1, 3596400)
		},
		{
			"PtzUseDefaultPresetAsPtzHome",
			new GenericBoolManager("PtzUseDefaultPresetAsPtzHome")
		},
		{
			"DeviceErrorTimeoutSec",
			new GenericMinMaxIntegerManager("DeviceErrorTimeoutSec", 0, 300)
		},
		{
			"LogSystemLevel",
			new GenericMinMaxIntegerManager("LogSystemLevel", 0, 3)
		},
		{
			"LogAuditEnableUserAccessLogging",
			new GenericBoolManager("LogAuditEnableUserAccessLogging")
		},
		{
			"LogAuditPlaybackWindowSizeInSec",
			new GenericMinMaxIntegerManager("LogAuditPlaybackWindowSizeInSec", 30, 3600)
		},
		{
			"LogAuditPlaybackFramesBeforeLogging",
			new GenericMinMaxIntegerManager("LogAuditPlaybackFramesBeforeLogging", 0, 20)
		},
		{
			"LogSystemEnabled",
			new GenericBoolManager("LogSystemEnabled")
		},
		{
			"LogAuditEnabled",
			new GenericBoolManager("LogAuditEnabled")
		},
		{
			"LogRuleEnabled",
			new GenericBoolManager("LogRuleEnabled")
		},
		{
			"BookmarkIdPrefix",
			new GenericStringManager("BookmarkIdPrefix")
		},
		{
			"BookmarkPreTime",
			new GenericMinMaxIntegerManager("BookmarkPreTime", 0, 99)
		},
		{
			"BookmarkPostTime",
			new GenericMinMaxIntegerManager("BookmarkPostTime", 0, 999)
		},
		{
			"CollectUsageDataEnabled",
			new GenericBoolManager("CollectUsageDataEnabled")
		},
		{
			"CollectTelemetryEnabled",
			new GenericBoolManager("CollectTelemetryEnabled")
		},
		{
			"SmtpSender",
			new GenericStringManager("SmtpSender")
		},
		{
			"SmtpServer",
			new GenericStringManager("SmtpServer")
		},
		{
			"SmtpPort",
			new GenericMinMaxIntegerManager("SmtpPort", 1, 65535)
		},
		{
			"SmtpUseSsl",
			new GenericBoolManager("SmtpUseSsl")
		},
		{
			"SmtpLogOn",
			new GenericBoolManager("SmtpLogOn")
		},
		{
			"SmtpUserName",
			new GenericStringManager("SmtpUserName")
		},
		{
			"SmtpPassword",
			new GenericStringManager("SmtpPassword")
		},
		{
			"LocalIPRanges",
			new GenericStringManager("LocalIPRanges")
		},
		{
			"ServerRecorderIPv6Comm",
			new GenericBoolManager("ServerRecorderIPv6Comm")
		},
		{
			"RetentionOptions",
			new GenericStringManager("RetentionOptions")
		},
		{
			"AVICodec",
			new GenericMinMaxIntegerManager("AVICodec", 0, int.MaxValue)
		}
	};

	public static string ConvertToDatabaseValue(IGeminiRequestContext requestContext, string name, string value)
	{
		if (!HasSetting(name))
		{
			throw new SystemSettingNotSupportedException(name);
		}
		return _settingsDictionary[name].ConvertToDatabaseValue(requestContext, value);
	}

	public static string ConvertToInternalServerValue(IGeminiRequestContext requestContext, string name, string value)
	{
		if (!HasSetting(name))
		{
			throw new SystemSettingNotSupportedException(name);
		}
		return _settingsDictionary[name].ConvertToInternalServerValue(requestContext, value);
	}

	public static string ConvertToClientValue(IGeminiRequestContext requestContext, string name, string value)
	{
		if (!HasSetting(name))
		{
			throw new SystemSettingNotSupportedException(name);
		}
		return _settingsDictionary[name].ConvertToClientValue(requestContext, value);
	}

	public static bool HasSetting(string name)
	{
		return _settingsDictionary.ContainsKey(name);
	}

	public static void CheckSetPermissions(string name, IGeminiRequestContext requestContext)
	{
		if (!HasSetting(name))
		{
			throw new SystemSettingNotSupportedException(name);
		}
		_settingsDictionary[name].CheckSetPermission(requestContext);
	}

	public static void CheckGetPermissions(string name, IGeminiRequestContext requestContext)
	{
		if (!HasSetting(name))
		{
			throw new SystemSettingNotSupportedException(name);
		}
		_settingsDictionary[name].CheckGetPermission(requestContext);
	}
}
