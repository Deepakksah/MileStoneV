using System;
using System.Globalization;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.SetBookmark", "7CA4F91E-5F3F-4a52-8FF0-4A60A391B416", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class SetBookmarkRuleActionFactory : RuleActionFactoryBase
{
	public SetBookmarkRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}

	public override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		if (subRule == null)
		{
			throw new ArgumentNullException("subRule");
		}
		XmlNode actionParameterByName = RuleActionFactoryBase.GetActionParameterByName(actionNode, "Header");
		if (actionParameterByName == null)
		{
			throw new RuleConfigurationException("Header parameter for SetBookmark action was not found");
		}
		XmlNode actionParameterByName2 = RuleActionFactoryBase.GetActionParameterByName(actionNode, "Description");
		XmlNode actionParameterByName3 = RuleActionFactoryBase.GetActionParameterByName(actionNode, "TimeBefore");
		XmlNode actionParameterByName4 = RuleActionFactoryBase.GetActionParameterByName(actionNode, "TimeAfter");
		TimeSpan delay = RuleActionFactoryBase.ParseDelay(actionNode);
		Server server = ((ISecretServerRuleActionDataProvider)base.ruleActionDataProvider).Server;
		IRuleEngine ruleEngine = base.ruleActionDataProvider.RuleEngine;
		return new SetBookmarkRuleAction(ruleEngine, server, subRule.ParentRule.ID, actionParameterByName.InnerText, actionParameterByName2.InnerText, TimeSpan.FromSeconds(Convert.ToInt32(actionParameterByName3.InnerText, CultureInfo.InvariantCulture)), TimeSpan.FromSeconds(Convert.ToInt32(actionParameterByName4.InnerText, CultureInfo.InvariantCulture)), delay);
	}
}
