using System;
using System.Linq;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.RemoveMonitorCameras", "7FEE9BB0-50D4-448F-8FFA-D035BE42A8D7", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class RemoveMonitorCamerasRuleActionFactory : RuleActionFactoryBase
{
	public RemoveMonitorCamerasRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}

	public override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		XmlNode actionParameterByName = RuleActionFactoryBase.GetActionParameterByName(actionNode, "VideoWallId");
		if (actionParameterByName == null)
		{
			throw new RuleConfigurationException("VideoWallId parameter for RemoveMonitorCameras action was not found");
		}
		Guid videoWallId = new Guid(actionParameterByName.InnerText.Trim());
		Guid monitorId = new Guid(RuleActionFactoryBase.GetActionParameterByName(actionNode, "MonitorId").InnerText.Trim());
		XmlNode actionParameterByName2 = RuleActionFactoryBase.GetActionParameterByName(actionNode, "SiteDeviceIds");
		Guid[] siteDeviceIds = new Guid[0];
		if (actionParameterByName2 != null && actionParameterByName2.InnerText.Length > 0)
		{
			string[] source = actionParameterByName2.InnerText.Split(',', ':');
			siteDeviceIds = (from s in source.Where((string x, int i) => i % 2 == 1)
				select Guid.Parse(s)).ToArray();
		}
		TimeSpan delay = RuleActionFactoryBase.ParseDelay(actionNode);
		IRuleEngine ruleEngine = base.ruleActionDataProvider.RuleEngine;
		return new RemoveMonitorCamerasRuleAction(ruleEngine, videoWallId, monitorId, siteDeviceIds, delay);
	}
}
