using System;

namespace VideoOS.Central.Util;

public interface IDelayedExecutorSettings
{
	DateTime StartTimeUtc { get; }

	TimeSpan MaxDuration { get; }

	TimeSpan Delay { get; }
}
