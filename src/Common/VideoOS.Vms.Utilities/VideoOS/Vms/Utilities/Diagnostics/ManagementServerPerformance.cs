using System.Diagnostics;

namespace VideoOS.Vms.Utilities.Diagnostics;

public sealed class ManagementServerPerformance : PerformanceCounterCategoryBase
{
	private const string Category = "VideoOS Management Server";

	private const string CategoryHelp = "Category help";

	private const string CounterRequests = "Requests";

	public override string CategoryName => "VideoOS Management Server";

	public override void CreateCategory()
	{
		CounterCreationDataCollection counterData = new CounterCreationDataCollection
		{
			new CounterCreationData("Requests", "", PerformanceCounterType.RateOfCountsPerSecond32)
		};
		PerformanceCounterCategory.Create(CategoryName, "Category help", PerformanceCounterCategoryType.MultiInstance, counterData);
	}

	public static PerformanceCounter CreateRequestsCounter(string instanceName)
	{
		string instanceName2 = PerformanceCounterHelper.CreateInstanceName(instanceName);
		return new PerformanceCounter("VideoOS Management Server", "Requests", instanceName2, readOnly: false);
	}
}
