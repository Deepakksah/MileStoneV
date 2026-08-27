using System;
using System.Globalization;
using System.Linq;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.SetMonitorCameras", "D36D245D-F4D1-48fd-99F1-5D2733BF630F", ActionTargetType.Device, "DeviceIds", "DeviceGroupIds")]
public sealed class SetMonitorCamerasRuleActionFactory : RuleActionFactoryBase
{
	public SetMonitorCamerasRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}

	public override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		XmlNode actionParameterByName = RuleActionFactoryBase.GetActionParameterByName(actionNode, "VideoWallId");
		if (actionParameterByName == null)
		{
			throw new RuleConfigurationException("VideoWallId parameter for SetMonitorCameras action was not found");
		}
		Guid videoWallId = new Guid(actionParameterByName.InnerText.Trim());
		Guid monitorId = new Guid(RuleActionFactoryBase.GetActionParameterByName(actionNode, "MonitorId").InnerText.Trim());
		Guid layoutId = new Guid(RuleActionFactoryBase.GetActionParameterByName(actionNode, "LayoutId").InnerText.Trim());
		int viewPosition = Convert.ToInt32(RuleActionFactoryBase.GetActionParameterByName(actionNode, "Position").InnerText.Trim(), CultureInfo.InvariantCulture);
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
		return new SetMonitorCamerasRuleAction(ruleEngine, videoWallId, monitorId, layoutId, viewPosition, siteDeviceIds, delay);
	}
}
