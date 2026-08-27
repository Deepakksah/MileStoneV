using System;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.SetMonitorView", "60597686-9140-4c37-984A-1B266A3F0FF0", ActionTargetType.Unspecified)]
public sealed class SetMonitorViewRuleActionFactory : RuleActionFactoryBase
{
	public SetMonitorViewRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}

	public override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		XmlNode actionParameterByName = RuleActionFactoryBase.GetActionParameterByName(actionNode, "VideoWallId");
		if (actionParameterByName == null)
		{
			throw new RuleConfigurationException("VideoWallId parameter for SetMonitorView action was not found");
		}
		Guid videoWallId = new Guid(actionParameterByName.InnerText.Trim());
		Guid monitorId = new Guid(RuleActionFactoryBase.GetActionParameterByName(actionNode, "MonitorId").InnerText.Trim());
		Guid viewId = new Guid(RuleActionFactoryBase.GetActionParameterByName(actionNode, "ViewId").InnerText.Trim());
		TimeSpan delay = RuleActionFactoryBase.ParseDelay(actionNode);
		IRuleEngine ruleEngine = base.ruleActionDataProvider.RuleEngine;
		return new SetMonitorViewRuleAction(ruleEngine, videoWallId, monitorId, viewId, delay);
	}
}
