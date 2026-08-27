using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Management.Server;

public class DataCollector
{
	public static int RegisterDataCollector(IGeminiRequestContext requestContext, string applicationId, string hostName, int version)
	{
		using IDataCollectionComponent dataCollectionComponent = requestContext.CreateDataCollectionComponent();
		return dataCollectionComponent.RegisterDataCollector(applicationId, hostName, version);
	}

	public static int RegisterDataCollectorNewApplicationId(IGeminiRequestContext requestContext, string applicationId, string oldApplicationId, string hostName, int version)
	{
		using IDataCollectionComponent dataCollectionComponent = requestContext.CreateDataCollectionComponent();
		return dataCollectionComponent.RegisterDataCollectorNewApplicationId(applicationId, oldApplicationId, hostName, version);
	}

	public static DataCollectorConfiguration GetDataCollectorConfiguration(IGeminiRequestContext requestContext, int dataCollectorId)
	{
		using IDataCollectionComponent dataCollectionComponent = requestContext.CreateDataCollectionComponent();
		return dataCollectionComponent.GetDataCollectorConfiguration(dataCollectorId);
	}

	public static void AddPerformanceCounterData(IGeminiRequestContext requestContext, PerformanceCounterData counterData)
	{
		using IDataCollectionComponent dataCollectionComponent = requestContext.CreateDataCollectionComponent();
		dataCollectionComponent.AddPerformanceCounterData(counterData);
	}

	public static void AddPerformanceCounterDataWithConfigRefresh(IGeminiRequestContext requestContext, PerformanceCounterData counterData)
	{
		PerformanceStateRuleConfig[] array = PerformanceStateRuleConfig.QueryPerformanceStateRuleConfigByIds(requestContext, counterData.DataCollectorId, null);
		AddPerformanceCounterData(requestContext, counterData);
		PerformanceStateRuleConfig[] source = PerformanceStateRuleConfig.QueryPerformanceStateRuleConfigByIds(requestContext, counterData.DataCollectorId, null);
		PerformanceStateRuleConfig[] array2 = array;
		foreach (PerformanceStateRuleConfig oldConfig in array2)
		{
			PerformanceStateRuleConfig performanceStateRuleConfig = source.SingleOrDefault((PerformanceStateRuleConfig performanceStateRuleConfig2) => performanceStateRuleConfig2.CounterRuleId.Equals(oldConfig.CounterRuleId, StringComparison.OrdinalIgnoreCase));
			if (performanceStateRuleConfig != null)
			{
				SystemMonitorManager.RegisterUpdatedItems(requestContext, oldConfig, performanceStateRuleConfig);
			}
		}
	}

	public static void MarkPerformanceCounterDataAsObsolete(IGeminiRequestContext requestContext, string[] instances)
	{
		using IDataCollectionComponent dataCollectionComponent = requestContext.CreateDataCollectionComponent();
		dataCollectionComponent.MarkPerformanceCounterDataAsObsolete(instances);
	}

	public static ServiceApplication[] GetServiceApplicationList(IGeminiRequestContext requestContext, int dataCollectorId)
	{
		using IDataCollectionComponent dataCollectionComponent = requestContext.CreateDataCollectionComponent();
		return dataCollectionComponent.GetServiceApplicationList(dataCollectorId);
	}

	public static void RegisterServiceApplications(IGeminiRequestContext requestContext, int dataCollectorId, ServiceApplication[] serviceApplications)
	{
		using IDataCollectionComponent dataCollectionComponent = requestContext.CreateDataCollectionComponent();
		dataCollectionComponent.RegisterServiceApplications(dataCollectorId, serviceApplications);
	}

	public static void UpdateMachineProperties(IGeminiRequestContext requestContext, int dataCollectorId, MachineProperty[] machineProperties)
	{
		using IDataCollectionComponent dataCollectionComponent = requestContext.CreateDataCollectionComponent();
		dataCollectionComponent.UpdateMachineProperties(dataCollectorId, machineProperties);
	}

