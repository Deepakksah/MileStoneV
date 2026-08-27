using System;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Server.ConfigurationApi.MethodHandling;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Methods;

public class RemoveRule : IMethod
{
	private readonly IRuleProxy _ruleProxy;

	public string Id => "RemoveRule";

	public RemoveRule(IRuleProxy ruleProxy)
	{
		ruleProxy.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Methods\\RemoveRule.cs", 21);
		_ruleProxy = ruleProxy;
	}

	public ConfigurationItem Invoke(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (!string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
		{
			return CreateInvokeInfo(item, Id);
		}
		return InvokeMethod(requestContext, item);
	}

	private ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		string text = item.Properties.FirstOrDefault((Property x) => x.Key == "RemoveRulePath")?.Value;
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(text);
		Rule rule = _ruleProxy.QueryRuleById(requestContext, configurationItemPath.Id);
		if (rule == null)
		{
			throw new PathNotFoundException(text);
		}
		_ruleProxy.DeleteRule(requestContext, rule.Id);
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static ConfigurationItem CreateInvokeInfo(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodRemoveRule;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[1] { GeneralPropertyDefinitions.GetPathSelectionProperty("Rule", "RemoveRulePath", ConfigurationApiResources.PropertySelectRuleToRemove) };
		return configurationItem;
	}
}
