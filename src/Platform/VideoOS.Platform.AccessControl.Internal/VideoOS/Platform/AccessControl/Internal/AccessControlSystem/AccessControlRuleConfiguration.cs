using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.AccessControl.Internal.Data.Rule;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlSystem;

public class AccessControlRuleConfiguration
{
	private List<AccessControlRule> _accessControlRules = new List<AccessControlRule>();

	public AccessControlRuleConfiguration()
	{
	}

	public AccessControlRuleConfiguration(string serializedXml)
	{
		_accessControlRules.Clear();
		RuleConfigurationSerializeData ruleConfigurationSerializeData = RuleConfigurationSerializeData.Deserialize(serializedXml);
		if (ruleConfigurationSerializeData == null)
		{
			return;
		}
		foreach (RuleConfigurationSerializeData.AccessControlRuleData accessControlRule in ruleConfigurationSerializeData.AccessControlRules)
		{
			_accessControlRules.Add(new AccessControlRule(accessControlRule));
		}
	}

	public RuleConfigurationSerializeData SerializeData()
	{
		lock (this)
		{
			RuleConfigurationSerializeData ruleConfigurationSerializeData = new RuleConfigurationSerializeData();
			foreach (AccessControlRule accessControlRule in _accessControlRules)
			{
				ruleConfigurationSerializeData.AccessControlRules.Add((RuleConfigurationSerializeData.AccessControlRuleData)accessControlRule.ToDataClass());
			}
			return ruleConfigurationSerializeData;
		}
	}

	public IEnumerable<AccessControlRule> GetAccessControlRules()
	{
		lock (this)
		{
			return _accessControlRules.ToList();
		}
	}

	public void SetAccessControlRules(IEnumerable<AccessControlRule> accessControlRules)
	{
		lock (this)
		{
			_accessControlRules = accessControlRules.ToList();
		}
	}
}
