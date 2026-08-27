using System;
using System.Diagnostics;

namespace VideoOS.Vms.Utilities.Diagnostics;

public sealed class StoragePerformance : PerformanceCounterCategoryBase
{
	private const string Category = "VideoOS Recording Server Storage";

	private const string CategoryHelp = "Category help";

	private const string CounterUsedBytes = "Used Bytes";

	public override string CategoryName => "VideoOS Recording Server Storage";

	public override void CreateCategory()
	{
		CounterCreationDataCollection counterData = new CounterCreationDataCollection(new CounterCreationData[1]
		{
			new CounterCreationData("Used Bytes", "Bytes used per archive storage", PerformanceCounterType.NumberOfItems64)
		});
		PerformanceCounterCategory.Create(CategoryName, "Category help", PerformanceCounterCategoryType.MultiInstance, counterData);
		SetLargeFileMappingSize();
	}

	public static PerformanceCounterHelper CreateUsedBytesCounter(string instanceName, Guid deviceId)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Storage", "Used Bytes", instanceName, deviceId);
	}
}
