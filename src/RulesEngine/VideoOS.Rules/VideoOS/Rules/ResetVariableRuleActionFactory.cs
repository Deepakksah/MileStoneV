using System;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

[RuleActionFactory("VideoOS.ResetVariable", "E79399D0-242C-4374-B417-91F856863212", ActionTargetType.Unspecified)]
public class ResetVariableRuleActionFactory : RuleActionFactoryBase
{
	public ResetVariableRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}

	public override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		XmlNode actionParameterByName = RuleActionFactoryBase.GetActionParameterByName(actionNode, "VariableInstanceId");
		if (actionParameterByName == null)
		{
			throw new RuleConfigurationException("VariableInstanceId parameter for ResetVariable action was not found");
		}
		IRuleEngine ruleEngine = base.ruleActionDataProvider.RuleEngine;
		IRuleVariableManager ruleVariableManager = base.ruleActionDataProvider.RuleEngine.RuleVariableManager;
		Guid variableInstanceId = new Guid(actionParameterByName.InnerText.Trim());
		Guid variableId = new Guid(RuleActionFactoryBase.GetActionParameterByName(actionNode, "VariableId").InnerText.Trim());
		TimeSpan delay = RuleActionFactoryBase.ParseDelay(actionNode);
		return new ResetVariableRuleAction(ruleEngine, ruleVariableManager, variableInstanceId, variableId, delay);
	}
}
