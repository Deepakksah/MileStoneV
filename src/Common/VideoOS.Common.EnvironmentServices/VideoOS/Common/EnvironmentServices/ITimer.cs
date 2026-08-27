using System;
using System.Threading;

namespace VideoOS.Common.EnvironmentServices;

public interface ITimer : IDisposable
{
	event EventHandler Elapsed;

	void StartTimer();

	void StartTimer(TimeSpan delay, TimeSpan period);

	void StopTimer();

	void Change(TimeSpan delay, TimeSpan period);

	bool Dispose(WaitHandle notifyObject);
}
