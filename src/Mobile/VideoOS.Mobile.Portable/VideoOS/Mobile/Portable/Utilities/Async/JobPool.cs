using System;
using System.Threading.Tasks;

namespace VideoOS.Mobile.Portable.Utilities.Async;

public class JobPool
{
	public enum DoJobStrategy
	{
		SyncIfPoolIsFull,
		AlwaysAsync
	}

	private readonly object _locker = new object();

	private int _jobsCount;

	public int MaxNumberAsyncJobs { get; set; }

	public int ActiveJobsCount => _jobsCount;

	public int FreeJobsCount => MaxNumberAsyncJobs - _jobsCount;

	public bool DoJob(Action jobAction, DoJobStrategy strategy = DoJobStrategy.SyncIfPoolIsFull)
	{
		bool flag = FreeJobsCount > 0;
		if (flag || strategy == DoJobStrategy.AlwaysAsync)
		{
			lock (_locker)
			{
				_jobsCount++;
			}
			Task.Factory.StartNew(new JobActionTaskWorker(jobAction, AsyncJobCompleted).DoTheJob);
		}
		else
		{
			jobAction();
		}
		return flag;
	}

	private void AsyncJobCompleted()
	{
		lock (_locker)
		{
			_jobsCount--;
		}
	}
}
