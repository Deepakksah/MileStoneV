using System;

namespace VideoOS.Rules;

public class SubdayRecurringInfo
{
	public TimeSpan Frequency = TimeSpan.MinValue;

	public TimeSpan StartTimeOfDay = TimeSpan.MinValue;

	public TimeSpan EndTimeOfDay = TimeSpan.MinValue;

	public bool IsMidnightCrossed()
	{
		return EndTimeOfDay < StartTimeOfDay;
	}
}
