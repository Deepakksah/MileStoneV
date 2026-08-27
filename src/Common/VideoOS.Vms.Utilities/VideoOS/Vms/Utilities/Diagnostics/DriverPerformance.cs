using System;
using System.Diagnostics;
using System.Globalization;

namespace VideoOS.Vms.Utilities.Diagnostics;

public sealed class DriverPerformance : PerformanceCounterCategoryBase
{
	private const string Category = "VideoOS Recording Server Drivers";

	private const string CategoryHelp = "Category help";

	private const string CounterFPS = "Media/sec";

	private const string CounterFPSPercent = "Media/sec perc";

	private const string CounterBps = "Bytes/sec";

	private const string TotalInstanceName = "_Total";

	public static int RecorderInstance { get; set; }

	public override string CategoryName => "VideoOS Recording Server Drivers";

	public override void CreateCategory()
	{
		CounterCreationDataCollection counterCreationDataCollection = new CounterCreationDataCollection();
		counterCreationDataCollection.Add(new CounterCreationData("Media/sec", "", PerformanceCounterType.RateOfCountsPerSecond32));
		counterCreationDataCollection.Add(new CounterCreationData("Bytes/sec", "", PerformanceCounterType.RateOfCountsPerSecond64));
		counterCreationDataCollection.Add(new CounterCreationData("Media/sec perc", "", PerformanceCounterType.RateOfCountsPerSecond32));
		PerformanceCounterCategory.Create(CategoryName, "Category help", PerformanceCounterCategoryType.MultiInstance, counterCreationDataCollection);
		SetLargeFileMappingSize();
	}

	public static PerformanceCounterHelper CreateFpsCounterTotal()
	{
		string instanceName = "_Total";
		if (RecorderInstance != 0)
		{
			instanceName = string.Format(CultureInfo.InvariantCulture, "{0} ({1})", "_Total", RecorderInstance);
		}
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Drivers", "Media/sec", instanceName);
	}

	public static PerformanceCounterHelper CreateFpsCounter(string deviceName, string streamName, Guid deviceId)
	{
		string instanceName = string.Format(CultureInfo.InvariantCulture, "{0} [{1} - {2}]", deviceName, streamName, deviceId);
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Drivers", "Media/sec", instanceName);
	}

	public static PerformanceCounterHelper CreateFpsPercentCounter(string deviceName, Guid deviceId)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Drivers", "Media/sec perc", deviceName, deviceId);
	}

	public static PerformanceCounterHelper CreateFpsCounter(string deviceName, Guid deviceId)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Drivers", "Media/sec", deviceName, deviceId);
	}

	public static PerformanceCounterHelper CreateBpsCounter(string deviceName, string streamName, Guid deviceId)
	{
		string instanceName = string.Format(CultureInfo.InvariantCulture, "{0} [{1} - {2}]", deviceName, streamName, deviceId);
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Drivers", "Bytes/sec", instanceName);
	}

	public static PerformanceCounterHelper CreateBpsCounterTotal()
	{
		string instanceName = "_Total";
		if (RecorderInstance != 0)
		{
			instanceName = string.Format(CultureInfo.InvariantCulture, "{0} ({1})", "_Total", RecorderInstance);
		}
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Drivers", "Bytes/sec", instanceName);
	}

	public static PerformanceCounterHelper CreateBpsCounter(string deviceName, Guid deviceId)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Drivers", "Bytes/sec", deviceName, deviceId);
	}
}
