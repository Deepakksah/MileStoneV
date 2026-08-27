using System;
using System.Diagnostics;
using System.Globalization;

namespace VideoOS.Vms.Utilities.Diagnostics;

public sealed class PipelinePerformance : PerformanceCounterCategoryBase
{
	private const string Category = "VideoOS Recording Server Pipeline";

	private const string CategoryHelp = "Category help";

	private const string CounterFPSPart1 = "Media/sec before queue";

	private const string CounterKeyFPSPart1 = "KeyFrames/sec before queue";

	private const string CounterFPSPart2 = "Media/sec after queue";

	private const string CounterAvgLiveFeedConsumer = "Average Live Feed Processing Time (ms)";

	private const string CounterAvgLiveFeedConsumerBase = "Average Live Feed Processing Time Base (ms)";

	private const string CounterAvgPlugins = "Average Plugins Processing Time (ms)";

	private const string CounterAvgPluginsBase = "Average Plugins Processing Time Base (ms)";

	private const string CounterAvgGOPConsumer = "Average GOP Consumer Processing Time (ms)";

	private const string CounterAvgGOPConsumerBase = "Average GOP Consumer Processing Time Base (ms)";

	private const string CounterAvgFrameSize = "Average Frame Size (bytes)";

	private const string CounterAvgFrameSizeBase = "Average Frame Size Base (bytes)";

	private const string CounterLostFPS = "Medias lost/sec";

	private const string CounterQueueSize = "Medias in queue";

	private const string TotalInstanceName = "_Total";

	public static int RecorderInstance { get; set; }

	public override string CategoryName => "VideoOS Recording Server Pipeline";

	public override void CreateCategory()
	{
		CounterCreationDataCollection counterCreationDataCollection = new CounterCreationDataCollection();
		counterCreationDataCollection.Add(new CounterCreationData("Media/sec before queue", "", PerformanceCounterType.RateOfCountsPerSecond32));
		counterCreationDataCollection.Add(new CounterCreationData("KeyFrames/sec before queue", "", PerformanceCounterType.RateOfCountsPerSecond32));
		counterCreationDataCollection.Add(new CounterCreationData("Media/sec after queue", "", PerformanceCounterType.RateOfCountsPerSecond32));
		counterCreationDataCollection.Add(new CounterCreationData("Average Plugins Processing Time (ms)", "", PerformanceCounterType.AverageTimer32));
		counterCreationDataCollection.Add(new CounterCreationData("Average Plugins Processing Time Base (ms)", "", PerformanceCounterType.AverageBase));
		counterCreationDataCollection.Add(new CounterCreationData("Average Live Feed Processing Time (ms)", "", PerformanceCounterType.AverageTimer32));
		counterCreationDataCollection.Add(new CounterCreationData("Average Live Feed Processing Time Base (ms)", "", PerformanceCounterType.AverageBase));
		counterCreationDataCollection.Add(new CounterCreationData("Average GOP Consumer Processing Time (ms)", "", PerformanceCounterType.AverageTimer32));
		counterCreationDataCollection.Add(new CounterCreationData("Average GOP Consumer Processing Time Base (ms)", "", PerformanceCounterType.AverageBase));
		counterCreationDataCollection.Add(new CounterCreationData("Average Frame Size (bytes)", "", PerformanceCounterType.AverageCount64));
		counterCreationDataCollection.Add(new CounterCreationData("Average Frame Size Base (bytes)", "", PerformanceCounterType.AverageBase));
		counterCreationDataCollection.Add(new CounterCreationData("Medias lost/sec", "", PerformanceCounterType.RateOfCountsPerSecond32));
		counterCreationDataCollection.Add(new CounterCreationData("Medias in queue", "", PerformanceCounterType.NumberOfItems32));
		PerformanceCounterCategory.Create(CategoryName, "Category help", PerformanceCounterCategoryType.MultiInstance, counterCreationDataCollection);
		SetLargeFileMappingSize();
	}

	public static PerformanceCounterHelper CreateFpsCounterPart1(string deviceName, string streamName, Guid deviceId)
	{
		string instanceName = string.Format(CultureInfo.InvariantCulture, "{0} [{1} - {2}]", deviceName, streamName, deviceId);
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Pipeline", "Media/sec before queue", instanceName);
	}

	public static PerformanceCounterHelper CreateKeyFpsCounterPart1(string deviceName, string streamName, Guid deviceId)
	{
		string instanceName = string.Format(CultureInfo.InvariantCulture, "{0} [{1} - {2}]", deviceName, streamName, deviceId);
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Pipeline", "KeyFrames/sec before queue", instanceName);
	}

