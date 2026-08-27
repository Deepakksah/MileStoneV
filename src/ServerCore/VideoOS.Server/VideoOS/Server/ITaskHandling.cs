using System;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Server;

public interface ITaskHandling
{
	ServerTaskDto[] GetPendingTasks(Guid serviceId, long sequenceNumber);

	void RemoveTask(long taskId);
}
