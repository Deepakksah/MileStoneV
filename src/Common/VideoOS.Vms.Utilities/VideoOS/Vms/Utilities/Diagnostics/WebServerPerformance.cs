using System.Diagnostics;

namespace VideoOS.Vms.Utilities.Diagnostics;

public sealed class WebServerPerformance : PerformanceCounterCategoryBase
{
	private const string Category = "VideoOS Recording Server WebServer";

	private const string CategoryHelp = "Category help";

	private const string CounterConnections = "Connections";

	private const string CounterRequests = "Requests";

	private const string CounterConnectsOk = "Connects OK";

	private const string CounterConnectsRejected = "Connects rejected";

	public override string CategoryName => "VideoOS Recording Server WebServer";

	public override void CreateCategory()
	{
		CounterCreationDataCollection counterCreationDataCollection = new CounterCreationDataCollection();
		counterCreationDataCollection.Add(new CounterCreationData("Connections", "", PerformanceCounterType.NumberOfItems32));
		counterCreationDataCollection.Add(new CounterCreationData("Requests", "", PerformanceCounterType.RateOfCountsPerSecond32));
		counterCreationDataCollection.Add(new CounterCreationData("Connects OK", "", PerformanceCounterType.RateOfCountsPerSecond32));
		counterCreationDataCollection.Add(new CounterCreationData("Connects rejected", "", PerformanceCounterType.RateOfCountsPerSecond32));
		PerformanceCounterCategory.Create(CategoryName, "Category help", PerformanceCounterCategoryType.MultiInstance, counterCreationDataCollection);
	}

	public static PerformanceCounterHelper CreateConnectionsCounter(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server WebServer", "Connections", instanceName);
	}

	public static PerformanceCounterHelper CreateRequestsCounter(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server WebServer", "Requests", instanceName);
	}

	public static PerformanceCounterHelper CreateConnectsOkCounter(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server WebServer", "Connects OK", instanceName);
	}

	public static PerformanceCounterHelper CreateConnectsRejectedCounter(string instanceName)
	{
		return PerformanceCounterHelper.CreatePerformanceCounterHelper("VideoOS Recording Server WebServer", "Connects rejected", instanceName);
	}
}
