using System.Threading;

namespace VideoOS.Common.EnvironmentServices;

public static class TimerExtensions
{
	public static void StopAndDispose(this ITimer timer)
	{
		timer?.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
		WaitHandle waitHandle = null;
		try
		{
			if (timer != null)
			{
				waitHandle = new AutoResetEvent(initialState: false);
				timer.Dispose(waitHandle);
				waitHandle.WaitOne();
			}
			timer = null;
		}
		finally
		{
			waitHandle?.Dispose();
		}
	}
}
