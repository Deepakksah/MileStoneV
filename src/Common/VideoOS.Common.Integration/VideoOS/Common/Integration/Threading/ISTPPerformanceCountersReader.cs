namespace VideoOS.Common.Integration.Threading;

public interface ISTPPerformanceCountersReader
{
	long InUseThreads { get; }

	long ActiveThreads { get; }

	long WorkItemsQueued { get; }

	long WorkItemsProcessed { get; }
}
