using System;

namespace VideoOS.DataCollector.Service.Entities.DataCollection;

public class PerformanceCounterValue
{
	public string CategoryName { get; set; }

	public string CounterName { get; set; }

	public string InstanceName { get; set; }

	public DateTime CollectionTime { get; set; }

	public long RawValue { get; set; }

	public float CalculatedValue { get; set; }

	public bool SendImmediately { get; set; }

	public PerformanceCounterValue(string instanceName, string counterName, string categoryName)
	{
		InstanceName = instanceName;
		CounterName = counterName;
		CategoryName = categoryName;
	}
}
