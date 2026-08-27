using System;

namespace VideoOS.Common.Integration.Threading.Internal;

public class NullSTPInstancePerformanceCounters : ISTPInstancePerformanceCounters, IDisposable, ISTPPerformanceCountersReader
{
	private static readonly NullSTPInstancePerformanceCounters _instance = new NullSTPInstancePerformanceCounters();

	public static NullSTPInstancePerformanceCounters Instance => _instance;

	public long InUseThreads => 0L;

	public long ActiveThreads => 0L;

	public long WorkItemsQueued => 0L;

	public long WorkItemsProcessed => 0L;

	public void Close()
	{
	}

	public void Dispose()
	{
	}

	public void SampleThreads(long activeThreads, long inUseThreads)
	{
	}

	public void SampleWorkItems(long workItemsQueued, long workItemsProcessed)
	{
	}

	public void SampleWorkItemsWaitTime(TimeSpan workItemWaitTime)
	{
	}

	public void SampleWorkItemsProcessTime(TimeSpan workItemProcessTime)
	{
	}
}
