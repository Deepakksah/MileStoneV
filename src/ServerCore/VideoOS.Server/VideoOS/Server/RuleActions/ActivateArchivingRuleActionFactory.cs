using System;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

[RuleActionFactory("VideoOS.ActivateArchiving", "15C687B2-BB44-4F09-8FC4-3C564EC9AAEC", ActionTargetType.Unspecified)]
public sealed class ActivateArchivingRuleActionFactory : RuleActionFactoryBase
{
	public ActivateArchivingRuleActionFactory(IRuleActionDataProvider ruleActionDataProvider)
		: base(ruleActionDataProvider)
	{
	}

	public override IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode)
	{
		IRuleEngine ruleEngine = base.ruleActionDataProvider.RuleEngine;
		Guid[] archiveStorageIds = RuleActionFactoryBase.BuildGuidArray(RuleActionFactoryBase.GetActionParameterByName(actionNode, "ArchiveIds"));
		return new ActivateArchivingRuleAction(ruleEngine, archiveStorageIds);
	}
}
