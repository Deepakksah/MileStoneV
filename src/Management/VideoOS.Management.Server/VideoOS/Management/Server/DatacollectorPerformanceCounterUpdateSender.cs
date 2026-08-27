using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Management.Server;

public class DatacollectorPerformanceCounterUpdateSender : IPerformanceCounterUpdateSender
{
	public async Task<bool> PostCreatedItems(IGeminiRequestContext requestContext, PerformanceStateRuleConfig newConfig)
	{
		Dictionary<int, List<CounterStateRulesConfigurationUpdate>> existingDataCollectorItems = GetExistingDataCollectorItems(requestContext, newConfig, deleted: false);
		return await SendUpdatesToDatacollectors(requestContext, existingDataCollectorItems);
	}

	public async Task<bool> PostCreatedSources(IGeminiRequestContext requestContext, RuleSource newSource)
	{
		Dictionary<int, List<CounterStateRulesConfigurationUpdate>> updates = new Dictionary<int, List<CounterStateRulesConfigurationUpdate>>();
		AddRuleConfigToUpdates(newSource.CounterPath, newSource.DataCollectorId, deleted: false, newSource.CounterRuleId, (float)newSource.CriticalThreshold, (float)newSource.WarningThreshold, newSource.TimeWindow, newSource.Name, newSource.CategoryName, newSource.CounterName, ref updates);
		return await SendUpdatesToDatacollectors(requestContext, updates);
	}

	public async Task<bool> PostDeletedItems(IGeminiRequestContext requestContext, PerformanceStateRuleConfig oldConfig)
	{
		Dictionary<int, List<CounterStateRulesConfigurationUpdate>> existingDataCollectorItems = GetExistingDataCollectorItems(requestContext, oldConfig, deleted: true);
		return await SendUpdatesToDatacollectors(requestContext, existingDataCollectorItems);
	}

	public async Task<bool> PostDeletedSources(IGeminiRequestContext requestContext, RuleSource existingRuleSource)
	{
		Dictionary<int, List<CounterStateRulesConfigurationUpdate>> updates = new Dictionary<int, List<CounterStateRulesConfigurationUpdate>>();
		AddRuleConfigToUpdates(existingRuleSource.CounterPath, existingRuleSource.DataCollectorId, deleted: true, existingRuleSource.CounterRuleId, (float)existingRuleSource.CriticalThreshold, (float)existingRuleSource.WarningThreshold, existingRuleSource.TimeWindow, existingRuleSource.Name, existingRuleSource.CategoryName, existingRuleSource.CounterName, ref updates);
		return await SendUpdatesToDatacollectors(requestContext, updates);
	}

	public async Task<bool> PostUpdatedItems(IGeminiRequestContext requestContext, PerformanceStateRuleConfig newConfig, PerformanceStateRuleConfig oldConfig)
	{
		Dictionary<int, List<CounterStateRulesConfigurationUpdate>> updates = GetUpdates(requestContext, newConfig, oldConfig);
		return await SendUpdatesToDatacollectors(requestContext, updates);
	}

	public async Task<bool> PostUpdatedSources(IGeminiRequestContext requestContext, RuleSource newSource, RuleSource oldSource)
	{
		GetRuleSourceUpdates(newSource, oldSource, out var updates);
		return await SendUpdatesToDatacollectors(requestContext, updates);
	}

	public static void GetRuleSourceUpdates(RuleSource newSource, RuleSource oldSource, out Dictionary<int, List<CounterStateRulesConfigurationUpdate>> updates)
	{
		updates = new Dictionary<int, List<CounterStateRulesConfigurationUpdate>>();
		if (Math.Abs(newSource.CriticalThreshold - oldSource.CriticalThreshold) > 0.001 || Math.Abs(newSource.WarningThreshold - oldSource.WarningThreshold) > 0.001 || newSource.TimeWindow != oldSource.TimeWindow)
		{
			AddRuleConfigToUpdates(newSource.CounterPath, newSource.DataCollectorId, deleted: false, newSource.CounterRuleId, (float)newSource.CriticalThreshold, (float)newSource.WarningThreshold, newSource.TimeWindow, newSource.Name, newSource.CategoryName, newSource.CounterName, ref updates);
		}
	}

