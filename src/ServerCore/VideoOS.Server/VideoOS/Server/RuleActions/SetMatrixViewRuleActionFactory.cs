using System;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.SetMatrixView", "9078AB88-C83E-41ee-B516-09E6F0702EF4", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class SetMatrixViewRuleActionFactory : RuleActionFactoryBase
{
	public SetMatrixViewRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}

	public override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		XmlNode actionParameterByName = RuleActionFactoryBase.GetActionParameterByName(actionNode, "MatrixId");
		if (actionParameterByName == null)
		{
			throw new RuleConfigurationException("MatrixId parameter for SetMatrixView action was not found");
		}
		TimeSpan delay = RuleActionFactoryBase.ParseDelay(actionNode);
		Server server = ((ISecretServerRuleActionDataProvider)base.ruleActionDataProvider).Server;
		IRuleEngine ruleEngine = base.ruleActionDataProvider.RuleEngine;
		return new SetMatrixViewRuleAction(ruleEngine, server, new Guid(actionParameterByName.InnerText.Trim()), delay);
	}
}
