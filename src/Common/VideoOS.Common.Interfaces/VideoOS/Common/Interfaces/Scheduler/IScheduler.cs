using System;

namespace VideoOS.Common.Interfaces.Scheduler;

public interface IScheduler
{
	bool AlwaysUseSeparateThreads { get; set; }

	void CancelAllTasks();

	ISchedulable CancelTask(Guid taskId);

	ISchedulable FindTask(Guid taskId);

	TaskInfo[] GetAllScheduledTasks();

	Guid ScheduleTask(ISchedulable task, DateTime utcTime);

	Guid ScheduleTask(ISchedulable task, long delay);

	Guid ScheduleTask(ISchedulable task, TimeSpan delay);

	Guid ScheduleTask(ISchedulable task, DateTime utcTime, Guid taskId);

	Guid ScheduleTask(ISchedulable task, long delay, Guid taskId);

	Guid ScheduleTask(ISchedulable task, TimeSpan delay, Guid taskId);
}
