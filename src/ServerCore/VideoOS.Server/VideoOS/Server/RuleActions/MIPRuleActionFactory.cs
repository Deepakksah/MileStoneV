using System;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.MIPAction", "D2553916-3170-466C-9CB4-5EE25366C2B2", ActionTargetType.MIP, "Items", "ItemGroups")]
public sealed class MIPRuleActionFactory : RuleActionFactoryBase
{
	public MIPRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}

	public override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		XmlNode actionParameterByName = RuleActionFactoryBase.GetActionParameterByName(actionNode, "MipId");
		if (actionParameterByName == null)
		{
			throw new RuleConfigurationException("MipId parameter for MIPRuleAction action was not found");
		}
		XmlNode actionParameterByName2 = RuleActionFactoryBase.GetActionParameterByName(actionNode, "MipName");
		if (actionParameterByName2 == null)
		{
			throw new RuleConfigurationException("MipName parameter for MIPRuleAction action was not found");
		}
		IRuleEngine ruleEngine = base.ruleActionDataProvider.RuleEngine;
		TimeSpan delay = RuleActionFactoryBase.ParseDelay(actionNode);
		return new MIPRuleAction(ruleEngine, new Guid(actionParameterByName.InnerText), actionParameterByName2.InnerText, delay);
	}
}
