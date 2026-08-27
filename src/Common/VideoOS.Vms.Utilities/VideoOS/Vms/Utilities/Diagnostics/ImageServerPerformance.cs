using System.Diagnostics;

namespace VideoOS.Vms.Utilities.Diagnostics;

public sealed class ImageServerPerformance : PerformanceCounterCategoryBase
{
	private const string Category = "VideoOS Recording Server ImageServer";

	private const string CategoryHelp = "Category help";

	private const string CounterFramesPerSecondLiveFeed = "Frame/sec live feed";

	private const string CounteBytesPerSecondLiveFeed = "Bytes/sec live feed";

	private const string CounterFPSHandleGoto = "Frame/sec goto";

	private const string CounterGOPHandleGoto = "GOP/sec goto";

	private const string CounterBytesPerSecondHandleGoto = "Bytes/sec goto";

	private const string CounterAverageHandleGotoFeed = "Average Goto Feed Processing Time (ms)";

	private const string CounterAverageHandleGotoFeedBase = "Average Goto Feed Processing Time (ms) Base";

	public override string CategoryName => "VideoOS Recording Server ImageServer";

	public override void CreateCategory()
	{
		CounterCreationDataCollection counterCreationDataCollection = new CounterCreationDataCollection();
		counterCreationDataCollection.Add(new CounterCreationData("Frame/sec live feed", "", PerformanceCounterType.RateOfCountsPerSecond32));
		counterCreationDataCollection.Add(new CounterCreationData("Bytes/sec live feed", "", PerformanceCounterType.RateOfCountsPerSecond64));
		counterCreationDataCollection.Add(new CounterCreationData("Frame/sec goto", "", PerformanceCounterType.RateOfCountsPerSecond32));
		counterCreationDataCollection.Add(new CounterCreationData("GOP/sec goto", "", PerformanceCounterType.RateOfCountsPerSecond32));
		counterCreationDataCollection.Add(new CounterCreationData("Bytes/sec goto", "", PerformanceCounterType.RateOfCountsPerSecond64));
		counterCreationDataCollection.Add(new CounterCreationData("Average Goto Feed Processing Time (ms)", "", PerformanceCounterType.AverageTimer32));
		counterCreationDataCollection.Add(new CounterCreationData("Average Goto Feed Processing Time (ms) Base", "", PerformanceCounterType.AverageBase));
		PerformanceCounterCategory.Create(CategoryName, "Category help", PerformanceCounterCategoryType.MultiInstance, counterCreationDataCollection);
	}

	public static PerformanceCounterHelper CreateFPSLiveFeedCounter(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server ImageServer", "Frame/sec live feed", instanceName);
	}

	public static PerformanceCounterHelper CreateBPSLiveFeedCounter(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server ImageServer", "Bytes/sec live feed", instanceName);
	}

	public static PerformanceCounterHelper CreateFPSHandleGotoCounter(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server ImageServer", "Frame/sec goto", instanceName);
	}

	public static PerformanceCounterHelper CreateGOPHandleGotoCounter(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server ImageServer", "GOP/sec goto", instanceName);
	}

	public static PerformanceCounterHelper CreateBPSHandleGotoCounter(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server ImageServer", "Bytes/sec goto", instanceName);
	}

	public static PerformanceCounterHelper CreateAverageHandleGotoFeed(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server ImageServer", "Average Goto Feed Processing Time (ms)", instanceName, 0L);
	}

	public static PerformanceCounterHelper CreateAverageHandleGotoFeedBase(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server ImageServer", "Average Goto Feed Processing Time (ms) Base", instanceName, 0L);
	}
}
