using System;

namespace VideoOS.Management.Server;

public interface IScheduledAction<T> : IDisposable
{
	TimeSpan ScheduleTimeSpan { get; }

	void StartSchedule(T scheduleData);

	void StopSchedule();
}
