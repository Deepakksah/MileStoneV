using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public class ConfigurationItemBuilder : IConfigurationItemBuilder
{
	private readonly IConfigurationPropertiesBuilder<RulePartModel> _conditionsBuilder;

	private readonly IConfigurationPropertiesBuilder<RuleModel> _actionPropertiesBuilder;

	private readonly IConfigurationPropertiesBuilder<RulePartModel> _ruleTypePropertiesBuilder;

	public ConfigurationItemBuilder(IEventTypeProxy eventTypeProxy, IValueValidatorFactory validatorFactory, IMIPActionProxy mipActionProxy, IMIPKindProxy mipKindProxy, IRecorderPluginProxy recorderPluginProxy, ISiteProxy siteProxy, IGeminiApplicationCoreProxy geminiApplicationCoreProxy)
	{
		eventTypeProxy.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Builders\\ConfigurationItem\\ConfigurationItemBuilder.cs", 20);
		validatorFactory.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Builders\\ConfigurationItem\\ConfigurationItemBuilder.cs", 21);
		_conditionsBuilder = new ConfigurationItemConditionsBuilder();
		_actionPropertiesBuilder = new ConfigurationItemActionPropertiesBuilder(validatorFactory, mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy);
		_ruleTypePropertiesBuilder = new ConfigurationItemRuleTypeBuilder(eventTypeProxy);
	}

	public ConfigurationItem Build(IGeminiRequestContext requestContext, RuleModel rule)
	{
		rule.NullCheck("", "Build", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Builders\\ConfigurationItem\\ConfigurationItemBuilder.cs", 29);
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateSystemPath(), "RuleFolder");
		ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreateItemPath("Rule", rule.Id.ToString());
		Property[] properties = BuildItemProperties(requestContext, rule);
		return new ConfigurationItem
		{
			EnableProperty = GeneralPropertyDefinitions.GetEnabledProperty(rule.Enabled, uiFollowsEnabled: false),
			ItemType = "Rule",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = rule.Name,
			ParentPath = configurationItemPath.Text,
			Path = configurationItemPath2.Text,
			Properties = properties,
			MethodIds = new string[0]
		};
	}

	private Property[] BuildItemProperties(IGeminiRequestContext requestContext, RuleModel rule)
	{
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(rule.Id, uiVisible: true));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(rule.Name, isSettable: true));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(rule.Description, isSettable: true));
		list.AddRange(_ruleTypePropertiesBuilder.Build(requestContext, rule.RuleStart));
		list.AddRange(_ruleTypePropertiesBuilder.Build(requestContext, rule.RuleStop));
		list.AddRange(_conditionsBuilder.Build(requestContext, rule.RuleStart));
		list.AddRange(_actionPropertiesBuilder.Build(requestContext, rule));
		return list.ToArray();
	}
}
