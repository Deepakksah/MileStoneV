using System;
using System.Diagnostics;

namespace VideoOS.Vms.Utilities.Diagnostics;

public sealed class ArchiveStoragePerformance : PerformanceCounterCategoryBase
{
	private const string Category = "VideoOS Recording Server Storage Archive";

	private const string CategoryHelp = "Category help";

	private const string ArchiveAvailable = "ArchiveAvailable";

	public override string CategoryName => "VideoOS Recording Server Storage Archive";

	public override void CreateCategory()
	{
		CounterCreationDataCollection counterData = new CounterCreationDataCollection(new CounterCreationData[1]
		{
			new CounterCreationData("ArchiveAvailable", "Determines if archive is available", PerformanceCounterType.NumberOfItems64)
		});
		PerformanceCounterCategory.Create(CategoryName, "Category help", PerformanceCounterCategoryType.MultiInstance, counterData);
		SetLargeFileMappingSize();
	}

	public static PerformanceCounterHelper CreateArchiveAvailableCounter(string instanceName, Guid deviceId)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Storage Archive", "ArchiveAvailable", instanceName, deviceId);
	}
}
