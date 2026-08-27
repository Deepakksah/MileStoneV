using System.Collections;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Common.Integration.Command;

public class ConfigureRulesInfo
{
	public enum RuleAction
	{
		Add,
		Remove,
		Update
	}

	public class RuleConfigInfo
	{
		public RuleAction Action;

		public ConfigurationDataSet.RulesRow RuleRow;

		public RuleConfigInfo(RuleAction action, ConfigurationDataSet.RulesRow ruleRow)
		{
			Action = action;
			RuleRow = ruleRow;
		}
	}

	public bool HasAnyChanges
	{
		get
		{
			if (RuleChanges.Count <= 0 && EventTypesChanges == null && DeviceGroupChanges == null && DeviceGroupMemberChanges == null && ServerGroupChanges == null && ServerGroupMembershipChanges == null && TimeProfileChanges == null && RuleVariablesChanges == null)
			{
				return RecorderChanges != null;
			}
			return true;
		}
	}

	public ArrayList RuleChanges { get; private set; }

	public ConfigurationDataSet.EventTypesDataTable EventTypesChanges { get; set; }

	public ConfigurationDataSet.DeviceGroupsDataTable DeviceGroupChanges { get; set; }

	public ConfigurationDataSet.DeviceGroupMembersDataTable DeviceGroupMemberChanges { get; set; }

	public ConfigurationDataSet.ServerGroupDataTable ServerGroupChanges { get; set; }

	public ConfigurationDataSet.ServerGroupMembershipDataTable ServerGroupMembershipChanges { get; set; }

	public ConfigurationDataSet.TimeProfileDataTable TimeProfileChanges { get; set; }

	public ConfigurationDataSet.RuleVariablesDataTable RuleVariablesChanges { get; set; }

	public ConfigurationDataSet.RecordersDataTable RecorderChanges { get; set; }

	public ConfigureRulesInfo()
	{
		RuleChanges = new ArrayList();
	}

	public void AddRuleChange(RuleAction action, ConfigurationDataSet.RulesRow ruleRow)
	{
		RuleChanges.Add(new RuleConfigInfo(action, ruleRow));
	}
}
