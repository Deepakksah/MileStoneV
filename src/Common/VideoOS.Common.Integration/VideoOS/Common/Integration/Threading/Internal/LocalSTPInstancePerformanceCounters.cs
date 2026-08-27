using System;

namespace VideoOS.Common.Integration.Threading.Internal;

public class LocalSTPInstancePerformanceCounters : ISTPInstancePerformanceCounters, IDisposable, ISTPPerformanceCountersReader
{
	private long _activeThreads;

	private long _inUseThreads;

	private long _workItemsQueued;

	private long _workItemsProcessed;

	public long InUseThreads => _inUseThreads;

	public long ActiveThreads => _activeThreads;

	public long WorkItemsQueued => _workItemsQueued;

	public long WorkItemsProcessed => _workItemsProcessed;

	public void Close()
	{
	}

	public void Dispose()
	{
	}

	public void SampleThreads(long activeThreads, long inUseThreads)
	{
		_activeThreads = activeThreads;
		_inUseThreads = inUseThreads;
	}

	public void SampleWorkItems(long workItemsQueued, long workItemsProcessed)
	{
		_workItemsQueued = workItemsQueued;
		_workItemsProcessed = workItemsProcessed;
	}

	public void SampleWorkItemsWaitTime(TimeSpan workItemWaitTime)
	{
	}

	public void SampleWorkItemsProcessTime(TimeSpan workItemProcessTime)
	{
	}
}
