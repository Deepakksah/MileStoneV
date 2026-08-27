using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Security;
using VideoOS.Management.Common.Tasks;

namespace VideoOS.Common.Integration.Tasks;

public abstract class TaskManagerBase : IDisposable, IResponseHandler, ITaskManager
{
	private Timer availableTasksTimer;

	private Timer stoppingTasksTimer;

	private object getTaskSync = new object();

	private long lastTaskSeqNumber;

	private List<TaskCommandBase> runningTasks;

	protected ITaskAccessor TaskAccessor { get; private set; }

	protected ITaskCommandFactory TaskCommandFactory { get; private set; }

	protected ITaskProcessor TaskProcessor { get; private set; }

	protected TimeSpan TimeBetweenGetServerTasks { get; set; }

	protected TimeSpan TimeBetweenGetStoppingTasks { get; set; }

	protected TimeSpan ShutdownWaitForTasksTime { get; set; }

	protected TimeSpan RetryTaskWaitTime { get; set; }

	protected Collection<IStartStopCommandHandler> StartStopCommandHandlers { get; private set; }

	public WaitHandle StopEvent { get; private set; }

	public ILogger Logger { get; }

	public bool HasRunningTasks
	{
		get
		{
			lock (runningTasks)
			{
				return runningTasks.Count > 0;
			}
		}
	}

	protected TaskManagerBase(ITaskAccessor taskAccessor, ITaskCommandFactory taskCommandFactory, ITaskProcessor taskProcessor, ILogger logger)
	{
		if (taskAccessor == null)
		{
			throw new ArgumentNullException("taskAccessor");
		}
		if (taskCommandFactory == null)
		{
			throw new ArgumentNullException("taskCommandFactory");
		}
		if (taskProcessor == null)
		{
			throw new ArgumentNullException("taskProcessor");
		}
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		TaskAccessor = taskAccessor;
		TaskCommandFactory = taskCommandFactory;
		TaskProcessor = taskProcessor;
		Logger = logger;
		TimeBetweenGetServerTasks = TimeSpan.FromSeconds(60.0);
		TimeBetweenGetStoppingTasks = TimeSpan.FromSeconds(10.0);
		ShutdownWaitForTasksTime = TimeSpan.FromSeconds(60.0);
		RetryTaskWaitTime = TimeSpan.FromSeconds(30.0);
		availableTasksTimer = new Timer(OnAvailableTasksTimerCallback);
		stoppingTasksTimer = new Timer(OnStoppingTasksTimerCallback);
		StopEvent = new ManualResetEvent(initialState: false);
		runningTasks = new List<TaskCommandBase>();
		StartStopCommandHandlers = new Collection<IStartStopCommandHandler>();
	}

	public void Start()
	{
		LoggerExtensions.LogInformation(Logger, "Starting task manager", Array.Empty<object>());
		foreach (IStartStopCommandHandler startStopCommandHandler in StartStopCommandHandlers)
		{
			startStopCommandHandler.Start();
		}
		availableTasksTimer.Change(TimeSpan.Zero, TimeBetweenGetServerTasks);
		stoppingTasksTimer.Change(TimeSpan.Zero, TimeBetweenGetStoppingTasks);
	}

	public void Stop()
	{
		LoggerExtensions.LogInformation(Logger, "Stopping task manager", Array.Empty<object>());
		availableTasksTimer.Change(-1, -1);
		stoppingTasksTimer.Change(-1, -1);
		((ManualResetEvent)StopEvent).Set();
		lock (runningTasks)
		{
			if (runningTasks.Count > 0 && !Monitor.Wait(runningTasks, ShutdownWaitForTasksTime, exitContext: false))
			{
				foreach (TaskCommandBase runningTask in runningTasks)
				{
					LoggerExtensions.LogWarning(Logger, "The following task could not stop within the expected time during shutdown: {Name}", new object[1] { runningTask.GetType().Name });
				}
			}
		}
		foreach (IStartStopCommandHandler startStopCommandHandler in StartStopCommandHandlers)
		{
			startStopCommandHandler.Stop();
		}
	}