	public static Dictionary<int, List<CounterStateRulesConfigurationUpdate>> GetUpdates(IGeminiRequestContext requestContext, PerformanceStateRuleConfig newConfig, PerformanceStateRuleConfig oldConfig)
	{
		Dictionary<int, List<CounterStateRulesConfigurationUpdate>> updates = new Dictionary<int, List<CounterStateRulesConfigurationUpdate>>();
		List<DataCollectorParent> machines = DataCollectorService.GetMachines(requestContext);
		if (Math.Abs(newConfig.CriticalThreshold - oldConfig.CriticalThreshold) > 0.001 || Math.Abs(newConfig.WarningThreshold - oldConfig.WarningThreshold) > 0.001 || newConfig.TimeWindow != oldConfig.TimeWindow || newConfig.Disabled != oldConfig.Disabled)
		{
			foreach (DataCollectorParent item in machines)
			{
				AddRuleConfigToUpdates(null, item.DataCollectorId, newConfig.Disabled, newConfig.CounterRuleId, (float)newConfig.CriticalThreshold, (float)newConfig.WarningThreshold, newConfig.TimeWindow, newConfig.RuleName, newConfig.CategoryName, newConfig.CounterName, ref updates);
			}
		}
		if (newConfig.Disabled != oldConfig.Disabled)
		{
			foreach (RuleSource item2 in RuleSource.QueryRuleSourceByCounterRuleId(requestContext, oldConfig.CounterRuleId))
			{
				AddRuleConfigToUpdates(item2.CounterPath, item2.DataCollectorId, newConfig.Disabled, item2.CounterRuleId, (float)item2.CriticalThreshold, (float)item2.WarningThreshold, item2.TimeWindow, item2.Name, item2.CategoryName, item2.CounterName, ref updates);
			}
		}
		return updates;
	}

	public async Task<bool> SendUpdatesToDatacollectors(IGeminiRequestContext requestContext, Dictionary<int, List<CounterStateRulesConfigurationUpdate>> updates)
	{
		DataCollectorHost[] hosts = DataCollectorHost.GetDataCollectorHosts(requestContext, -1);
		foreach (KeyValuePair<int, List<CounterStateRulesConfigurationUpdate>> update in updates)
		{
			DataCollectorHost dataCollectorHost = hosts.FirstOrDefault((DataCollectorHost h) => h.DataCollectorId == update.Key);
			if (dataCollectorHost != null)
			{
				await SendUpdatesAsync(requestContext, dataCollectorHost.HostName, update.Value.ToArray());
			}
		}
		return true;
	}

	public static Dictionary<int, List<CounterStateRulesConfigurationUpdate>> GetExistingDataCollectorItems(IGeminiRequestContext requestContext, PerformanceStateRuleConfig config, bool deleted)
	{
		Dictionary<int, List<CounterStateRulesConfigurationUpdate>> updates = new Dictionary<int, List<CounterStateRulesConfigurationUpdate>>();
		foreach (DataCollectorParent machine in DataCollectorService.GetMachines(requestContext))
		{
			AddRuleConfigToUpdates(null, machine.DataCollectorId, deleted, config.CounterRuleId, (float)config.CriticalThreshold, (float)config.WarningThreshold, config.TimeWindow, config.RuleName, config.CategoryName, config.CounterName, ref updates);
		}
		return updates;
	}

	private static void AddRuleConfigToUpdates(string sourcePath, int dataCollectorId, bool deleted, string ruleId, float critical, float warning, int time, string name, string categoryName, string counterName, ref Dictionary<int, List<CounterStateRulesConfigurationUpdate>> updates)
	{
		List<CounterStateRulesConfigurationUpdate> value = new List<CounterStateRulesConfigurationUpdate>();
		if (updates.TryGetValue(dataCollectorId, out value))
		{
			if (value == null)
			{
				updates[dataCollectorId] = new List<CounterStateRulesConfigurationUpdate>();
			}
			updates[dataCollectorId].Add(new CounterStateRulesConfigurationUpdate
			{
				Deleted = deleted,
				RuleConfig = NewRuleConfig(sourcePath, ruleId, critical, warning, time, name, categoryName, counterName)
			});
		}
		else
		{
			updates.Add(dataCollectorId, new List<CounterStateRulesConfigurationUpdate>
			{
				new CounterStateRulesConfigurationUpdate
				{
					Deleted = deleted,
					RuleConfig = NewRuleConfig(sourcePath, ruleId, critical, warning, time, name, categoryName, counterName)
				}
			});
		}
	}

	private static async Task<bool> SendUpdatesAsync(IGeminiRequestContext requestContext, string hostName, CounterStateRulesConfigurationUpdate[] updates)
	{
		await System.Threading.Tasks.Task.Run(delegate
		{
			LiveDataCollector.PostUpdatedCounterStateRuleConfigurations(requestContext, hostName, updates.ToArray());
		});
		return true;
	}

	private static CounterStateRuleConfiguration NewRuleConfig(string path, string ruleId, float critical, float warning, int time, string name, string categoryName, string counterName)
	{
		return new CounterStateRuleConfiguration
		{
			CategoryName = categoryName,
			CounterName = counterName,
			CounterPath = path,
			CounterRuleId = ruleId,
			CriticalThreshold = critical,
			WarningThreshold = warning,
			TimeWindow = time,
			Name = name
		};
	}
}
