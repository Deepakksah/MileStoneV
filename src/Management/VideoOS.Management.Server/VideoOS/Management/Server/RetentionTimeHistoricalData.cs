using System;

namespace VideoOS.Management.Server;

public sealed class RetentionTimeHistoricalData
{
	private double _value;

	private DateTime _time;

	public double Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
		}
	}

	public DateTime Time
	{
		get
		{
			return _time;
		}
		set
		{
			_time = value;
		}
	}

	public static RetentionTimeHistoricalData[] QueryRetentionTimeHistoricalData(IGeminiRequestContext requestContext, RetentionTimeQueryData data)
	{
		RetentionTimeHistoricalData[] array = null;
		using PerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = requestContext.CreateComponent<PerformanceStateRuleConfigComponent>();
		return performanceStateRuleConfigComponent.QueryHistoricalStorageData(data);
	}
}
