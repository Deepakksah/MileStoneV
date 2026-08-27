using System;
using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class DeviceDriverSettingsProperties
{
	public const string ManualPTZTimeoutKey = "ManualPTZTimeout";

	public const string PausePatrollingTimeoutKey = "PausePatrollingTimeout";

	public const string ReservedPTZTimeoutKey = "ReservedPTZTimeout";

	public static Property GetManualPTZTimeoutProperty(int timeout, bool isSettable)
	{
		return new Property("ManualPTZTimeout", timeout.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyManualPTZTimeout,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyManualPTZTimeout",
			ToolTipTranslationId = "PropertyPtzTimeoutTooltip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property GetPausePatrollingTimeoutProperty(int timeout, bool isSettable)
	{
		return new Property("PausePatrollingTimeout", timeout.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PausePatrollingTimeout,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PausePatrollingTimeout",
			ToolTipTranslationId = "PropertyPtzTimeoutTooltip",
			UIImportance = UIImportance.Default,
			SortKey = 101
		};
	}

	public static Property GetReservedPTZTimeoutProperty(int timeout, bool isSettable)
	{
		return new Property("ReservedPTZTimeout", timeout.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.ReservedPTZTimeout,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "ReservedPTZTimeout",
			ToolTipTranslationId = "PropertyPtzTimeoutTooltip",
			UIImportance = UIImportance.Default,
			SortKey = 102
		};
	}

	public static Property StreamReferenceIdProperty(Guid streamReferenceId)
	{
		return new Property("StreamReferenceId", streamReferenceId.ToString().ToUpper(CultureInfo.InvariantCulture))
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyStreamReferenceId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyStreamReferenceId",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}
}
