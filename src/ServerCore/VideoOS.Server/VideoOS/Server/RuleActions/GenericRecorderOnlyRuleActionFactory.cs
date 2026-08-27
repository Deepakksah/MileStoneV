using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

public class GenericRecorderOnlyRuleActionFactory : RuleActionFactoryBase
{
	public GenericRecorderOnlyRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}

	public sealed override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		IRuleEngine ruleEngine = base.ruleActionDataProvider.RuleEngine;
		return new GenericRecorderOnlyRuleAction(ruleEngine);
	}
}
