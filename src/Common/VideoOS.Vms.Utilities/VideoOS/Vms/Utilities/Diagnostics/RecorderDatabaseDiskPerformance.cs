using System.Diagnostics;

namespace VideoOS.Vms.Utilities.Diagnostics;

public sealed class RecorderDatabaseDiskPerformance : PerformanceCounterCategoryBase
{
	private const string Category = "VideoOS Recording Server Database Disk";

	private const string CategoryHelp = "Category help";

	private const string CounterMediaDataBytes = "Media Data Bytes";

	private const string CounterOtherDataBytes = "Other Data Bytes";

	private const string CounterFreeBytes = "Free Bytes";

	public override string CategoryName => "VideoOS Recording Server Database Disk";

	public override void CreateCategory()
	{
		CounterCreationDataCollection counterData = new CounterCreationDataCollection(new CounterCreationData[3]
		{
			new CounterCreationData("Media Data Bytes", "Media data usage per disk in bytes", PerformanceCounterType.NumberOfItems64),
			new CounterCreationData("Other Data Bytes", "Other data usage per disk in bytes", PerformanceCounterType.NumberOfItems64),
			new CounterCreationData("Free Bytes", "Free space in bytes", PerformanceCounterType.NumberOfItems64)
		});
		PerformanceCounterCategory.Create(CategoryName, "Category help", PerformanceCounterCategoryType.MultiInstance, counterData);
	}

	public static PerformanceCounterHelper CreateMediaDataBytesCounter(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Database Disk", "Media Data Bytes", instanceName);
	}

	public static PerformanceCounterHelper CreateOtherDataBytesCounter(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Database Disk", "Other Data Bytes", instanceName);
	}

	public static PerformanceCounterHelper CreateFreeBytesCounter(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Database Disk", "Free Bytes", instanceName);
	}
}
