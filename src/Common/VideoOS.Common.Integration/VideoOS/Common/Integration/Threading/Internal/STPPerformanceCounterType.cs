namespace VideoOS.Common.Integration.Threading.Internal;

public enum STPPerformanceCounterType
{
	ActiveThreads = 0,
	InUseThreads = 1,
	OverheadThreads = 2,
	OverheadThreadsPercent = 3,
	OverheadThreadsPercentBase = 4,
	WorkItems = 5,
	WorkItemsInQueue = 6,
	WorkItemsProcessed = 7,
	WorkItemsQueuedPerSecond = 8,
	WorkItemsProcessedPerSecond = 9,
	AvgWorkItemWaitTime = 10,
	AvgWorkItemWaitTimeBase = 11,
	AvgWorkItemProcessTime = 12,
	AvgWorkItemProcessTimeBase = 13,
	WorkItemsGroups = 14,
	LastCounter = 14
}
