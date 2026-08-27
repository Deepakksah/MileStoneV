using System;

namespace VideoOS.DataCollector.Service;

public class CounterValue
{
	public DateTime Time { get; private set; }

	public float Value { get; private set; }

	public CounterValue(DateTime time, float value)
	{
		Time = time;
		Value = value;
	}
}
