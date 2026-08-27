using System.Collections.Generic;
using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class PatrollingProfileDefinition
{
	public const string OrderKey = "Order";

	public const string SpeedKey = "Speed";

	public const string TransitionTimeKey = "TransitionTime";

	public const string WaitTimeKey = "WaitTime";

	public const string PresetIdKey = "PresetId";

	public const string CustomizeTransitionsKey = "CustomizeTransitions";

	public const string EndSpeedKey = "EndSpeed";

	public const string EndTransitionTimeKey = "EndTransitionTime";

	public const string EndPresetIdKey = "EndPresetId";

	public const string InitSpeedKey = "InitSpeed";

	public const string InitTransitionTimeKey = "InitTransitionTime";

	public static Property OrderProperty(int order)
	{
		return new Property("Order", order.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPatrollingEntryOrder,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPatrollingEntryOrder",
			ToolTipTranslationId = "PropertyPatrollingEntryOrderToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property SpeedProperty(double d)
	{
		return new Property("Speed", d.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPatrollingSpeed,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPatrollingSpeed",
			ToolTipTranslationId = "PropertyPatrollingSpeedToolTip",
			UIImportance = UIImportance.Default,
			ValueTypeInfos = PropertyTypesHelper.GetDoubleRange(0.0, 1.0),
			SortKey = 100
		};
	}

	public static Property EndSpeedProperty(double d)
	{
		return new Property("EndSpeed", d.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPatrollingEndSpeed,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPatrollingEndSpeed",
			ToolTipTranslationId = "PropertyPatrollingSpeedToolTip",
			UIImportance = UIImportance.Default,
			ValueTypeInfos = PropertyTypesHelper.GetDoubleRange(0.0, 1.0),
			SortKey = 100
		};
	}

	public static Property InitSpeedProperty(double d)
	{
		return new Property("InitSpeed", d.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPatrollingInitSpeed,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPatrollingInitSpeed",
			ToolTipTranslationId = "PropertyPatrollingSpeedToolTip",
			UIImportance = UIImportance.Default,
			ValueTypeInfos = PropertyTypesHelper.GetDoubleRange(0.0, 1.0),
			SortKey = 100
		};
	}

	public static Property TransitionTimeProperty(int seconds)
	{
		return new Property("TransitionTime", seconds.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPatrollingTransitionTime,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPatrollingTransitionTime",
			ToolTipTranslationId = "PropertyPatrollingTransitionTimeToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property EndTransitionTimeProperty(int seconds)
	{
		return new Property("EndTransitionTime", seconds.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPatrollingEndTransitionTime,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPatrollingEndTransitionTime",
			ToolTipTranslationId = "PropertyPatrollingTransitionTimeToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property InitTransitionTimeProperty(int seconds)
	{
		return new Property("InitTransitionTime", seconds.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPatrollingInitTransitionTime,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPatrollingInitTransitionTime",
			ToolTipTranslationId = "PropertyPatrollingTransitionTimeToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property WaitTimeProperty(int? seconds)
	{
		return new Property("WaitTime", seconds?.ToString(CultureInfo.InvariantCulture) ?? "3000")
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPatrollingWaitTime,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPatrollingWaitTime",
			ToolTipTranslationId = "PropertyPatrollingWaitTimeToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property PresetIdProperty(string presetId, Dictionary<string, string> presets)
	{
		return GeneralPropertyDefinitions.GetEnumProperty("PresetId", (presetId ?? "").ToLowerInvariant(), ConfigurationApiResources.PropertyPatrollingPresetId, "PropertyPatrollingPresetId", "", presets);
	}

	public static Property EndPresetIdProperty(string presetId, Dictionary<string, string> presets)
	{
		return GeneralPropertyDefinitions.GetEnumProperty("EndPresetId", (presetId ?? "").ToLowerInvariant(), ConfigurationApiResources.PropertyPatrollingEndPresetId, "PropertyPatrollingEndPresetId", "", presets);
	}

	public static Property CustomizeTransitionsProperty(bool customizeTransitions)
	{
		return new Property("CustomizeTransitions", customizeTransitions.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyPatrollingCustomizeTransitions,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPatrollingCustomizeTransitions",
			ToolTipTranslationId = "PropertyPatrollingCustomizeTransitionsToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}
}
