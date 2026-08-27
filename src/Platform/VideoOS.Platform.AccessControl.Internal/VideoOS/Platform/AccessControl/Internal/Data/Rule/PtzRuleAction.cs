using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public class PtzRuleAction : RuleActionBase
{
	public class PtzEntry
	{
		public FQID PtzId { get; set; }

		public TimeSpan Delay { get; set; }
	}

	public IEnumerable<PtzEntry> PtzEntries { get; set; }

	public PtzRuleAction(RuleConfigurationSerializeData.PtzRuleActionData ptzRuleActionData)
	{
		if (ptzRuleActionData.PtzEntries == null || !ptzRuleActionData.PtzEntries.Any())
		{
			return;
		}
		List<PtzEntry> list = new List<PtzEntry>();
		foreach (RuleConfigurationSerializeData.PtzRuleActionData.PtzEntryData ptzEntry in ptzRuleActionData.PtzEntries)
		{
			list.Add(new PtzEntry
			{
				PtzId = ptzEntry.PtzId,
				Delay = new TimeSpan(ptzEntry.DelayTicks)
			});
		}
		PtzEntries = list;
	}

	public PtzRuleAction()
	{
	}

	public override object ToDataClass()
	{
		List<RuleConfigurationSerializeData.PtzRuleActionData.PtzEntryData> list = new List<RuleConfigurationSerializeData.PtzRuleActionData.PtzEntryData>();
		if (PtzEntries != null)
		{
			foreach (PtzEntry ptzEntry in PtzEntries)
			{
				list.Add(new RuleConfigurationSerializeData.PtzRuleActionData.PtzEntryData
				{
					PtzId = ptzEntry.PtzId,
					DelayTicks = ptzEntry.Delay.Ticks
				});
			}
		}
		return new RuleConfigurationSerializeData.PtzRuleActionData
		{
			PtzEntries = list
		};
	}
}
