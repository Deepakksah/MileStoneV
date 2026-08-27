using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Tasks;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.Tasks;

public class ServerTaskAccessor : ITaskAccessor
{
	private static readonly Guid ServerServiceId = new Guid("909749AE-88C4-4780-A381-383D691E98FA");

	private readonly ILogger _logger;

	public ServerTaskAccessor(ILogger logger)
	{
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		_logger = logger;
	}

	public ServerTaskDto[] QueryTasks(long lastSequenceNumber)
	{
		try
		{
			return TaskHandling.GetPendingTasks(ServerServiceId, lastSequenceNumber);
		}
		catch (DatabaseException ex)
		{
			_logger.LogWarning("GetRecorderTasks() failed. Exception: {Exception}", ex);
			return new ServerTaskDto[0];
		}
	}

	public ServerTaskDto[] QueryStoppingTasks()
	{
		try
		{
			return TaskHandling.GetTasksWithPendingStop(ServerServiceId);
		}
		catch (DatabaseException ex)
		{
			_logger.LogWarning("GetTasksWithPendingStop() failed. Exception: {Exception}", ex);
			return new ServerTaskDto[0];
		}
	}

	public void UpdateTaskProgress(long taskId, int percentComplete, string details, int status, DateTime endTime)
	{
		TaskHandling.UpdateTaskProgress(taskId, percentComplete, details, status, endTime);
	}

	public void UpdateTaskProgressAsync(long taskId, int percentComplete, string details, int status, DateTime endTime)
	{
		Task.Factory.StartNew(delegate
		{
			UpdateTaskProgress(taskId, percentComplete, details, status, endTime);
		}, TaskCreationOptions.LongRunning);
	}

	public void UpdateTaskSucceeded(long taskId, string result)
	{
		TaskHandling.UpdateTaskProgress(taskId, 100, result, 2, DateTime.UtcNow);
	}

	public void UpdateTaskSucceededAsync(long taskId, string result)
	{
		Task.Factory.StartNew(delegate
		{
			UpdateTaskSucceeded(taskId, result);
		}, TaskCreationOptions.LongRunning);
	}

	public void UpdateTaskFailed(long taskId, string description, bool isFatal, string exceptionMessage)
	{
		TaskHandling.UpdateTaskProgress(taskId, 100, exceptionMessage, 3, DateTime.UtcNow);
	}

	public void UpdateTaskFailedAsync(long taskId, string description, bool isFatal, string exceptionMessage)
	{
		Task.Factory.StartNew(delegate
		{
			UpdateTaskFailed(taskId, description, isFatal, exceptionMessage);
		}, TaskCreationOptions.LongRunning);
	}

	public void UpdateTaskStopped(long taskId)
	{
		TaskHandling.StopTask(taskId);
	}

	public void UpdateTaskStoppedAsync(long taskId)
	{
		Task.Factory.StartNew(delegate
		{
			UpdateTaskStopped(taskId);
		}, TaskCreationOptions.LongRunning);
	}

	public void RemoveTask(long taskId)
	{
		TaskHandling.RemoveTask(taskId);
	}

	public void RemoveTaskAsync(long taskId)
	{
		Task.Factory.StartNew(delegate
		{
			RemoveTask(taskId);
		}, TaskCreationOptions.LongRunning);
	}
}
