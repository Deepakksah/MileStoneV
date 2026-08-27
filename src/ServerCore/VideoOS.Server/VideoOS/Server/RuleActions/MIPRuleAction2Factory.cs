using System;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.MIPAction2", "91304EDB-8637-444A-AEEA-A8D2A05E2955", ActionTargetType.MIP, "Items", "ItemGroups")]
public sealed class MIPRuleAction2Factory : RuleActionFactoryBase
{
	public MIPRuleAction2Factory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}

	public override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		XmlNode actionParameterByName = RuleActionFactoryBase.GetActionParameterByName(actionNode, "MipId");
		if (actionParameterByName == null)
		{
			throw new RuleConfigurationException("MipId parameter for MIPRuleAction2 action was not found");
		}
		XmlNode actionParameterByName2 = RuleActionFactoryBase.GetActionParameterByName(actionNode, "MipName");
		if (actionParameterByName2 == null)
		{
			throw new RuleConfigurationException("MipName parameter for MIPRuleAction2 action was not found");
		}
		XmlNode actionParameterByName3 = RuleActionFactoryBase.GetActionParameterByName(actionNode, "Items2");
		if (actionParameterByName3 == null)
		{
			throw new RuleConfigurationException("Items2 parameter for MIPRuleAction2 action was not found");
		}
		XmlNode actionParameterByName4 = RuleActionFactoryBase.GetActionParameterByName(actionNode, "ItemGroups2");
		if (actionParameterByName4 == null)
		{
			throw new RuleConfigurationException("ItemGroups2 parameter for MIPRuleAction2 action was not found");
		}
		Guid[] param = RuleActionFactoryBase.BuildGuidArray(actionParameterByName3);
		Guid[] paramGroups = RuleActionFactoryBase.BuildGuidArray(actionParameterByName4);
		IRuleEngine ruleEngine = base.ruleActionDataProvider.RuleEngine;
		TimeSpan delay = RuleActionFactoryBase.ParseDelay(actionNode);
		return new MIPRuleAction2(ruleEngine, new Guid(actionParameterByName.InnerText), actionParameterByName2.InnerText, param, paramGroups, delay);
	}
}
