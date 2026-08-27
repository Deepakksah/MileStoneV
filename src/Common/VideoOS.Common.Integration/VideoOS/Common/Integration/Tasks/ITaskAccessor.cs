using System;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.Tasks;

public interface ITaskAccessor
{
	ServerTaskDto[] QueryTasks(long lastSequenceNumber);

	ServerTaskDto[] QueryStoppingTasks();

	void UpdateTaskProgress(long taskId, int percentComplete, string details, int status, DateTime endTime);

	void UpdateTaskProgressAsync(long taskId, int percentComplete, string details, int status, DateTime endTime);

	void UpdateTaskSucceeded(long taskId, string result);

	void UpdateTaskSucceededAsync(long taskId, string result);

	void UpdateTaskFailed(long taskId, string description, bool isFatal, string exceptionMessage);

	void UpdateTaskFailedAsync(long taskId, string description, bool isFatal, string exceptionMessage);

	void UpdateTaskStopped(long taskId);

	void UpdateTaskStoppedAsync(long taskId);

	void RemoveTask(long taskId);

	void RemoveTaskAsync(long taskId);
}
