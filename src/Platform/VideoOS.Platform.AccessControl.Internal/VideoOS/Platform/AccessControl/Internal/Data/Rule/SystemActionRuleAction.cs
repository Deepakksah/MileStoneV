using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public class SystemActionRuleAction : RuleActionBase
{
	public IEnumerable<FQID> SystemActionIds { get; set; }

	public SystemActionRuleAction(RuleConfigurationSerializeData.SystemActionRuleActionData externalEventRuleActionData)
	{
		SystemActionIds = externalEventRuleActionData.SystemActionIds;
	}

	public SystemActionRuleAction()
	{
	}

	public override object ToDataClass()
	{
		return new RuleConfigurationSerializeData.SystemActionRuleActionData
		{
			SystemActionIds = ((SystemActionIds != null) ? SystemActionIds.ToList() : new List<FQID>())
		};
	}
}
