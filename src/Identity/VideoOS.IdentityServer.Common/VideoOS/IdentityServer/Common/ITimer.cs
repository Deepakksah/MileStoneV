using System;

namespace VideoOS.IdentityServer.Common;

public interface ITimer : IDisposable
{
	event EventHandler Elapsed;

	void StartTimer(TimeSpan delay, TimeSpan period);

	void StopTimer();
}
