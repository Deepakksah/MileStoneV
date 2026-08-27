using System;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.SetVideoWallPreset", "B4B7FE57-3161-4bc8-B994-94EBF6AFF7AE", ActionTargetType.Unspecified)]
public sealed class SetVideoWallPresetRuleActionFactory : RuleActionFactoryBase
{
	public SetVideoWallPresetRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}

	public override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		XmlNode actionParameterByName = RuleActionFactoryBase.GetActionParameterByName(actionNode, "VideoWallId");
		if (actionParameterByName == null)
		{
			throw new RuleConfigurationException("VideoWallId parameter for SetVideoWallPreset action was not found");
		}
		Guid videoWallId = new Guid(actionParameterByName.InnerText.Trim());
		Guid presetId = new Guid(RuleActionFactoryBase.GetActionParameterByName(actionNode, "PresetId").InnerText.Trim());
		TimeSpan delay = RuleActionFactoryBase.ParseDelay(actionNode);
		IRuleEngine ruleEngine = base.ruleActionDataProvider.RuleEngine;
		return new SetVideoWallPresetRuleAction(ruleEngine, videoWallId, presetId, delay);
	}
}
