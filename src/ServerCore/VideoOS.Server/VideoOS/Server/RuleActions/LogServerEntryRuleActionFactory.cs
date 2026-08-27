using System;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.CreateLogEntry", "AA8649A5-49B9-48bf-A18F-8FFF2DDCF5D5", ActionTargetType.Unspecified)]
public sealed class LogServerEntryRuleActionFactory : RuleActionFactoryBase
{
	public LogServerEntryRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}

	public override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		if (subRule == null)
		{
			throw new ArgumentNullException("subRule");
		}
		XmlNode actionParameterByName = RuleActionFactoryBase.GetActionParameterByName(actionNode, "Text");
		if (actionParameterByName == null)
		{
			throw new RuleConfigurationException("Text parameter for CreateLogEntry action was not found");
		}
		Server server = ((ISecretServerRuleActionDataProvider)base.ruleActionDataProvider).Server;
		IRuleEngine ruleEngine = base.ruleActionDataProvider.RuleEngine;
		return new LogServerEntryRuleAction(ruleEngine, server, subRule.ParentRule.ID, actionParameterByName.InnerText.Trim());
	}
}
