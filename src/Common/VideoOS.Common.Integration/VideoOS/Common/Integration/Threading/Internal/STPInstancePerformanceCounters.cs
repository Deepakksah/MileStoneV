using System;

namespace VideoOS.Common.Integration.Threading.Internal;

public class STPInstancePerformanceCounters : ISTPInstancePerformanceCounters, IDisposable
{
	private bool _isDisposed;

	private STPInstancePerformanceCounter[] _pcs;

	private static readonly STPInstancePerformanceCounter _stpInstanceNullPerformanceCounter;

	static STPInstancePerformanceCounters()
	{
		_stpInstanceNullPerformanceCounter = new STPInstanceNullPerformanceCounter();
	}

	public STPInstancePerformanceCounters(string instance)
	{
		_isDisposed = false;
		_pcs = new STPInstancePerformanceCounter[14];
		STPPerformanceCounters.Instance.GetHashCode();
		for (int i = 0; i < _pcs.Length; i++)
		{
			if (instance != null)
			{
				_pcs[i] = new STPInstancePerformanceCounter(instance, (STPPerformanceCounterType)i);
			}
			else
			{
				_pcs[i] = _stpInstanceNullPerformanceCounter;
			}
		}
	}

	public void Close()
	{
		if (_pcs == null)
		{
			return;
		}
		for (int i = 0; i < _pcs.Length; i++)
		{
			if (_pcs[i] != null)
			{
				_pcs[i].Dispose();
			}
		}
		_pcs = null;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public virtual void Dispose(bool disposing)
	{
		if (!_isDisposed && disposing)
		{
			Close();
		}
		_isDisposed = true;
	}

	private STPInstancePerformanceCounter GetCounter(STPPerformanceCounterType spcType)
	{
		return _pcs[(int)spcType];
	}

	public void SampleThreads(long activeThreads, long inUseThreads)
	{
		GetCounter(STPPerformanceCounterType.ActiveThreads).Set(activeThreads);
		GetCounter(STPPerformanceCounterType.InUseThreads).Set(inUseThreads);
		GetCounter(STPPerformanceCounterType.OverheadThreads).Set(activeThreads - inUseThreads);
		GetCounter(STPPerformanceCounterType.OverheadThreadsPercentBase).Set(activeThreads - inUseThreads);
		GetCounter(STPPerformanceCounterType.OverheadThreadsPercent).Set(inUseThreads);
	}

	public void SampleWorkItems(long workItemsQueued, long workItemsProcessed)
	{
		GetCounter(STPPerformanceCounterType.WorkItems).Set(workItemsQueued + workItemsProcessed);
		GetCounter(STPPerformanceCounterType.WorkItemsInQueue).Set(workItemsQueued);
		GetCounter(STPPerformanceCounterType.WorkItemsProcessed).Set(workItemsProcessed);
		GetCounter(STPPerformanceCounterType.WorkItemsQueuedPerSecond).Set(workItemsQueued);
		GetCounter(STPPerformanceCounterType.WorkItemsProcessedPerSecond).Set(workItemsProcessed);
	}

	public void SampleWorkItemsWaitTime(TimeSpan workItemWaitTime)
	{
		GetCounter(STPPerformanceCounterType.AvgWorkItemWaitTime).IncrementBy((long)workItemWaitTime.TotalMilliseconds);
		GetCounter(STPPerformanceCounterType.AvgWorkItemWaitTimeBase).Increment();
	}

	public void SampleWorkItemsProcessTime(TimeSpan workItemProcessTime)
	{
		GetCounter(STPPerformanceCounterType.AvgWorkItemProcessTime).IncrementBy((long)workItemProcessTime.TotalMilliseconds);
		GetCounter(STPPerformanceCounterType.AvgWorkItemProcessTimeBase).Increment();
	}
}
