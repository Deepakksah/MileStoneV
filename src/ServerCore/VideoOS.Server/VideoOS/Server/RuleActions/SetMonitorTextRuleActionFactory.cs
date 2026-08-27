using System;
using System.Globalization;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.SetMonitorText", "4750B15E-5670-49F4-810F-536C0DC9A06C", ActionTargetType.Unspecified)]
public sealed class SetMonitorTextRuleActionFactory : RuleActionFactoryBase
{
	private const string ActionName = "VideoOS.SetMonitorText";

	public SetMonitorTextRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}

	public override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		XmlNode actionParameterByName = RuleActionFactoryBase.GetActionParameterByName(actionNode, "VideoWallId");
		if (actionParameterByName == null)
		{
			throw new RuleConfigurationException(string.Format(CultureInfo.InvariantCulture, "VideoWallId parameter for {0} action was not found.", "VideoOS.SetMonitorText"));
		}
		Guid videoWallId = new Guid(actionParameterByName.InnerText.Trim());
		Guid monitorId = new Guid(RuleActionFactoryBase.GetActionParameterByName(actionNode, "MonitorId").InnerText.Trim());
		Guid layoutId = new Guid(RuleActionFactoryBase.GetActionParameterByName(actionNode, "LayoutId").InnerText.Trim());
		int viewPosition = Convert.ToInt32(RuleActionFactoryBase.GetActionParameterByName(actionNode, "Position").InnerText.Trim(), CultureInfo.InvariantCulture);
		string textToDisplay = RuleActionFactoryBase.GetActionParameterByName(actionNode, "Text").InnerText.Trim();
		TimeSpan delay = RuleActionFactoryBase.ParseDelay(actionNode);
		IRuleEngine ruleEngine = base.ruleActionDataProvider.RuleEngine;
		return new SetMonitorTextRuleAction(ruleEngine, videoWallId, monitorId, layoutId, viewPosition, textToDisplay, delay);
	}
}
