using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public class AccessControlRule : RuleElementBase
{
	public static readonly Guid DefaultAccessRequestRuleId = new Guid("3ED8A2DB-EB3E-4E44-A013-4854535D8854");

	public Guid Id { get; private set; }

	public string Name { get; private set; }

	public bool Enabled { get; private set; }

	public bool ReadOnly { get; private set; }

	public IEnumerable<RuleEventBase> RuleEvents { get; private set; }

	public IEnumerable<RuleSourceBase> RuleSources { get; private set; }

	public IEnumerable<RuleTimeProfileBase> RuleTimeProfiles { get; private set; }

	public IEnumerable<RuleActionBase> RuleActions { get; private set; }

	public AccessControlRule(RuleConfigurationSerializeData.AccessControlRuleData accessControlRuleData)
	{
		List<RuleEventBase> list = new List<RuleEventBase>();
		foreach (RuleConfigurationSerializeData.RuleEventBaseData ruleEventBaseData in accessControlRuleData.RuleEventBaseDatas)
		{
			RuleEventBase ruleEventBase = RuleEventBase.Create(ruleEventBaseData);
			if (ruleEventBase != null)
			{
				list.Add(ruleEventBase);
			}
		}
		List<RuleSourceBase> list2 = new List<RuleSourceBase>();
		foreach (RuleConfigurationSerializeData.RuleSourceBaseData ruleSourceBaseData in accessControlRuleData.RuleSourceBaseDatas)
		{
			RuleSourceBase ruleSourceBase = RuleSourceBase.Create(ruleSourceBaseData);
			if (ruleSourceBase != null)
			{
				list2.Add(ruleSourceBase);
			}
		}
		List<RuleTimeProfileBase> list3 = new List<RuleTimeProfileBase>();
		foreach (RuleConfigurationSerializeData.RuleTimeProfileBaseData ruleTimeProfileData in accessControlRuleData.RuleTimeProfileDatas)
		{
			RuleTimeProfileBase ruleTimeProfileBase = RuleTimeProfileBase.Create(ruleTimeProfileData);
			if (ruleTimeProfileBase != null)
			{
				list3.Add(ruleTimeProfileBase);
			}
		}
		List<RuleActionBase> list4 = new List<RuleActionBase>();
		foreach (RuleConfigurationSerializeData.RuleActionBaseData ruleActionBaseData in accessControlRuleData.RuleActionBaseDatas)
		{
			RuleActionBase ruleActionBase = RuleActionBase.Create(ruleActionBaseData);
			if (ruleActionBase != null)
			{
				list4.Add(ruleActionBase);
			}
		}
		Id = accessControlRuleData.Id;
		Name = accessControlRuleData.Name;
		Enabled = accessControlRuleData.Enabled;
		ReadOnly = accessControlRuleData.ReadOnly;
		RuleEvents = list;
		RuleSources = list2;
		RuleTimeProfiles = list3;
		RuleActions = list4;
	}

	public AccessControlRule(Guid id, bool enabled, IEnumerable<RuleEventBase> ruleEvents, IEnumerable<RuleSourceBase> ruleSources, IEnumerable<RuleTimeProfileBase> ruleTimeProfiles, IEnumerable<RuleActionBase> ruleActions)
	{
		Id = id;
		Name = string.Empty;
		Enabled = enabled;
		ReadOnly = false;
		RuleEvents = ((ruleEvents != null) ? ruleEvents.ToList() : new List<RuleEventBase>());
		RuleSources = ((ruleSources != null) ? ruleSources.ToList() : new List<RuleSourceBase>());
		RuleTimeProfiles = ((ruleTimeProfiles != null) ? ruleTimeProfiles.ToList() : new List<RuleTimeProfileBase>());
		RuleActions = ((ruleActions != null) ? ruleActions.ToList() : new List<RuleActionBase>());
	}

	public AccessControlRule(Guid id, string name, bool enabled, bool readOnly, IEnumerable<RuleEventBase> ruleEvents, IEnumerable<RuleSourceBase> ruleSources, IEnumerable<RuleTimeProfileBase> ruleTimeProfiles, IEnumerable<RuleActionBase> ruleActions)
	{
		Id = id;
		Name = name;
		Enabled = enabled;
		ReadOnly = readOnly;
		RuleEvents = ((ruleEvents != null) ? ruleEvents.ToList() : new List<RuleEventBase>());
		RuleSources = ((ruleSources != null) ? ruleSources.ToList() : new List<RuleSourceBase>());
		RuleTimeProfiles = ((ruleTimeProfiles != null) ? ruleTimeProfiles.ToList() : new List<RuleTimeProfileBase>());
		RuleActions = ((ruleActions != null) ? ruleActions.ToList() : new List<RuleActionBase>());
	}

	public override object ToDataClass()
	{
		RuleConfigurationSerializeData.AccessControlRuleData accessControlRuleData = new RuleConfigurationSerializeData.AccessControlRuleData();
		accessControlRuleData.Id = Id;
		accessControlRuleData.Name = Name;
		accessControlRuleData.Enabled = Enabled;
		accessControlRuleData.ReadOnly = ReadOnly;
		foreach (RuleEventBase ruleEvent in RuleEvents)
		{
			accessControlRuleData.RuleEventBaseDatas.Add((RuleConfigurationSerializeData.RuleEventBaseData)ruleEvent.ToDataClass());
		}
		foreach (RuleSourceBase ruleSource in RuleSources)
		{
			accessControlRuleData.RuleSourceBaseDatas.Add((RuleConfigurationSerializeData.RuleSourceBaseData)ruleSource.ToDataClass());
		}
		foreach (RuleTimeProfileBase ruleTimeProfile in RuleTimeProfiles)
		{
			accessControlRuleData.RuleTimeProfileDatas.Add((RuleConfigurationSerializeData.RuleTimeProfileBaseData)ruleTimeProfile.ToDataClass());
		}
		foreach (RuleActionBase ruleAction in RuleActions)
		{
			accessControlRuleData.RuleActionBaseDatas.Add((RuleConfigurationSerializeData.RuleActionBaseData)ruleAction.ToDataClass());
		}
		return accessControlRuleData;
	}
}
