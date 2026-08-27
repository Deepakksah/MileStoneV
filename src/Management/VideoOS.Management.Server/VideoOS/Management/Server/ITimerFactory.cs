using System.Threading;

namespace VideoOS.Management.Server;

public interface ITimerFactory
{
	Timer CreateScheduleTimer(TimerCallback callback, object state, long dueTime, long period);
}
