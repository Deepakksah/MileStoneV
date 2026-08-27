using System;
using System.Diagnostics;
using System.Diagnostics.PerformanceData;

namespace VideoOS.Management.Server;

public struct GeminiPerformanceCounter
{
	private readonly CounterData _counterData;

	public GeminiPerformanceCounter(CounterData counterData)
	{
		_counterData = counterData;
	}

	public long Increment()
	{
		return IncrementBy(1L);
	}

	public long Decrement()
	{
		return IncrementBy(-1L);
	}

	public long IncrementTicks(TimeSpan timeSpan)
	{
		return IncrementBy((long)timeSpan.TotalSeconds * Stopwatch.Frequency);
	}

	public long IncrementMilliseconds(long milliseconds)
	{
		return IncrementBy(milliseconds * Stopwatch.Frequency / 1000);
	}

	public long IncrementBy(long value)
	{
		if (_counterData == null)
		{
			return 0L;
		}
		_counterData.IncrementBy(value);
		if (_counterData.Value < 0)
		{
			_counterData.Value = 0L;
		}
		return _counterData.Value;
	}
}
