using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.CreateSNMPTrap", "FC317806-7975-4d5a-94C3-CAD4D7F0D7A8", ActionTargetType.Unspecified)]
public sealed class SendSNMPTrapRuleActionFactory : RuleActionFactoryBase
{
	public SendSNMPTrapRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}

	public override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		Server server = ((ISecretServerRuleActionDataProvider)base.ruleActionDataProvider).Server;
		IRuleEngine ruleEngine = base.ruleActionDataProvider.RuleEngine;
		return new SendSNMPTrapRuleAction(ruleEngine, server);
	}
}
