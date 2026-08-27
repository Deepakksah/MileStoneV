using System;

namespace VideoOS.Common.Interfaces.Scheduler;

public interface ISchedulable
{
	void ProcessTask(Guid TaskId);
}
