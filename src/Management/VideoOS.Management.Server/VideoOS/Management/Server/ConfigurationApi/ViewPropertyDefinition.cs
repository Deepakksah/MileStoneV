using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class ViewPropertyDefinition
{
	public const string ShortcutKey = "Shortcut";

	public const string ViewLayoutTypeKey = "ViewLayoutType";

	public const string LayoutViewItemsKey = "LayoutViewItems";

	public const string LayoutIconKey = "LayoutIcon";

	public const string LayoutCustomIdKey = "LayoutCustomId";

	public const string ViewItemPositionKey = "ViewItemPosition";

	public const string ViewItemDefinitionXmlKey = "ViewItemDefinitionXml";

	public const string ClientIdKey = "ClientId";

	public const string ViewItemIdKey = "ViewItemId";

	public const string ViewToApplyKey = "ViewToApply";

	public static Property GetViewItemPositionProperty(string position)
	{
		return new Property("ViewItemPosition", position)
		{
			ValueType = "Int",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyViewItemPosition,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyViewItemPosition",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property GetViewItemDefinitionXmlProperty(string xml)
	{
		return new Property("ViewItemDefinitionXml", xml)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyViewItemDefinitionXml,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyViewItemDefinitionXml",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property GetShortcutProperty(string shortcut)
	{
		return new Property("Shortcut", shortcut)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyViewShortcut,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyViewShortcut",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property GetViewLayoutTypeProperty(string viewLayoutType)
	{
		return new Property("ViewLayoutType", viewLayoutType)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyViewLayoutType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyViewLayoutType",
			ToolTipTranslationId = "PropertyViewLayoutTypeToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property GetViewLayoutCustomIdProperty(string id)
	{
		return new Property("LayoutCustomId", id ?? "")
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyLayoutCustomId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLayoutCustomId",
			ToolTipTranslationId = "PropertyLayoutCustomIdToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property GetViewLayoutIconProperty(string base64Icon)
	{
		return new Property("LayoutIcon", base64Icon)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyViewLayoutIcon,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyViewLayoutIcon",
			ToolTipTranslationId = "PropertyViewLayoutIconToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property GetLayoutViewItemsProperty(string xml)
	{
		return new Property("LayoutViewItems", xml)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyLayoutViewItems,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyLayoutViewItems",
			ToolTipTranslationId = "PropertyLayoutViewItemsToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property GetMembersAsEnumsProperty(ConfigurationItem[] members)
	{
		Property property = new Property("ItemSelection", "")
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertySelectMemberToRemove,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		foreach (ConfigurationItem configurationItem in members)
		{
			list.Add(new ValueTypeInfo
			{
				Name = configurationItem.DisplayName,
				TranslationId = "",
				Value = configurationItem.Path
			});
		}
		property.ValueTypeInfos = list.ToArray();
		return property;
	}

	public static Property GetClientIdProperty(string clientId)
	{
		return new Property("ClientId", clientId ?? "")
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyViewClientId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyViewClientId",
			ToolTipTranslationId = "PropertyViewClientIdToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property GetViewItemIdProperty(string viewItemId)
	{
		return new Property("ViewItemId", viewItemId ?? "")
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyViewItemId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyViewItemId",
			ToolTipTranslationId = "PropertyViewItemIdToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}
}
