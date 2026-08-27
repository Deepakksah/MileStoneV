using System;
using System.Data;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.Adapters;

public class DiffGramConfigurationAdapter : ConfigurationAdapter, IRuleConfigurationProcessing, IRuleConfiguration
{
	private readonly IResponseSink _responseSink;

	private readonly ICallContextInfo _callContextInfo;

	public DiffGramConfigurationAdapter(ConfigurationDataSet configurationDataSetDiff, IResponseSink responseSink, ICallContextInfo callContextInfo)
		: base(UpdateConfigurationFromDiffGram(configurationDataSetDiff), null)
	{
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		if (callContextInfo == null)
		{
			throw new ArgumentNullException("callContextInfo");
		}
		_responseSink = responseSink;
		_callContextInfo = callContextInfo;
	}

	private static ConfigureRulesInfo UpdateConfigurationFromDiffGram(ConfigurationDataSet configurationDataSetDiff)
	{
		if (configurationDataSetDiff == null)
		{
			throw new ArgumentNullException("configurationDataSetDiff");
		}
		ConfigureRulesInfo configureRulesInfo = new ConfigureRulesInfo();
		configureRulesInfo.EventTypesChanges = (ConfigurationDataSet.EventTypesDataTable)configurationDataSetDiff.EventTypes.GetChanges();
		configureRulesInfo.DeviceGroupChanges = (ConfigurationDataSet.DeviceGroupsDataTable)configurationDataSetDiff.DeviceGroups.GetChanges();
		configureRulesInfo.DeviceGroupMemberChanges = (ConfigurationDataSet.DeviceGroupMembersDataTable)configurationDataSetDiff.DeviceGroupMembers.GetChanges();
		configureRulesInfo.ServerGroupChanges = (ConfigurationDataSet.ServerGroupDataTable)configurationDataSetDiff.ServerGroup.GetChanges();
		configureRulesInfo.ServerGroupMembershipChanges = (ConfigurationDataSet.ServerGroupMembershipDataTable)configurationDataSetDiff.ServerGroupMembership.GetChanges();
		configureRulesInfo.TimeProfileChanges = (ConfigurationDataSet.TimeProfileDataTable)configurationDataSetDiff.TimeProfile.GetChanges();
		configureRulesInfo.RuleVariablesChanges = (ConfigurationDataSet.RuleVariablesDataTable)configurationDataSetDiff.RuleVariables.GetChanges();
		configureRulesInfo.RecorderChanges = (ConfigurationDataSet.RecordersDataTable)configurationDataSetDiff.Recorders.GetChanges();
		ConfigurationDataSet.RulesDataTable rulesDataTable = (ConfigurationDataSet.RulesDataTable)configurationDataSetDiff.Rules.GetChanges();
		if (rulesDataTable != null)
		{
			foreach (ConfigurationDataSet.RulesRow row in rulesDataTable.Rows)
			{
				if ((row.RowState & DataRowState.Added) == DataRowState.Added)
				{
					configureRulesInfo.AddRuleChange(ConfigureRulesInfo.RuleAction.Add, row);
				}
				else if ((row.RowState & DataRowState.Modified) == DataRowState.Modified)
				{
					configureRulesInfo.AddRuleChange(ConfigureRulesInfo.RuleAction.Update, row);
				}
				else if ((row.RowState & DataRowState.Deleted) == DataRowState.Deleted)
				{
					row.RejectChanges();
					configureRulesInfo.AddRuleChange(ConfigureRulesInfo.RuleAction.Remove, row);
				}
			}
		}
		return configureRulesInfo;
	}

	public void ProcessConfiguration(IRuleConfiguration ruleConfiguration)
	{
		ConfigureRulesCommand command = new ConfigureRulesCommand(ruleConfiguration);
		_responseSink.ProcessAsync(command, _callContextInfo);
	}
}
