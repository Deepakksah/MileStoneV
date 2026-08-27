using System;
using System.Collections.Generic;
using System.Diagnostics;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Sample.Services;

namespace VideoOS.DataCollector.Service.Entities;

public class PerformanceCounterCollector : PerformanceCounterInstance
{
	public DateTime LastSampleTime { get; set; }

	public DateTime LastLiveRequest { get; set; }

	public bool Active { get; set; }

	public bool InitValuesSent { get; set; }

	public PerformanceCounterCollector(PerformanceCounterInstance instance)
		: base(instance.InstanceName, instance.CounterName, instance.CategoryName)
	{
		base.CounterType = instance.CounterType;
		base.CategoryType = instance.CategoryType;
		base.CounterDirection = instance.CounterDirection;
		base.EnableHistory = instance.EnableHistory;
		base.TriggerStatePerValue = instance.TriggerStatePerValue;
		base.SampleFrequency = instance.SampleFrequency;
		LastSampleTime = DateTime.MinValue;
		Active = true;
	}

	public bool ShouldSample(DateTime currentTime)
	{
		return LastSampleTime.AddSeconds(base.SampleFrequency) <= currentTime;
	}

	public void SampleData(PerformanceCounterReaderService reader)
	{
		reader.DoSample(base.CategoryName);
	}

	public void SampleData(DateTime currentTime, PerformanceCounterReaderService reader)
	{
		SampleData(reader);
		LastSampleTime = currentTime;
	}

	public PerformanceCounterValue GetValue(PerformanceCounterReaderService reader)
	{
		LastLiveRequest = DateTime.UtcNow;
		string instanceName = ((base.CategoryType == PerformanceCounterCategoryType.SingleInstance) ? "" : base.InstanceName);
		return reader.GetValue(base.CategoryName, base.CounterName, instanceName);
	}

	public PerformanceCounterInstance GetInstance()
	{
		return new PerformanceCounterInstance(base.InstanceName, base.CounterName, base.CategoryName)
		{
			EnableHistory = base.EnableHistory,
			TriggerStatePerValue = base.TriggerStatePerValue,
			CounterDirection = base.CounterDirection,
			CounterType = base.CounterType
		};
	}

	public bool CheckInstanceExists(ref Dictionary<string, HashSet<string>>? cache)
	{
		if (cache == null)
		{
			cache = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
		}
		if (!cache.TryGetValue(base.CategoryName, out HashSet<string> value))
		{
			PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory(base.CategoryName);
			value = new HashSet<string>(performanceCounterCategory.GetInstanceNames(), StringComparer.OrdinalIgnoreCase);
			cache[base.CategoryName] = value;
		}
		return value.Contains(base.InstanceName);
	}

	public bool IsCounterActive(ref Dictionary<string, HashSet<string>>? cache)
	{
		if (base.CategoryType != PerformanceCounterCategoryType.MultiInstance)
		{
			return true;
		}
		if (!CheckInstanceExists(ref cache))
		{
			Active = false;
			return false;
		}
		return true;
	}
}
