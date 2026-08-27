using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.AccessControl.Builders;

public class ConfigurationItemBuilder : IConfigurationItemBuilder
{
	public ConfigurationItem BuildAccessControlItem(ConfigurationItemPath path, ConfigurationItemPath parentPath, MIPServerGroup serverGroup, MIPObject mipObject, string[] methodIds)
	{
		path.NullCheck("", "BuildAccessControlItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\AccessControl\\Builders\\ConfigurationItemBuilder.cs", 19);
		parentPath.NullCheck("", "BuildAccessControlItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\AccessControl\\Builders\\ConfigurationItemBuilder.cs", 20);
		mipObject.NullCheck("", "BuildAccessControlItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\AccessControl\\Builders\\ConfigurationItemBuilder.cs", 21);
		serverGroup.NullCheck("", "BuildAccessControlItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\AccessControl\\Builders\\ConfigurationItemBuilder.cs", 22);
		Property[] properties = BuildAccessControlItemProperties(serverGroup.Name, mipObject);
		return new ConfigurationItem
		{
			ItemType = path.ItemType,
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = serverGroup.Name,
			ParentPath = parentPath.Text,
			Path = path.Text,
			Properties = properties,
			MethodIds = methodIds
		};
	}

	public ConfigurationItem BuildAccessControlItem(ConfigurationItemPath path, ConfigurationItemPath parentPath, string itemName, MIPObject mipObject, string[] methodIds)
	{
		path.NullCheck("", "BuildAccessControlItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\AccessControl\\Builders\\ConfigurationItemBuilder.cs", 40);
		parentPath.NullCheck("", "BuildAccessControlItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\AccessControl\\Builders\\ConfigurationItemBuilder.cs", 41);
		mipObject.NullCheck("", "BuildAccessControlItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\AccessControl\\Builders\\ConfigurationItemBuilder.cs", 42);
		itemName.NullCheck("", "BuildAccessControlItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\AccessControl\\Builders\\ConfigurationItemBuilder.cs", 43);
		Property[] properties = BuildAccessControlItemProperties(itemName, mipObject);
		return new ConfigurationItem
		{
			ItemType = path.ItemType,
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = itemName,
			ParentPath = parentPath.Text,
			Path = path.Text,
			Properties = properties,
			MethodIds = methodIds
		};
	}

	private static Property[] BuildAccessControlItemProperties(string itemName, MIPObject mipObject)
	{
		return new Property[3]
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(mipObject.Id),
			GeneralPropertyDefinitions.GetNameProperty(itemName, isSettable: false),
			CreateRuleGroupProperty(mipObject)
		};
	}

	public static string GetRuleGroupPath(MIPObject mIPObject)
	{
		string arg = mIPObject.RelationIds.Split(';').FirstOrDefault().Trim();
		return string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", "AccessControlRuleGroup", arg);
	}

	private static Property CreateRuleGroupProperty(MIPObject mipObject)
	{
		string ruleGroupPath = GetRuleGroupPath(mipObject);
		Property stringProperty = GeneralPropertyDefinitions.GetStringProperty("RuleGroup", ruleGroupPath, ConfigurationApiResources.PropertyAccessControlRuleGroup, "PropertyAccessControlRuleGroup", isSettable: false);
		stringProperty.ValueType = "Path";
		stringProperty.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "AccessControlRuleGroup"
			}
		};
		return stringProperty;
	}
}
