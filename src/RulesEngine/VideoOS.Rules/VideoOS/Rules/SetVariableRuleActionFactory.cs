using System;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

[RuleActionFactory("VideoOS.SetVariable", "98089DBA-5F14-4de1-97F0-3CD27DB29679", ActionTargetType.Unspecified)]
public class SetVariableRuleActionFactory : RuleActionFactoryBase
{
	public SetVariableRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}

	public override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		XmlNode actionParameterByName = RuleActionFactoryBase.GetActionParameterByName(actionNode, "VariableInstanceId");
		if (actionParameterByName == null)
		{
			throw new RuleConfigurationException("VariableInstanceId parameter for SetVariable action was not found");
		}
		IRuleEngine ruleEngine = base.ruleActionDataProvider.RuleEngine;
		IRuleVariableManager ruleVariableManager = base.ruleActionDataProvider.RuleEngine.RuleVariableManager;
		Guid variableInstanceId = new Guid(actionParameterByName.InnerText.Trim());
		Guid variableId = new Guid(RuleActionFactoryBase.GetActionParameterByName(actionNode, "VariableId").InnerText.Trim());
		string variableValue = RuleActionFactoryBase.GetActionParameterByName(actionNode, "Text").InnerText.Trim();
		TimeSpan delay = RuleActionFactoryBase.ParseDelay(actionNode);
		return new SetVariableRuleAction(ruleEngine, ruleVariableManager, variableInstanceId, variableId, variableValue, delay);
	}
}
