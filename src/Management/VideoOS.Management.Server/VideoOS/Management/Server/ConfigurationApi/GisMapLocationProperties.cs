using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class GisMapLocationProperties
{
	public static Property PositionXProperty(double val, bool isSettable)
	{
		return new Property("PositionX", val.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Double",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyGisMapLocationPositionX,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGisMapLocationPositionX",
			ToolTipTranslationId = "PropertyGisMapLocationPositionXTooltip",
			UIImportance = UIImportance.Summary,
			SortKey = 100
		};
	}

	public static Property PositionYProperty(double val, bool isSettable)
	{
		return new Property("PositionY", val.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Double",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyGisMapLocationPositionY,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGisMapLocationPositionY",
			ToolTipTranslationId = "PropertyGisMapLocationPositionYTooltip",
			UIImportance = UIImportance.Summary,
			SortKey = 100
		};
	}

	public static Property ScaleProperty(double val, bool isSettable)
	{
		return new Property("Scale", val.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Double",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyGisMapLocationScale,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGisMapLocationScale",
			ToolTipTranslationId = "PropertyGisMapLocationScaleTooltip",
			UIImportance = UIImportance.Summary,
			SortKey = 100
		};
	}

	public static Property ColorProperty(string val, bool isSettable)
	{
		return new Property("Color", val)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyGisMapLocationColor,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGisMapLocationColor",
			ToolTipTranslationId = "PropertyGisMapLocationColorTooltip",
			UIImportance = UIImportance.Summary,
			SortKey = 100
		};
	}
}
