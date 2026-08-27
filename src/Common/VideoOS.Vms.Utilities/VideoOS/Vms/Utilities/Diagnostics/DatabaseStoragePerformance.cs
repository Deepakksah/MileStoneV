using System;
using System.Diagnostics;

namespace VideoOS.Vms.Utilities.Diagnostics;

public sealed class DatabaseStoragePerformance : PerformanceCounterCategoryBase
{
	private const string Category = "VideoOS Recording Server Storage Database";

	private const string CategoryHelp = "Category help";

	private const string DatabaseAvailable = "DatabaseAvailable";

	public override string CategoryName => "VideoOS Recording Server Storage Database";

	public override void CreateCategory()
	{
		CounterCreationDataCollection counterData = new CounterCreationDataCollection(new CounterCreationData[1]
		{
			new CounterCreationData("DatabaseAvailable", "Determines if database is available", PerformanceCounterType.NumberOfItems64)
		});
		PerformanceCounterCategory.Create(CategoryName, "Category help", PerformanceCounterCategoryType.MultiInstance, counterData);
		SetLargeFileMappingSize();
	}

	public static PerformanceCounterHelper CreateDatabaseAvailableCounter(string instanceName, Guid deviceId)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server Storage Database", "DatabaseAvailable", instanceName, deviceId);
	}
}
