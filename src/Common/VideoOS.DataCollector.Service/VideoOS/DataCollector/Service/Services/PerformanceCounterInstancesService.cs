using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.Services;

public class PerformanceCounterInstancesService : IPerformanceCounterInstancesService
{
	private readonly ILogger<PerformanceCounterInstancesService> _logger;

	public PerformanceCounterInstancesService(ILogger<PerformanceCounterInstancesService> logger)
	{
		_logger = logger;
	}

	public Dictionary<string, PerformanceCounterInstance> GetConfiguredPerformanceCounterInstances(PerformanceCounterCategory category, PerformanceCounterConfiguration counterConfiguration)
	{
		return GetConfiguredPerformanceCounterInstancesInnerMethod(category.CategoryType, category.GetInstanceNames(), counterConfiguration);
	}

	public Dictionary<string, PerformanceCounterInstance> GetConfiguredPerformanceCounterInstancesInnerMethod(PerformanceCounterCategoryType categoryType, string[] instanceNames, PerformanceCounterConfiguration counterConfiguration)
	{
		Dictionary<string, PerformanceCounterInstance> dictionary = new Dictionary<string, PerformanceCounterInstance>(StringComparer.OrdinalIgnoreCase);
		if (categoryType == PerformanceCounterCategoryType.SingleInstance || !string.IsNullOrEmpty(counterConfiguration.CounterInstance.InstanceName))
		{
			PerformanceCounterInstance performanceCounterInstance = new PerformanceCounterInstance(counterConfiguration.CounterInstance.InstanceName, counterConfiguration.CounterInstance.CounterName, counterConfiguration.CounterInstance.CategoryName)
			{
				CategoryType = categoryType,
				SampleFrequency = counterConfiguration.SampleFrequency,
				CounterDirection = counterConfiguration.CounterInstance.CounterDirection,
				EnableHistory = counterConfiguration.CounterInstance.EnableHistory,
				TriggerStatePerValue = counterConfiguration.CounterInstance.TriggerStatePerValue
			};
			dictionary.Add(performanceCounterInstance.Path, performanceCounterInstance);
		}
		else if (categoryType == PerformanceCounterCategoryType.MultiInstance)
		{
			foreach (string instanceName in instanceNames)
			{
				PerformanceCounterInstance performanceCounterInstance2 = new PerformanceCounterInstance(instanceName, counterConfiguration.CounterInstance.CounterName, counterConfiguration.CounterInstance.CategoryName)
				{
					CategoryType = categoryType,
					SampleFrequency = counterConfiguration.SampleFrequency,
					CounterDirection = counterConfiguration.CounterInstance.CounterDirection,
					EnableHistory = counterConfiguration.CounterInstance.EnableHistory,
					TriggerStatePerValue = counterConfiguration.CounterInstance.TriggerStatePerValue
				};
				if (!dictionary.ContainsKey(performanceCounterInstance2.Path))
				{
					dictionary.Add(performanceCounterInstance2.Path, performanceCounterInstance2);
					continue;
				}
				_logger.LogWarning($"Performance counter instance with same path. Skipping the instance. Instance category: {performanceCounterInstance2.CategoryName}, counter name: {performanceCounterInstance2.CounterName} Instance name {performanceCounterInstance2.InstanceName}");
			}
		}
		return dictionary;
	}

	public string[] GetCategoryNames()
	{
		return (from c in PerformanceCounterCategory.GetCategories()
			select c.CategoryName).ToArray();
	}

	public PerformanceCounterInstance[] GetPerformanceCounterInstances(string categoryName)
	{
		List<PerformanceCounterInstance> list = new List<PerformanceCounterInstance>();
		PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory(categoryName);
		switch (performanceCounterCategory.CategoryType)
		{
		case PerformanceCounterCategoryType.MultiInstance:
		{
			string[] instanceNames = performanceCounterCategory.GetInstanceNames();
			foreach (string instanceName in instanceNames)
			{
				PerformanceCounter[] counters2 = performanceCounterCategory.GetCounters(instanceName);
				foreach (PerformanceCounter performanceCounter2 in counters2)
				{
					using (performanceCounter2)
					{
						PerformanceCounterInstance item2 = new PerformanceCounterInstance(instanceName, performanceCounter2.CounterName, performanceCounterCategory.CategoryName)
						{
							CounterType = performanceCounter2.CounterType
						};
						list.Add(item2);
					}
				}
			}
			break;
		}
		case PerformanceCounterCategoryType.SingleInstance:
		{
			PerformanceCounter[] counters = performanceCounterCategory.GetCounters();
			foreach (PerformanceCounter performanceCounter in counters)
			{
				using (performanceCounter)
				{
					PerformanceCounterInstance item = new PerformanceCounterInstance(performanceCounter.InstanceName, performanceCounter.CounterName, performanceCounterCategory.CategoryName)
					{
						CounterType = performanceCounter.CounterType
					};
					list.Add(item);
				}
			}
			break;
		}
		}
		return list.ToArray();
	}
}
