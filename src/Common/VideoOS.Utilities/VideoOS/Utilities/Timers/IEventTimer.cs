using System;

namespace VideoOS.Utilities.Timers;

public interface IEventTimer : ITimer, IDisposable
{
	bool AutoReset { get; set; }

	double Interval { get; set; }
}
