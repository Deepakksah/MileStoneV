using System;
using System.Collections;
using System.Threading;
using VideoOS.Common.Interfaces.Scheduler;

namespace VideoOS.Vms.Utilities.Scheduler;

public class Scheduler : IScheduler, IDisposable
{
	private AutoResetEvent waitObject;

	private RegisteredWaitHandle waitHandle;

	private DateTime currentTimeStamp;

	private SortedList scheduledTasks;

	private Hashtable taskIdMap;

	private bool alwaysUseSeparateThreads;

	private object syncRoot = new object();

	public bool AlwaysUseSeparateThreads
	{
		get
		{
			return alwaysUseSeparateThreads;
		}
		set
		{
			alwaysUseSeparateThreads = value;
		}
	}

	public Scheduler()
	{
		waitObject = new AutoResetEvent(initialState: false);
		waitHandle = null;
		currentTimeStamp = DateTime.MaxValue;
		scheduledTasks = new SortedList();
		taskIdMap = new Hashtable();
		alwaysUseSeparateThreads = false;
	}

	public Guid ScheduleTask(ISchedulable task, long delay, Guid taskId)
	{
		if (task == null)
		{
			throw new ArgumentNullException("task");
		}
		if (delay < 0)
		{
			throw new ArgumentException("The delay for a task passed to the scheduler must be non-negative", "delay");
		}
		DateTime utcTime = DateTime.UtcNow.AddMilliseconds(delay);
		return ScheduleTask(task, utcTime, taskId);
	}

	public Guid ScheduleTask(ISchedulable task, long delay)
	{
		return ScheduleTask(task, delay, Guid.Empty);
	}

	public Guid ScheduleTask(ISchedulable task, TimeSpan delay, Guid taskId)
	{
		if (task == null)
		{
			throw new ArgumentNullException("task");
		}
		if (delay < TimeSpan.Zero)
		{
			throw new ArgumentException("The delay for a task passed to the scheduler must be non-negative", "delay");
		}
		DateTime utcTime = DateTime.UtcNow.Add(delay);
		return ScheduleTask(task, utcTime, taskId);
	}

	public Guid ScheduleTask(ISchedulable task, TimeSpan delay)
	{
		return ScheduleTask(task, delay, Guid.Empty);
	}

	public Guid ScheduleTask(ISchedulable task, DateTime utcTime, Guid taskId)
	{
		if (task == null)
		{
			throw new ArgumentNullException("task");
		}
		TaskInfo taskInfo = (taskId.Equals(Guid.Empty) ? new TaskInfo(Guid.NewGuid(), task, utcTime) : new TaskInfo(taskId, task, utcTime));
		lock (syncRoot)
		{
			ArrayList arrayList = scheduledTasks[utcTime] as ArrayList;
			if (arrayList == null)
			{
				arrayList = new ArrayList();
				scheduledTasks[utcTime] = arrayList;
			}
			arrayList.Add(taskInfo);
			taskIdMap[taskInfo.TaskId] = taskInfo;
			if (waitHandle == null)
			{
				WaitNext();
			}
			else if (waitHandle != null && utcTime < currentTimeStamp)
			{
				waitHandle.Unregister(null);
				WaitNext();
			}
			return taskInfo.TaskId;
		}
	}

	public Guid ScheduleTask(ISchedulable task, DateTime utcTime)
	{
		return ScheduleTask(task, utcTime, Guid.Empty);
	}

	public ISchedulable CancelTask(Guid taskId)
	{
		lock (syncRoot)
		{
			if (!taskIdMap.ContainsKey(taskId))
			{
				return null;
			}
			TaskInfo taskInfo = (TaskInfo)taskIdMap[taskId];
			taskIdMap.Remove(taskId);
			ArrayList obj = scheduledTasks[taskInfo.TimeStamp] as ArrayList;
			obj.Remove(taskInfo);
			if (obj.Count == 0)
			{
				scheduledTasks.Remove(taskInfo.TimeStamp);
				if (currentTimeStamp == taskInfo.TimeStamp)
				{
					if (waitHandle != null)
					{
						waitHandle.Unregister(null);
						waitHandle = null;
					}
					WaitNext();
				}
			}
			return taskInfo.Task;
		}
	}

	public TaskInfo[] GetAllScheduledTasks()
	{
		lock (syncRoot)
		{
			TaskInfo[] array = new TaskInfo[taskIdMap.Values.Count];
			taskIdMap.Values.CopyTo(array, 0);
			return array;
		}
	}

	public ISchedulable FindTask(Guid taskId)
	{
		lock (syncRoot)
		{
			if (!taskIdMap.ContainsKey(taskId))
			{
				return null;
			}
			return ((TaskInfo)taskIdMap[taskId]).Task;
		}
	}

	public void CancelAllTasks()
	{
		lock (syncRoot)
		{
			if (waitHandle != null)
			{
				waitHandle.Unregister(null);
				waitHandle = null;
			}
			currentTimeStamp = DateTime.MaxValue;
			scheduledTasks.Clear();
			taskIdMap.Clear();
		}
	}

	private void WaitNext()
	{
		lock (syncRoot)
		{
			if (scheduledTasks.Count != 0)
			{
				DateTime dateTime = (DateTime)scheduledTasks.GetKey(0);
				TimeSpan timeSpan = dateTime.Subtract(DateTime.UtcNow);
				if (timeSpan < TimeSpan.Zero)
				{
					timeSpan = TimeSpan.Zero;
				}
				long millisecondsTimeOutInterval = Convert.ToInt64(timeSpan.TotalMilliseconds);
				waitHandle = ThreadPool.RegisterWaitForSingleObject(waitObject, WaitCallBack, dateTime, millisecondsTimeOutInterval, executeOnlyOnce: true);
				currentTimeStamp = dateTime;
			}
		}
	}

	private void WaitCallBack(object state, bool timedout)
	{
		try
		{
			DateTime dateTime = (DateTime)state;
			if (alwaysUseSeparateThreads)
			{
				TaskInfo taskInfo = null;
				lock (syncRoot)
				{
					if (currentTimeStamp != dateTime)
					{
						return;
					}
					if (scheduledTasks[dateTime] is ArrayList arrayList)
					{
						taskInfo = arrayList[0] as TaskInfo;
						arrayList.RemoveAt(0);
						if (arrayList.Count == 0)
						{
							scheduledTasks.Remove(dateTime);
						}
						taskIdMap.Remove(taskInfo.TaskId);
						if (waitHandle != null)
						{
							waitHandle.Unregister(null);
							waitHandle = null;
						}
					}
					WaitNext();
				}
				if (taskInfo != null)
				{
					try
					{
						taskInfo.Task.ProcessTask(taskInfo.TaskId);
						return;
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch
					{
						return;
					}
				}
				return;
			}
			ArrayList arrayList2;
			lock (syncRoot)
			{
				if (currentTimeStamp != dateTime)
				{
					return;
				}
				arrayList2 = scheduledTasks[dateTime] as ArrayList;
				if (arrayList2 != null)
				{
					scheduledTasks.Remove(dateTime);
					foreach (TaskInfo item in arrayList2)
					{
						taskIdMap.Remove(item.TaskId);
					}
					if (waitHandle != null)
					{
						waitHandle.Unregister(null);
						waitHandle = null;
					}
				}
				WaitNext();
			}
			if (arrayList2 == null)
			{
				return;
			}
			foreach (TaskInfo item2 in arrayList2)
			{
				try
				{
					item2.Task.ProcessTask(item2.TaskId);
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}

	public void Dispose()
	{
		waitObject.Dispose();
		GC.SuppressFinalize(this);
	}
}