	public virtual void CheckForTasksAvailable()
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		lock (getTaskSync)
		{
			stopwatch.Stop();
			if (stopwatch.ElapsedMilliseconds > 1000)
			{
				LoggerExtensions.LogWarning(Logger, "CheckForTasksAvailable: Waited {ElapsedMilliseconds} milliseconds to obtain GetTask sync object", new object[1] { stopwatch.ElapsedMilliseconds });
			}
			ServerTaskDto[] array = TaskAccessor.QueryTasks(lastTaskSeqNumber);
			if (array.Length != 0)
			{
				long num = lastTaskSeqNumber;
				long num2 = array.Max((ServerTaskDto t) => t.SequenceNumber);
				LoggerExtensions.LogInformation(Logger, "CheckForTasksAvailable: New tasks {ServerTasksCount} (range:{PreviousSequenceNo} - {NewSequenceNo})", new object[3] { array.Length, num, num2 });
				try
				{
					ProcessConfigurationChangeTasks(array);
					ProcessSecurityConfigurationChangeTasks(array);
					ProcessAsyncTasks(array);
					LoggerExtensions.LogInformation(Logger, "CheckForTasksAvailable: {ServerTasksCount} task(s) processed", new object[1] { array.Length });
				}
				catch (Exception ex)
				{
					LoggerExtensions.LogError(Logger, "CheckForTasksAvailable: Error processing task(s) {ServerTasksCount}, {Exception}", new object[2] { array.Length, ex });
					throw;
				}
				lastTaskSeqNumber = num2;
				Task.Factory.StartNew(delegate
				{
					CheckForTasksAvailable();
				}, TaskCreationOptions.LongRunning);
			}
		}
	}

	protected virtual void ProcessConfigurationChangeTasks(ServerTaskDto[] allServerTasks)
	{
		foreach (ServerTaskDto item in from t in allServerTasks
			where t.TaskType == ServerTaskTypeDto.ConfigurationDiffGram
			orderby t.SequenceNumber
			select t)
		{
			using ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
			ReadDataSet(configurationDataSet, item.TaskXml);
			TaskProcessor.UpdateConfiguration(configurationDataSet);
		}
	}

	protected virtual void ProcessSecurityConfigurationChangeTasks(ServerTaskDto[] allServerTasks)
	{
		foreach (ServerTaskDto item in from t in allServerTasks
			where t.TaskType == ServerTaskTypeDto.SecurityDiffGram
			orderby t.SequenceNumber
			select t)
		{
			using SecurityDataSet securityDataSet = new SecurityDataSet();
			ReadDataSet(securityDataSet, item.TaskXml);
			TaskProcessor.UpdateSecurityConfiguration(securityDataSet);
		}
	}

	protected virtual void ProcessAsyncTasks(ServerTaskDto[] allServerTasks)
	{
		foreach (ServerTaskDto item in from t in allServerTasks
			where t.TaskType == ServerTaskTypeDto.Task && (t.Status == null || t.Status.Equals("0", StringComparison.OrdinalIgnoreCase) || t.Status.Equals("1", StringComparison.OrdinalIgnoreCase) || t.Status.Equals("4", StringComparison.OrdinalIgnoreCase))
			orderby t.SequenceNumber
			select t)
		{
			TaskCommandBase taskCommandBase = TaskCommandFactory.CreateTaskCommand(item);
			if (taskCommandBase != null)
			{
				QueueTask(taskCommandBase);
			}
		}
	}

	public void CheckForStoppingTasks()
	{
		lock (getTaskSync)
		{
			ServerTaskDto[] array = TaskAccessor.QueryStoppingTasks();
			lock (runningTasks)
			{
				ServerTaskDto[] array2 = array;
				foreach (ServerTaskDto serverTaskDto in array2)
				{
					TaskCommandBase taskCommandBase = runningTasks.Find((TaskCommandBase x) => x.TaskId.Equals(serverTaskDto.SequenceNumber));
					if (taskCommandBase != null && taskCommandBase is IStoppableTaskCommand stoppableTaskCommand)
					{
						stoppableTaskCommand.StopTask();
					}
				}
			}
		}
	}

	private void QueueTask(TaskCommandBase command)
	{
		lock (runningTasks)
		{
			if (!StopEvent.WaitOne(0, exitContext: false))
			{
				runningTasks.Add(command);
				TaskProcessor.ProcessTaskCommand(command, this);
			}
		}
	}

	void IResponseHandler.HandleResponse(CommandBase command, ResponseBase response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (!(command is TaskCommandBase taskCommandBase))
		{
			throw new ArgumentException("Command must inherit from TaskCommandBase", "command");
		}
		if (response is TaskProgressResponse taskProgressResponse)
		{
			DateTime endTime = TaskCommandBase.CalculateEstimatedEndTime(taskProgressResponse.StartTime, taskProgressResponse.PercentComplete);
			if (taskProgressResponse.Async)
			{
				TaskAccessor.UpdateTaskProgressAsync(taskCommandBase.TaskId, taskProgressResponse.PercentComplete, "", 1, endTime);
			}
			else
			{
				TaskAccessor.UpdateTaskProgress(taskCommandBase.TaskId, taskProgressResponse.PercentComplete, "", 1, endTime);
			}
			return;
		}
		UnregisterRunningTask(taskCommandBase);
		if (response is TaskSuccessResponse taskSuccessResponse)
		{
			if (taskSuccessResponse.Async)
			{
				TaskAccessor.UpdateTaskSucceededAsync(taskCommandBase.TaskId, taskSuccessResponse.Result);
				if (!taskCommandBase.Permanent)
				{
					TaskAccessor.RemoveTaskAsync(taskCommandBase.TaskId);
				}
			}
			else
			{
				TaskAccessor.UpdateTaskSucceeded(taskCommandBase.TaskId, taskSuccessResponse.Result);
				if (!taskCommandBase.Permanent)
				{
					TaskAccessor.RemoveTask(taskCommandBase.TaskId);
				}
			}
		}
		else if (response is SuccessResponse)
		{
			TaskAccessor.UpdateTaskSucceeded(taskCommandBase.TaskId, null);
			if (!taskCommandBase.Permanent)
			{
				TaskAccessor.RemoveTask(taskCommandBase.TaskId);
			}
		}
		else if (response is TaskStoppedResponse taskStoppedResponse)
		{
			if (taskStoppedResponse.Async)
			{
				TaskAccessor.UpdateTaskStoppedAsync(taskCommandBase.TaskId);
				if (!taskCommandBase.Permanent)
				{
					TaskAccessor.RemoveTaskAsync(taskCommandBase.TaskId);
				}
			}
			else
			{
				TaskAccessor.UpdateTaskStopped(taskCommandBase.TaskId);
				if (!taskCommandBase.Permanent)
				{
					TaskAccessor.RemoveTask(taskCommandBase.TaskId);
				}
			}
		}
		else
		{
			if (response is TaskAbortResponse)
			{
				return;
			}
			if (response is TaskFailedResponse taskFailedResponse)
			{
				if (taskFailedResponse.IsFatal || taskCommandBase.StopRetry)
				{
					if (taskFailedResponse.Async)
					{
						TaskAccessor.UpdateTaskFailedAsync(taskCommandBase.TaskId, taskFailedResponse.Description, taskFailedResponse.IsFatal, taskFailedResponse.StatusDetails);
						if (!taskCommandBase.Permanent)
						{
							TaskAccessor.RemoveTaskAsync(taskCommandBase.TaskId);
						}
					}
					else
					{
						TaskAccessor.UpdateTaskFailed(taskCommandBase.TaskId, taskFailedResponse.Description, taskFailedResponse.IsFatal, taskFailedResponse.StatusDetails);
						if (!taskCommandBase.Permanent)
						{
							TaskAccessor.RemoveTask(taskCommandBase.TaskId);
						}
					}
				}
				else
				{
					RetryCommand(taskCommandBase);
				}
			}
			else if (response is ExceptionResponse exceptionResponse)
			{
				TaskError taskError = new TaskError();
				taskError.ErrorCode = 60254;
				taskError.ErrorText = string.Format(CultureInfo.InvariantCulture, "An public server error occurred.\r\nThe server provided the following error message: {0}", exceptionResponse.Exception.Message);
				taskError.Arguments.Add(0, exceptionResponse.Exception.Message);
				taskError.Arguments.Add(1, exceptionResponse.Exception.ToString());
				TaskAccessor.UpdateTaskFailed(taskCommandBase.TaskId, string.Empty, isFatal: true, taskError.ToXml());
				if (!taskCommandBase.Permanent)
				{
					TaskAccessor.RemoveTask(taskCommandBase.TaskId);
				}
			}
			else
			{
				LoggerExtensions.LogInformation(Logger, "CommandProcessor_ResponseCallback failed. Unknown response type: '{Response}' unable to send to server.", new object[1] { response.GetType() });
			}
		}
	}

	private void RetryCommand(TaskCommandBase taskCommandBase)
	{
		Task.Factory.StartNew(delegate
		{
			if (!StopEvent.WaitOne(RetryTaskWaitTime, exitContext: false))
			{
				taskCommandBase.RetryCount++;
				QueueTask(taskCommandBase);
			}
		}, TaskCreationOptions.LongRunning);
	}

	private void UnregisterRunningTask(TaskCommandBase command)
	{
		lock (runningTasks)
		{
			runningTasks.Remove(command);
			if (runningTasks.Count == 0)
			{
				Monitor.Pulse(runningTasks);
			}
		}
	}

	private void OnAvailableTasksTimerCallback(object state)
	{
		try
		{
			CheckForTasksAvailable();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			LoggerExtensions.LogError(Logger, "OnAvailableTasksTimerCallback() failed. Exception: {Exception}", new object[1] { ex2 });
		}
	}

	private void OnStoppingTasksTimerCallback(object state)
	{
		try
		{
			CheckForStoppingTasks();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			LoggerExtensions.LogError(Logger, "OnStoppingTasksTimerCallback() failed. Exception: {Exception}", new object[1] { ex2 });
		}
	}

	private static void ReadDataSet(DataSet dataSet, string dataSetXml)
	{
		using TextReader input = new StringReader(dataSetXml);
		using XmlReader reader = XmlReader.Create(input);
		dataSet.ReadXml(reader, XmlReadMode.DiffGram);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (availableTasksTimer != null)
			{
				availableTasksTimer.Dispose();
				availableTasksTimer = null;
			}
			if (stoppingTasksTimer != null)
			{
				stoppingTasksTimer.Dispose();
				stoppingTasksTimer = null;
			}
		}
	}
}
