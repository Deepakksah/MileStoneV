using System;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.TriggerUserDefinedEventOnRemoteSite", "CA4D8E4F-F349-440b-B119-0F2FFA2459DE", ActionTargetType.Unspecified)]
public sealed class ActivateUserDefinedEventOnRemoteSiteRuleActionFactory : RuleActionFactoryBase
{
	public ActivateUserDefinedEventOnRemoteSiteRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}

	public override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		XmlNode actionParameterByName = RuleActionFactoryBase.GetActionParameterByName(actionNode, "SiteId");
		if (actionParameterByName == null)
		{
			throw new RuleConfigurationException("SiteId parameter for ActivateUserDefinedEventOnRemoteSite action was not found");
		}
		XmlNode actionParameterByName2 = RuleActionFactoryBase.GetActionParameterByName(actionNode, "UserDefinedEvent");
		if (actionParameterByName2 == null)
		{
			throw new RuleConfigurationException("UserDefinedEvent parameter for ActivateUserDefinedEventOnRemoteSite action was not found");
		}
		Guid siteId = new Guid(actionParameterByName.InnerText);
		string innerText = actionParameterByName2.InnerText;
		IRuleEngine ruleEngine = base.ruleActionDataProvider.RuleEngine;
		return new ActivateUserDefinedEventOnRemoteSiteRuleAction(ruleEngine, siteId, innerText);
	}
}
