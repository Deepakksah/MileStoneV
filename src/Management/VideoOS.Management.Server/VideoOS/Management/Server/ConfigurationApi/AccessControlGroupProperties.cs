using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public class AccessControlGroupProperties
{
	public const string GroupNameKey = "GroupName";

	public const string GroupDescriptionKey = "GroupDescription";

	public const string RemoveMembers = "RemoveMembers";

	public static Property GetSelectGroupName()
	{
		return new Property("GroupName", "")
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyGroupName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGroupName",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	public static Property GetSelectGroupDescription()
	{
		return new Property("GroupDescription", "")
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyGroupDescription,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGroupDescription",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	public static Property RemoveMembersProperty(bool removeMembers)
	{
		return new Property("RemoveMembers", removeMembers.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyAccessControlGroupRemoveMembers,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyAccessControlGroupRemoveMembers"
		};
	}

	public static Property GetMembersAsEnumsProperty(ConfigurationItem[] members)
	{
		Property property = new Property("ItemSelection", "")
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertySelectAccessControlToRemove,
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
}
