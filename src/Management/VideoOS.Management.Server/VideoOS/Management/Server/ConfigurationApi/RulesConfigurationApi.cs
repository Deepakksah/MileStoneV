using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;
using VideoOS.Management.Server.ConfigurationApi.AccessControl;
using VideoOS.Management.Server.ConfigurationApi.MethodHandling;
using VideoOS.Management.Server.ConfigurationApi.Rules;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;
using VideoOS.Management.Server.ConfigurationApi.Rules.Builders;
using VideoOS.Management.Server.ConfigurationApi.Rules.Methods;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Management.Server.ConfigurationApi.Validators;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIRule")]
[EntityName("ConfigAPIRuleFolder")]
public class RulesConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	private readonly IConfigurationApiSupport _service;

	public RulesConfigurationApi()
	{
		CommonValidator commonValidator = new CommonValidator();
		EventTypeProxy eventTypeProxy = new EventTypeProxy();
		ServerInformationProxy serverInformationProxy = new ServerInformationProxy();
		MIPActionProxy mipActionProxy = new MIPActionProxy();
		RecorderPluginProxy recorderPluginProxy = new RecorderPluginProxy();
		MIPKindProxy mipKindProxy = new MIPKindProxy();
		ValueValidatorFactory validatorFactory = new ValueValidatorFactory(new DeviceProxy(), new TimeProfileProxy(), new MIPObjectProxy(), new MIPServerGroupProxy(), new HardwareProxy(), new RecordingServerProxy(), serverInformationProxy, new VideoWallProxy(), new VideoWallPresetProxy(), new MonitorProxy(), new LayoutProxy(), new ArchiveProxy(), new RecorderPluginProxy(), new DeviceSettingsProxy(), new SiteProxy(), new UserDefinedEventProxy(), new PatrollingProfileProxy());
		ConditionsParser conditionsParser = new ConditionsParser();
		RuleModelConditionsBuilder conditionsBuilder = new RuleModelConditionsBuilder();
		SiteProxy siteProxy = new SiteProxy();
		GeminiApplicationCoreProxy geminiApplicationCoreProxy = new GeminiApplicationCoreProxy();
		ConfigurationItemBuilder itemBuilder = new ConfigurationItemBuilder(eventTypeProxy, validatorFactory, mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy);
		RuleModelBuilder ruleModelBuilder = new RuleModelBuilder(conditionsBuilder, serverInformationProxy, mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy);
		RuleModelXmlBuilder modelToXmlBuilder = new RuleModelXmlBuilder(new RuleTypeXmlBuilder(), new ConditionXmlBuilder(), new ActionXmlBuilder());
		RuleParametersValidator parametersValidator = new RuleParametersValidator(validatorFactory);
		RuleTypeParser ruleTypeParser = new RuleTypeParser(eventTypeProxy, validatorFactory);
		XmlRuleModelBuilder modelFromXmlBuilder = new XmlRuleModelBuilder(conditionsParser, ruleTypeParser);
		RuleModelValidator ruleModelValidator = new RuleModelValidator(new ConditionModelValidator(parametersValidator), new EventValidator(eventTypeProxy, validatorFactory, new EventTypeProvider(eventTypeProxy)), recurringTimeValidator: new RecurringTimeValidator(), actionsValidator: new ActionsValidator(parametersValidator, mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy));
		List<IWizardStep> wizardSteps = GetWizardSteps(eventTypeProxy, mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy);
		MethodProvider methodProvider = new MethodProvider(new IMethod[2]
		{
			new AddRule(new RuleProxy(), itemBuilder, modelToXmlBuilder, ruleModelValidator, ruleModelBuilder, new RuleWizardInfoProvider(wizardSteps)),
			new RemoveRule(new RuleProxy())
		});
		RuleProxy ruleProxy = new RuleProxy();
		_service = new VideoOS.Management.Server.ConfigurationApi.Rules.ConfigurationApiService(itemBuilder, ruleModelBuilder, methodProvider, commonValidator, ruleProxy, modelToXmlBuilder, modelFromXmlBuilder, ruleModelValidator);
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		return _service.GetChildItems(requestContext, path);
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		return _service.GetItem(requestContext, path);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		return _service.InvokeMethod(requestContext, item, path, parentPath, method);
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		return _service.SetItem(requestContext, item, path, parentPath);
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		return _service.ValidateItem(requestContext, item, path, parentPath);
	}

	private static List<IWizardStep> GetWizardSteps(IEventTypeProxy eventTypeProxy, IMIPActionProxy mipActionProxy, IMIPKindProxy mipKindProxy, IRecorderPluginProxy recorderPluginProxy, ISiteProxy siteProxy, IGeminiApplicationCoreProxy geminiApplicationCoreProxy)
	{
		return new List<IWizardStep>
		{
			new BasicInfoStep(),
			new StartRuleTypeStep(eventTypeProxy, mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy),
			new StartConditionsStep(),
			new StartActionsStep(mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy),
			new StopRuleTypeStep(eventTypeProxy, mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy),
			new StopActionsStep(mipActionProxy, mipKindProxy, recorderPluginProxy, siteProxy, geminiApplicationCoreProxy)
		};
	}
}
