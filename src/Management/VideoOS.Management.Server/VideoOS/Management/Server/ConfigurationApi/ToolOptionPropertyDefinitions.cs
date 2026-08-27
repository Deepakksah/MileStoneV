using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using VideoOS.Common.Integration;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class ToolOptionPropertyDefinitions
{
	public const string PtzManualSessionTimeoutSec = "PtzManualSessionTimeoutSec";

	public const string PausePatrollingTimeoutSec = "PausePatrollingTimeoutSec";

	public const string PtzReservedSessionTimeoutSec = "PtzReservedSessionTimeoutSec";

	public const string PtzUseDefaultPresetAsPtzHome = "PtzUseDefaultPresetAsPtzHome";

	public const string DeviceErrorTimeoutSec = "DeviceErrorTimeoutSec";

	public const string SystemLogEnabled = "SystemLogEnabled";

	public const string SystemDaysToKeep = "SystemDaysToKeep";

	public const string SystemLogLevel = "SystemLogLevel";

	public const string AuditLogEnabled = "AuditLogEnabled";

	public const string AuditDaysToKeep = "AuditDaysToKeep";

	public const string AuditUserAccessLogging = "AuditUserAccessLogging";

	public const string AuditPlaybackSequenceLogLength = "AuditPlaybackSequenceLogLength";

	public const string AuditRecordsSeenBeforeLogging = "AuditRecordsSeenBeforeLogging";

	public const string RuleLogEnabled = "RuleLogEnabled";

	public const string RuleDaysToKeep = "RuleDaysToKeep";

	public const string BookmarkIdPrefix = "BookmarkIdPrefix";

	public const string BookmarkPreTime = "BookmarkPreTime";

	public const string BookmarkPostTime = "BookmarkPostTime";

	public const string CollectUsageDataEnabled = "CollectUsageDataEnabled";

	public const string CollectTelemetryEnabled = "CollectTelemetryEnabled";

	public const string SmtpSender = "SmtpSender";

	public const string SmtpServer = "SmtpServer";

	public const string SmtpPort = "SmtpPort";

	public const string SmtpUseSsl = "SmtpUseSsl";

	public const string SmtpLogOn = "SmtpLogOn";

	public const string SmtpUsername = "SmtpUsername";

	public const string SmtpPassword = "SmtpPassword";

	public const string NetworkRangeList = "NetworkRangeList";

	public const string ServerRecorderIPv6Comm = "ServerRecorderIPv6Comm";

	public const string ServerLanAddress = "ServerLanAddress";

	public const string ServerWanAddress = "ServerWanAddress";

	public const string AviGenerationCompressor = "Compressor";

	public static Property GetIntProperty(string key, string value, string toolTipId, bool settable = true)
	{
		ResourceManager resourceManager = ConfigurationApiResources.ResourceManager;
		Property intProperty = GeneralPropertyDefinitions.GetIntProperty(key, resourceManager.GetString("ToolOption_" + key, CultureInfo.InvariantCulture), value, "ToolOption_" + key, !string.IsNullOrEmpty(value));
		intProperty.ToolTipTranslationId = toolTipId;
		intProperty.IsSettable = settable;
		return intProperty;
	}

	public static Property GetRangeIntProperty(string key, string value, string toolTipId, int min, int max, bool isSettable)
	{
		ResourceManager resourceManager = ConfigurationApiResources.ResourceManager;
		Property intProperty = GeneralPropertyDefinitions.GetIntProperty(key, resourceManager.GetString("ToolOption_" + key, CultureInfo.InvariantCulture), value, "ToolOption_" + key, !string.IsNullOrEmpty(value));
		intProperty.ToolTipTranslationId = toolTipId;
		intProperty.IsSettable = isSettable;
		intProperty.ValueTypeInfos = new ValueTypeInfo[2]
		{
			new ValueTypeInfo
			{
				Name = "MinValue",
				Value = min.ToString(CultureInfo.InvariantCulture)
			},
			new ValueTypeInfo
			{
				Name = "MaxValue",
				Value = max.ToString(CultureInfo.InvariantCulture)
			}
		};
		return intProperty;
	}

	public static Property GetStringProperty(string key, string value, string toolTipId, bool settable = true)
	{
		ResourceManager resourceManager = ConfigurationApiResources.ResourceManager;
		Property stringProperty = GeneralPropertyDefinitions.GetStringProperty(key, value, resourceManager.GetString("ToolOption_" + key, CultureInfo.InvariantCulture), "ToolOption_" + key, isSettable: true);
		stringProperty.ToolTipTranslationId = toolTipId;
		stringProperty.IsSettable = settable;
		return stringProperty;
	}

	public static Property GetTickProperty(string key, string value, string toolTipId, bool settable)
	{
		string displayName = ConfigurationApiResources.ResourceManager.GetString("ToolOption_" + key, CultureInfo.InvariantCulture)?.Replace("{OemClientName}", OemResources.Instance.SmartClient.ClientName);
		Property tickProperty = GeneralPropertyDefinitions.GetTickProperty(key, displayName, value, "ToolOption_" + key, settable);
		tickProperty.ToolTipTranslationId = toolTipId;
		return tickProperty;
	}

	public static Property GetLogLevelProperty(string key, string value, string toolTipId)
	{
		Dictionary<string, string> members = new Dictionary<string, string>
		{
			{
				"0",
				ConfigurationApiResources.ToolOption_LogLevelAll
			},
			{
				"1",
				ConfigurationApiResources.ToolOption_LogLevel1
			},
			{
				"2",
				ConfigurationApiResources.ToolOption_LogLevel2
			},
			{
				"3",
				ConfigurationApiResources.ToolOption_LogLevel3
			}
		};
		ResourceManager resourceManager = ConfigurationApiResources.ResourceManager;
		return GeneralPropertyDefinitions.GetEnumProperty(key, value, resourceManager.GetString("ToolOption_" + key, CultureInfo.InvariantCulture), "ToolOption_" + key, toolTipId, members);
	}

	public static Property GetDeviceErrorTimeoutSecProperty(string value, string toolTipId)
	{
		ResourceManager resourceManager = ConfigurationApiResources.ResourceManager;
		Dictionary<string, string> members = new Dictionary<string, string>
		{
			{ "0", "0" },
			{ "5", "5" },
			{ "10", "10" },
			{ "15", "15" },
			{ "30", "30" },
			{ "45", "45" },
			{ "60", "60" },
			{ "120", "120" },
			{ "300", "300" }
		};
		return GeneralPropertyDefinitions.GetEnumProperty("DeviceErrorTimeoutSec", value, resourceManager.GetString("ToolOption_DeviceErrorTimeoutSec"), "ToolOption_DeviceErrorTimeoutSec", toolTipId, members);
	}

	public static Property GetNetworkListRangeProperty(string key, string[] valueArray, string tooltipId)
	{
		Property stringProperty = GetStringProperty(key, "", tooltipId);
		stringProperty.ValueType = "Array";
		stringProperty.ValueArray = valueArray;
		stringProperty.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "ArrayValueType",
				Value = "String"
			}
		};
		return stringProperty;
	}

	public static Property GetAviCodecProperty(string key, string currentValue, string toolTipId)
	{
		ResourceManager resourceManager = ConfigurationApiResources.ResourceManager;
		Property pathProperty = GeneralPropertyDefinitions.GetPathProperty(key, resourceManager.GetString("ToolOption_" + key, CultureInfo.InvariantCulture), currentValue, new string[1] { "Compressor" }, "ToolOption_" + key, isSettable: true);
		pathProperty.ToolTipTranslationId = toolTipId;
		return pathProperty;
	}
}
