using System;

namespace VideoOS.DataCollector.Service.Rule.Entities.Sample;

public record PerformanceCounterSample
{
	public string CategoryName { get; }

	public string CounterName { get; }

	public string InstanceName { get; }

	public DateTime CollectionTime { get; }

	public long RawValue { get; }

	public float CalculatedValue { get; }

	public PerformanceCounterSample(string categoryName, string counterName, string instanceName, DateTime collectionTime, long rawValue, float calculatedValue)
	{
		CategoryName = categoryName;
		CounterName = counterName;
		InstanceName = instanceName;
		CollectionTime = collectionTime;
		RawValue = rawValue;
		CalculatedValue = calculatedValue;
	}
}
