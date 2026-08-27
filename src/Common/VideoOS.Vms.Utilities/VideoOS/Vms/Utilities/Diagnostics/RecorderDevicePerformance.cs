using System;
using System.Diagnostics;

namespace VideoOS.Vms.Utilities.Diagnostics;

public sealed class RecorderDevicePerformance : PerformanceCounterCategoryBase
{
	private const string Category = "VideoOS Recording Server Device";

	private const string CategoryHelp = "Category help";

	private const string CounterRecordingFpsPercentage = "Media/sec perc";

	private const string CounterRecordingFpsPercentageHelp = "Shows the percentage of recorded frames based on the expected framerate from the camera and the potential sub-sampling. If more than one stream is recording for a device, the value will be the lowest of the values of all the recording streams. ";

	private const string CounterDeviceState = "State";

	private const string CounterDeviceStateHelp = "Performance counter reflecting the device state. Possible values are: 0 (Unknown); 1 (Started); 2 (Stopped); 3 (DbRepair); 4 (Overflow); 5 (WriteDbError); 6 (LicenseError); 7 (ConnectionError)";

	public override string CategoryName => "VideoOS Recording Server Device";

	public override void CreateCategory()
	{
		CounterCreationDataCollection counterData = new CounterCreationDataCollection
		{
			new CounterCreationData("Media/sec perc", "Shows the percentage of recorded frames based on the expected framerate from the camera and the potential sub-sampling. If more than one stream is recording for a device, the value will be the lowest of the values of all the recording streams. ", PerformanceCounterType.NumberOfItems64),
			new CounterCreationData("State", "Performance counter reflecting the device state. Possible values are: 0 (Unknown); 1 (Started); 2 (Stopped); 3 (DbRepair); 4 (Overflow); 5 (WriteDbError); 6 (LicenseError); 7 (ConnectionError)", PerformanceCounterType.NumberOfItems64)
		};
		PerformanceCounterCategory.Create(CategoryName, "Category help", PerformanceCounterCategoryType.MultiInstance, counterData);
		SetLargeFileMappingSize();
	}

	public static PerformanceCounterHelper CreateRecordingFpsPercentageCounter(string instanceName, Guid deviceId)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Device", "Media/sec perc", instanceName, 100L, deviceId);
	}

	public static PerformanceCounterHelper CreateDeviceStatusCounter(string instanceName, Guid deviceId)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Device", "State", instanceName, deviceId);
	}
}
