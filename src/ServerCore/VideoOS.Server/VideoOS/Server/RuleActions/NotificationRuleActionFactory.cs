using System;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.CreateNotification", "58E350F3-4827-4255-9348-A8C5C0F0F6D5", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class NotificationRuleActionFactory : RuleActionFactoryBase
{
	public NotificationRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}

	public override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		if (subRule == null)
		{
			throw new ArgumentNullException("subRule");
		}
		XmlNode actionParameterByName = RuleActionFactoryBase.GetActionParameterByName(actionNode, "ProfileId");
		if (actionParameterByName == null)
		{
			throw new RuleConfigurationException("ProfileId parameter for CreateNotification action was not found");
		}
		Guid[] array = RuleActionFactoryBase.BuildGuidArray(RuleActionFactoryBase.GetActionParameterByName(actionNode, "DeviceIds"));
		Guid[] array2 = RuleActionFactoryBase.BuildGuidArray(RuleActionFactoryBase.GetActionParameterByName(actionNode, "DeviceGroupIds"));
		if (array.Length == 0 && array2.Length == 0)
		{
			array = new Guid[1];
		}
		Server server = ((ISecretServerRuleActionDataProvider)base.ruleActionDataProvider).Server;
		IRuleEngine ruleEngine = base.ruleActionDataProvider.RuleEngine;
		return new NotificationRuleAction(ruleEngine, server, new Guid(actionParameterByName.InnerText.Trim()), subRule.ParentRule.ID, array, array2);
	}
}
