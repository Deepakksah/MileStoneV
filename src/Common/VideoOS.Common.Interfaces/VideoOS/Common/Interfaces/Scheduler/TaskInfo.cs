using System;

namespace VideoOS.Common.Interfaces.Scheduler;

public class TaskInfo
{
	public Guid TaskId { get; }

	public ISchedulable Task { get; }

	public DateTime TimeStamp { get; }

	public TaskInfo(Guid taskId, ISchedulable task, DateTime timeStamp)
	{
		TaskId = taskId;
		Task = task;
		TimeStamp = timeStamp;
	}
}
