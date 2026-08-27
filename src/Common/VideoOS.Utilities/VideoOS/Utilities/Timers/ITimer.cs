using System;

namespace VideoOS.Utilities.Timers;

public interface ITimer : IDisposable
{
	event EventHandler Elapsed;

	void StartTimer();

	void StartTimer(TimeSpan delay, TimeSpan period);

	void StopTimer();
}