	public static PerformanceCounterHelper CreateFpsCounterPart2(string deviceName, string streamName, Guid deviceId)
	{
		string instanceName = string.Format(CultureInfo.InvariantCulture, "{0} [{1} - {2}]", deviceName, streamName, deviceId);
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Pipeline", "Media/sec after queue", instanceName, 0L);
	}

	public static PerformanceCounterHelper CreateAveragePlugins(string deviceName, string streamName, Guid deviceId)
	{
		string instanceName = string.Format(CultureInfo.InvariantCulture, "{0} [{1} - {2}]", deviceName, streamName, deviceId);
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Pipeline", "Average Plugins Processing Time (ms)", instanceName, 0L);
	}

	public static PerformanceCounterHelper CreateAverageLiveFeedConsumer(string deviceName, string streamName, Guid deviceId)
	{
		string instanceName = string.Format(CultureInfo.InvariantCulture, "{0} [{1} - {2}]", deviceName, streamName, deviceId);
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Pipeline", "Average Live Feed Processing Time (ms)", instanceName, 0L);
	}

	public static PerformanceCounterHelper CreateAverageGobConsumer(string deviceName, string streamName, Guid deviceId)
	{
		string instanceName = string.Format(CultureInfo.InvariantCulture, "{0} [{1} - {2}]", deviceName, streamName, deviceId);
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Pipeline", "Average GOP Consumer Processing Time (ms)", instanceName, 0L);
	}

	public static PerformanceCounterHelper CreateAverageFrameSize(string deviceName, string streamName, Guid deviceId)
	{
		string instanceName = string.Format(CultureInfo.InvariantCulture, "{0} [{1} - {2}]", deviceName, streamName, deviceId);
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Pipeline", "Average Frame Size (bytes)", instanceName, 0L);
	}

	public static PerformanceCounterHelper CreateAveragePluginsBase(string deviceName, string streamName, Guid deviceId)
	{
		string instanceName = string.Format(CultureInfo.InvariantCulture, "{0} [{1} - {2}]", deviceName, streamName, deviceId);
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Pipeline", "Average Plugins Processing Time Base (ms)", instanceName, 0L);
	}

	public static PerformanceCounterHelper CreateAverageLiveFeedConsumerBase(string deviceName, string streamName, Guid deviceId)
	{
		string instanceName = string.Format(CultureInfo.InvariantCulture, "{0} [{1} - {2}]", deviceName, streamName, deviceId);
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Pipeline", "Average Live Feed Processing Time Base (ms)", instanceName, 0L);
	}

	public static PerformanceCounterHelper CreateAverageGobConsumerBase(string deviceName, string streamName, Guid deviceId)
	{
		string instanceName = string.Format(CultureInfo.InvariantCulture, "{0} [{1} - {2}]", deviceName, streamName, deviceId);
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Pipeline", "Average GOP Consumer Processing Time Base (ms)", instanceName, 0L);
	}

	public static PerformanceCounterHelper CreateAverageFrameSizeBase(string deviceName, string streamName, Guid deviceId)
	{
		string instanceName = string.Format(CultureInfo.InvariantCulture, "{0} [{1} - {2}]", deviceName, streamName, deviceId);
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Pipeline", "Average Frame Size Base (bytes)", instanceName, 0L);
	}

	public static PerformanceCounterHelper CreateLostFpsCounterTotal()
	{
		string instanceName = "_Total";
		if (RecorderInstance != 0)
		{
			instanceName = string.Format(CultureInfo.InvariantCulture, "{0} ({1})", "_Total", RecorderInstance);
		}
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Pipeline", "Medias lost/sec", instanceName);
	}

	public static PerformanceCounterHelper CreateLostFpsCounter(string deviceName, string streamName, Guid deviceId)
	{
		string instanceName = string.Format(CultureInfo.InvariantCulture, "{0} [{1} - {2}]", deviceName, streamName, deviceId);
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Pipeline", "Medias lost/sec", instanceName);
	}

	public static PerformanceCounterHelper CreateQueueSizeCounterTotal()
	{
		string instanceName = "_Total";
		if (RecorderInstance != 0)
		{
			instanceName = string.Format(CultureInfo.InvariantCulture, "{0} ({1})", "_Total", RecorderInstance);
		}
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Pipeline", "Medias in queue", instanceName);
	}

	public static PerformanceCounterHelper CreateQueueSizeCounter(string deviceName, string streamName, Guid deviceId)
	{
		string instanceName = string.Format(CultureInfo.InvariantCulture, "{0} [{1} - {2}]", deviceName, streamName, deviceId);
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Pipeline", "Medias in queue", instanceName, 0L);
	}
}