	public static HistoricSample[] QueryHistoricSamples(GeminiRequestContext geminiRequestContext, HistoricSampleRequestData data)
	{
		using DataCollectionComponent dataCollectionComponent = geminiRequestContext.CreateComponent<DataCollectionComponent>();
		return dataCollectionComponent.QueryHistoricSamples(data);
	}

	public static CounterStateRulesConfiguration GetCountersStateRulesConfiguration(GeminiRequestContext geminiRequestContext, int dataCollector)
	{
		PerformanceStateRuleConfig[] perfConfig = PerformanceStateRuleConfig.QueryDatacollectorPerformanceStateRuleConfigByIds(geminiRequestContext, dataCollector, null);
		RuleSource[] source = RuleSource.QueryRuleSourceByIds(geminiRequestContext, dataCollector, null, generateSources: false);
		return ConvertToCounterStateRulesConfiguration(perfConfig, source.ToList());
	}

	public static void RegisterStateChangeData(GeminiRequestContext geminiRequestContext, CounterStateChangeData data)
	{
		PerformanceStateRuleConfig.RegisterStateChange(geminiRequestContext, data);
	}

	public static void DeleteStateData(GeminiRequestContext geminiRequestContext, CounterStateChangeData data)
	{
		PerformanceStateRuleConfig.DeleteState(geminiRequestContext, data);
	}

	public static void DeleteStateDataFromDatacollector(GeminiRequestContext geminiRequestContext, int datacollectorId, string[] activePaths)
	{
		PerformanceStateRuleConfig.DeleteStateDataFromDatacollector(geminiRequestContext, datacollectorId, activePaths);
	}

	private static CounterStateRulesConfiguration ConvertToCounterStateRulesConfiguration(PerformanceStateRuleConfig[] perfConfig, List<RuleSource> ruleSources)
	{
		CounterStateRulesConfiguration counterStateRulesConfiguration = new CounterStateRulesConfiguration();
		counterStateRulesConfiguration.CounterStateRules = new CounterStateRuleConfiguration[perfConfig.Length];
		List<CounterStateRuleConfiguration> list = new List<CounterStateRuleConfiguration>();
		int i;
		for (i = 0; i < perfConfig.Length; i++)
		{
			CounterStateRuleConfiguration counterStateRuleConfiguration = new CounterStateRuleConfiguration();
			if (perfConfig[i].Disabled)
			{
				continue;
			}
			foreach (RuleSource item in ruleSources.Where((RuleSource s) => s.CounterRuleId.Equals(perfConfig[i].CounterRuleId, StringComparison.OrdinalIgnoreCase)))
			{
				counterStateRuleConfiguration = (item.HasOverride ? new CounterStateRuleConfiguration
				{
					CounterPath = item.CounterPath,
					CategoryName = item.CategoryName,
					CounterName = item.CounterName,
					CounterRuleId = perfConfig[i].CounterRuleId,
					CriticalThreshold = (float)item.CriticalThreshold,
					WarningThreshold = (float)item.WarningThreshold,
					TimeWindow = item.TimeWindow,
					Name = perfConfig[i].RuleName
				} : new CounterStateRuleConfiguration
				{
					CounterPath = item.CounterPath,
					CategoryName = item.CategoryName,
					CounterName = item.CounterName,
					CounterRuleId = perfConfig[i].CounterRuleId,
					CriticalThreshold = (float)perfConfig[i].CriticalThreshold,
					WarningThreshold = (float)perfConfig[i].WarningThreshold,
					TimeWindow = perfConfig[i].TimeWindow,
					Name = perfConfig[i].RuleName
				});
				list.Add(counterStateRuleConfiguration);
			}
		}
		counterStateRulesConfiguration.CounterStateRules = list.ToArray();
		return counterStateRulesConfiguration;
	}
}
