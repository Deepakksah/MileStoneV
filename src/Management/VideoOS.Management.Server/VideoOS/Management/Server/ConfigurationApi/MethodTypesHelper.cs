using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class MethodTypesHelper
{
	public const string ItemSelectionKey = "ItemSelection";

	public static ConfigurationItem CreateInvokeResult(ConfigurationItem invokeInfoItem, string resultPath = null, string displayName = "Result", string invokeInfoState = "Success", bool includeItemPropertiesAndChildren = true, params Property[] resultProperties)
	{
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			ItemType = "InvokeResult",
			DisplayName = displayName,
			UIHint = "",
			ItemCategory = ItemCategories.Item,
			ParentPath = invokeInfoItem.ParentPath,
			Path = invokeInfoItem.Path,
			MethodIds = Array.Empty<string>()
		};
		List<Property> list = new List<Property>(includeItemPropertiesAndChildren ? invokeInfoItem.Properties : Array.Empty<Property>());
		if (resultProperties != null)
		{
			list.AddRange(resultProperties);
		}
		list.Add(new Property("State", ConfigurationApiResources.InvokeState, invokeInfoState));
		if (resultPath != null)
		{
			list.Add(new Property("Path", "Path", resultPath));
		}
		configurationItem.Properties = list.ToArray();
		if (includeItemPropertiesAndChildren)
		{
			configurationItem.Children = invokeInfoItem.Children;
		}
		return configurationItem;
	}

	public static ConfigurationItem CreateInvokeResultWithoutItemPropertiesAndChildren(ConfigurationItem invokeInfoItem)
	{
		return CreateInvokeResult(invokeInfoItem, null, "Result", "Success", false);
	}

	public static ConfigurationItem CreateInvokeInfo(string methodId, string displayName, ConfigurationItem item, params Property[] properties)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = displayName;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.EnableProperty = GeneralPropertyDefinitions.GetEnabledProperty(enabled: true, uiFollowsEnabled: true);
		configurationItem.Properties = properties;
		return configurationItem;
	}

	public static ConfigurationItem CreateInvokeMemberSelectionInfo(string methodId, string displayName, string itemTypeTranslationId, string propertyDisplayName, ConfigurationItem item, ConfigurationItem[] members)
	{
		return CreateInvokeMemberSelectionInfo(methodId, displayName, itemTypeTranslationId, propertyDisplayName, item, members, (ConfigurationItem ci) => ci.Path);
	}

	public static ConfigurationItem CreateInvokeMemberSelectionInfo(string methodId, string displayName, string itemTypeTranslationId, string propertyDisplayName, ConfigurationItem item, ConfigurationItem[] members, Func<ConfigurationItem, string> memberValueFunc)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = displayName;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[1] { PropertyTypesHelper.GetMembersAsEnumsProperty("ItemSelection", propertyDisplayName, itemTypeTranslationId, "", members, memberValueFunc) };
		return configurationItem;
	}

	public static ConfigurationItem CreateInvokeMemberSelectionInfo(string methodId, string displayName, string itemTypeTranslationId, string propertyDisplayName, ConfigurationItem item, Dictionary<string, string> members)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = displayName;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[1] { GeneralPropertyDefinitions.GetEnumProperty("ItemSelection", "", propertyDisplayName, itemTypeTranslationId, "", members) };
		return configurationItem;
	}

	public static ConfigurationItemPath GetInvokeMemberItemSelectionPath(ConfigurationItem item)
	{
		string itemSelectionPropertyValue = GetItemSelectionPropertyValue(item);
		try
		{
			return ConfigurationItemPath.CreatePath(itemSelectionPropertyValue);
		}
		catch (ArgumentException inner)
		{
			throw new PropertyInvalidException("Invalid path selection. " + itemSelectionPropertyValue, "ItemSelection", inner);
		}
	}

	public static string GetItemSelectionPropertyValue(ConfigurationItem item)
	{
		string value = null;
		if (item.Properties == null || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ItemSelection", out value))
		{
			throw new PropertyInvalidException("No selection.", "ItemSelection");
		}
		return value;
	}
}
