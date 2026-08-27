using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class VideoWallPropertyDefinition
{
	public static Property HeightProperty(double height)
	{
		return new Property("Height", PropertyTypesHelper.GetValue(height))
		{
			ValueType = "Double",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyVideoWallHeight,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyVideoWallHeight",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property WidthProperty(double width)
	{
		return new Property("Width", PropertyTypesHelper.GetValue(width))
		{
			ValueType = "Double",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyVideoWallWidth,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyVideoWallWidth",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property StatusTextProperty(bool statusText)
	{
		return new Property("StatusText", statusText.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyVideoWallStatusText,
			ItemReference = false,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyVideoWallStatusTextToolTip",
			TranslationId = "PropertyVideoWallStatusText",
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property TitleBarProperty(TitleBar titleBar)
	{
		Property obj = new Property("TitleBar", titleBar.ToString())
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyVideoWallTitleBar,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyVideoWallTitleBar",
			ToolTipTranslationId = "PropertyVideoWallTitleBarToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
		List<ValueTypeInfo> list = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(TitleBar)).ToList();
		list.Remove(list.Last());
		obj.ValueTypeInfos = list.ToArray();
		return obj;
	}
}
