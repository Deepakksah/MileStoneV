using System.Diagnostics;
using System.Globalization;

namespace VideoOS.Vms.Utilities.Diagnostics;

public sealed class RecorderDatabasePerformance : PerformanceCounterCategoryBase
{
	private const string Category = "VideoOS Recording Server Database";

	private const string CategoryHelp = "Category help";

	private const string CounterFPS = "Media/sec";

	private const string CounterBps = "Bytes/sec";

	private const string CounterAvgGopWriteTime = "Average Gop Write Time (ms)";

	private const string CounterAvgGopWriteTimeBase = "Average Gop Write Time Base (ms)";

	private const string TotalInstanceName = "_Total";

	public static int RecorderInstance { get; set; }

	public override string CategoryName => "VideoOS Recording Server Database";

	public override void CreateCategory()
	{
		CounterCreationDataCollection counterCreationDataCollection = new CounterCreationDataCollection();
		counterCreationDataCollection.Add(new CounterCreationData("Media/sec", "", PerformanceCounterType.RateOfCountsPerSecond32));
		counterCreationDataCollection.Add(new CounterCreationData("Bytes/sec", "", PerformanceCounterType.RateOfCountsPerSecond64));
		counterCreationDataCollection.Add(new CounterCreationData("Average Gop Write Time (ms)", "", PerformanceCounterType.AverageTimer32));
		counterCreationDataCollection.Add(new CounterCreationData("Average Gop Write Time Base (ms)", "", PerformanceCounterType.AverageBase));
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
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Database", "Media/sec", instanceName);
	}

	public static PerformanceCounterHelper CreateFpsCounter(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Database", "Media/sec", instanceName);
	}

	public static PerformanceCounterHelper CreateBytesPerSecondCounterTotal()
	{
		string instanceName = "_Total";
		if (RecorderInstance != 0)
		{
			instanceName = string.Format(CultureInfo.InvariantCulture, "{0} ({1})", "_Total", RecorderInstance);
		}
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Database", "Bytes/sec", instanceName);
	}

	public static PerformanceCounterHelper CreateBytesPerSecondCounter(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Database", "Bytes/sec", instanceName);
	}

	public static PerformanceCounterHelper CreateAverageGopWriteTimeCounterTotal()
	{
		string instanceName = "_Total";
		if (RecorderInstance != 0)
		{
			instanceName = string.Format(CultureInfo.InvariantCulture, "{0} ({1})", "_Total", RecorderInstance);
		}
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Database", "Average Gop Write Time (ms)", instanceName);
	}

	public static PerformanceCounterHelper CreateAverageGopWriteTimeCounter(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Database", "Average Gop Write Time (ms)", instanceName, 0L);
	}

	public static PerformanceCounterHelper CreateAverageGopWriteTimeBaseTotal()
	{
		string instanceName = "_Total";
		if (RecorderInstance != 0)
		{
			instanceName = string.Format(CultureInfo.InvariantCulture, "{0} ({1})", "_Total", RecorderInstance);
		}
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Database", "Average Gop Write Time Base (ms)", instanceName);
	}

	public static PerformanceCounterHelper CreateAverageGopWriteTimeBase(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Database", "Average Gop Write Time Base (ms)", instanceName, 0L);
	}
}
