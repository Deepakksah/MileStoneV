using System;

namespace VideoOS.Central.Util;

public class DelayedExecutorSettings : IDelayedExecutorSettings
{
	public DateTime StartTimeUtc { get; }

	public TimeSpan MaxDuration { get; }

	public TimeSpan Delay { get; }

	public DelayedExecutorSettings(TimeSpan delay, TimeSpan maxDuration)
	{
		StartTimeUtc = DateTime.UtcNow;
		MaxDuration = maxDuration;
		Delay = delay;
	}
}
