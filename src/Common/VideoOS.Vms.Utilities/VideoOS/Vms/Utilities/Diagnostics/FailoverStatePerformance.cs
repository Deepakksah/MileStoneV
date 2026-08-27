using System;
using System.Diagnostics;

namespace VideoOS.Vms.Utilities.Diagnostics;

public sealed class FailoverStatePerformance : PerformanceCounterCategoryBase
{
	private const string Category = "VideoOS Failover State";

	private const string CategoryHelp = "Category help";

	private const string FailoverStarted = "Started";

	private const string FailoverErrorState = "Error State";

	public override string CategoryName => "VideoOS Failover State";

	public override void CreateCategory()
	{
		CounterCreationDataCollection counterData = new CounterCreationDataCollection(new CounterCreationData[2]
		{
			new CounterCreationData("Started", "Determines if failover started", PerformanceCounterType.NumberOfItems64),
			new CounterCreationData("Error State", "Shows error state of the failover server", PerformanceCounterType.NumberOfItems64)
		});
		PerformanceCounterCategory.Create(CategoryName, "Category help", PerformanceCounterCategoryType.MultiInstance, counterData);
		SetLargeFileMappingSize();
	}

	public static PerformanceCounterHelper CreateFailoverStartedCounter(string instanceName, Guid failoverId)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Failover State", "Started", instanceName, failoverId);
	}

	public static PerformanceCounterHelper CreateFailoverErrorStateCounter(string instanceName, Guid failoverId)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Failover State", "Error State", instanceName, failoverId);
	}
}
