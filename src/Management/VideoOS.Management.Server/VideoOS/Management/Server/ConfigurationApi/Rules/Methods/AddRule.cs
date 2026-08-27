using System;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.MethodHandling;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;
using VideoOS.Management.Server.ConfigurationApi.Rules.Builders;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Methods;

public class AddRule : IMethod
{
	private readonly IRuleProxy _ruleProxy;

	private readonly IConfigurationItemBuilder _itemBuilder;

	private readonly IRuleModelXmlBuilder _modelToXmlBuilder;

	private readonly IValidator<RuleModel> _ruleModelValidator;

	private readonly IRuleModelBuilder _ruleModelBuilder;

	private readonly IRuleWizardInfoProvider _ruleWizardInfoProvider;

	public string Id => "AddRule";

	public AddRule(IRuleProxy ruleProxy, IConfigurationItemBuilder itemBuilder, IRuleModelXmlBuilder modelToXmlBuilder, IValidator<RuleModel> ruleModelValidator, IRuleModelBuilder ruleModelBuilder, IRuleWizardInfoProvider ruleWizardInfoProvider)
	{
		ruleProxy.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Methods\\AddRule.cs", 35);
		_ruleProxy = ruleProxy;
		_itemBuilder = itemBuilder;
		_modelToXmlBuilder = modelToXmlBuilder;
		_ruleModelValidator = ruleModelValidator;
		_ruleModelBuilder = ruleModelBuilder;
		_ruleWizardInfoProvider = ruleWizardInfoProvider;
	}

	public ConfigurationItem Invoke(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (!_ruleWizardInfoProvider.IsComplete(requestContext, item))
		{
			return CreateInvokeInfo(item, Id, _ruleWizardInfoProvider.GetProperties(requestContext, item));
		}
		return InvokeMethod(requestContext, item);
	}

	private static ConfigurationItem CreateInvokeInfo(ConfigurationItem item, string methodId, params Property[] properties)
	{
		return MethodTypesHelper.CreateInvokeInfo(methodId, ConfigurationApiResources.MethodAddRule, item, properties);
	}

	private ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		RuleModel ruleModel = _ruleModelBuilder.Build(requestContext, item);
		ValidationError[] source = _ruleModelValidator.Validate(requestContext, ruleModel);
		if (source.Any())
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Error when calling AddRule: {0}", source.First().Message));
		}
		string xml = _modelToXmlBuilder.GetXml(ruleModel);
		Guid id = Guid.NewGuid();
		if (ruleModel.Id != Guid.Empty)
		{
			id = ruleModel.Id;
		}
		else
		{
			ruleModel.Id = id;
		}
		_ruleProxy.CreateRule(requestContext, id.ToString(), ruleModel.Name, ruleModel.Description, xml, enabled: true);
		ConfigurationItem configurationItem = _itemBuilder.Build(requestContext, ruleModel);
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
	}
}
