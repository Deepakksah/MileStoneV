using System;
using System.Diagnostics;

namespace VideoOS.Vms.Utilities.Diagnostics;

public sealed class MulticastPerformance : PerformanceCounterCategoryBase
{
	private const string Category = "VideoOS Recording Server Live Multicast";

	private const string CategoryHelp = "Category help";

	private const string CounterFPS = "Frames Sent/sec";

	private const string CounterPPS = "Packets Sent/sec";

	private const string CounterAvgSendFrame = "Average Send Frame Processing Time (ms)";

	private const string CounterAvgSendFrameBase = "Average Send Frame Processing Time Base (ms)";

	public override string CategoryName => "VideoOS Recording Server Live Multicast";

	public override void CreateCategory()
	{
		CounterCreationDataCollection counterCreationDataCollection = new CounterCreationDataCollection();
		counterCreationDataCollection.Add(new CounterCreationData("Frames Sent/sec", "", PerformanceCounterType.RateOfCountsPerSecond32));
		counterCreationDataCollection.Add(new CounterCreationData("Packets Sent/sec", "", PerformanceCounterType.RateOfCountsPerSecond32));
		counterCreationDataCollection.Add(new CounterCreationData("Average Send Frame Processing Time (ms)", "", PerformanceCounterType.AverageTimer32));
		counterCreationDataCollection.Add(new CounterCreationData("Average Send Frame Processing Time Base (ms)", "", PerformanceCounterType.AverageBase));
		PerformanceCounterCategory.Create(CategoryName, "Category help", PerformanceCounterCategoryType.MultiInstance, counterCreationDataCollection);
		SetLargeFileMappingSize();
	}

	public static PerformanceCounterHelper CreateFpsCounter(string deviceName, Guid deviceId)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Live Multicast", "Frames Sent/sec", deviceName, deviceId);
	}

	public static PerformanceCounterHelper CreatePackagePerSecondCounter(string deviceName, Guid deviceId)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Live Multicast", "Packets Sent/sec", deviceName, deviceId);
	}

	public static PerformanceCounterHelper CreateAverageSendFrame(string deviceName, Guid deviceId)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Live Multicast", "Average Send Frame Processing Time (ms)", deviceName, 0L, deviceId);
	}

	public static PerformanceCounterHelper CreateAverageSendFrameBase(string deviceName, Guid deviceId)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Live Multicast", "Average Send Frame Processing Time Base (ms)", deviceName, 0L, deviceId);
	}
}
