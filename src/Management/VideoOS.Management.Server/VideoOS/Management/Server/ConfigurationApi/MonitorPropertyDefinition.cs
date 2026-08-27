using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class MonitorPropertyDefinition
{
	public static Property LocationXProperty(double locationX)
	{
		return new Property("LocationX", PropertyTypesHelper.GetValue(locationX))
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMonitorLocationX,
			ItemReference = false,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyMonitorLocationXToolTip",
			TranslationId = "PropertyMonitorLocationX",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetDoubleRange(0.0, double.MaxValue)
		};
	}

	public static Property LocationYProperty(double locationY)
	{
		return new Property("LocationY", PropertyTypesHelper.GetValue(locationY))
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMonitorLocationY,
			ItemReference = false,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyMonitorLocationYToolTip",
			TranslationId = "PropertyMonitorLocationY",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetDoubleRange(0.0, double.MaxValue)
		};
	}

	public static Property MonitorSizeProperty(double monitorSize)
	{
		return new Property("MonitorSize", PropertyTypesHelper.GetValue(monitorSize))
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMonitorSize,
			ItemReference = false,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyMonitorSizeToolTip",
			TranslationId = "PropertyMonitorSize",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetDoubleRange(5.0, 3.4028234663852886E+38)
		};
	}

	public static Property MonitorStateProperty(string monitorState, bool isSettable)
	{
		return new Property("MonitorState", monitorState)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyMonitorState,
			ItemReference = false,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyMonitorStateToolTip",
			TranslationId = "PropertyMonitorState",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property AspectRatioProperty(AspectRatio aspectRatio)
	{
		return new Property("AspectRatio", aspectRatio.ToString())
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMonitorAspectRatio,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMonitorAspectRatio",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(AspectRatio))
		};
	}

	public static Property InsertionMethodProperty(ElementInsertionMethod insertionMethod)
	{
		return new Property("InsertionMethod", insertionMethod.ToString())
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMonitorInsertionMethod,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMonitorInsertionMethod",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(ElementInsertionMethod))
		};
	}

	public static Property EmptyViewItemsProperty(PresetLayoutMethod emptyViewItems)
	{
		return new Property("EmptyViewItems", emptyViewItems.ToString())
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMonitorEmptyViewItems,
			ItemReference = false,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyMonitorEmptyViewItemsToolTip",
			TranslationId = "PropertyMonitorEmptyViewItems",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(PresetLayoutMethod))
		};
	}

	public static Property NoLayoutProperty(PresetLayoutMethod noLayout)
	{
		return new Property("NoLayout", noLayout.ToString())
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMonitorNoLayout,
			ItemReference = false,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyMonitorNoLayoutToolTip",
			TranslationId = "PropertyMonitorNoLayout",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(PresetLayoutMethod))
		};
	}

	public static Property ViewProperty(string viewPath)
	{
		Property pathProperty = GeneralPropertyDefinitions.GetPathProperty("View", ConfigurationApiResources.PropertyMonitorView, viewPath, new string[1] { "View" }, "PropertyMonitorView", isSettable: false);
		pathProperty.ToolTipTranslationId = "PropertyMonitorViewToolTip";
		return pathProperty;
	}
}
