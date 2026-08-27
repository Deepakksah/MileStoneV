using System.Diagnostics;

namespace VideoOS.Vms.Utilities.Diagnostics;

public sealed class LiveStreamPerformance : PerformanceCounterCategoryBase
{
	private const string Category = "VideoOS Admin LiveStream";

	private const string CategoryHelp = "Category help";

	private const string CounterEventsReceived = "Items/sec received";

	private const string CounterEventsProcessed = "Items/sec processed";

	public override string CategoryName => "VideoOS Admin LiveStream";

	public override void CreateCategory()
	{
		CounterCreationDataCollection counterCreationDataCollection = new CounterCreationDataCollection();
		counterCreationDataCollection.Add(new CounterCreationData("Items/sec received", "", PerformanceCounterType.RateOfCountsPerSecond32));
		counterCreationDataCollection.Add(new CounterCreationData("Items/sec processed", "", PerformanceCounterType.RateOfCountsPerSecond32));
		PerformanceCounterCategory.Create(CategoryName, "Category help", PerformanceCounterCategoryType.MultiInstance, counterCreationDataCollection);
	}

	public static PerformanceCounter CreateEventsReceivedCounter(string instanceName)
	{
		string instanceName2 = PerformanceCounterHelper.CreateInstanceName(instanceName);
		return new PerformanceCounter("VideoOS Admin LiveStream", "Items/sec received", instanceName2, readOnly: false);
	}

	public static PerformanceCounter CreateEventsProcessedCounter(string instanceName)
	{
		string instanceName2 = PerformanceCounterHelper.CreateInstanceName(instanceName);
		return new PerformanceCounter("VideoOS Admin LiveStream", "Items/sec processed", instanceName2, readOnly: false);
	}
}
