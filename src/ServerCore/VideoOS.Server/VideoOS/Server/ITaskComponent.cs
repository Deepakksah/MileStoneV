using System;

namespace VideoOS.Server;

public interface ITaskComponent : IDisposable
{
	long CreateTask(string type, Guid serviceId, string data_a, int percentComplete, DateTime startTime);

	TaskHandling.RawServerTask[] GetPendingTasks(Guid serviceId, long sequenceNumber);

	TaskHandling.RawServerTask[] GetTasksByType(Guid serviceId, string type);

	TaskHandling.RawServerTask[] GetTasksWithPendingStop(Guid serviceId);

	void RemoveTask(long taskId);

	void RemoveTasksByDeviceIds(Guid[] deviceIds);

	string StopTask(long taskId);

	string UpdateTaskProgress(long taskId, int percentComplete, string details, int status, DateTime endTime);
}
