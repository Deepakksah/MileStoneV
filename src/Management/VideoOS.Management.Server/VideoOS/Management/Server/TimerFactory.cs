using System.Threading;

namespace VideoOS.Management.Server;

public class TimerFactory : ITimerFactory
{
	public Timer CreateScheduleTimer(TimerCallback callback, object state, long dueTime, long period)
	{
		return new Timer(callback, state, dueTime, period);
	}
}
