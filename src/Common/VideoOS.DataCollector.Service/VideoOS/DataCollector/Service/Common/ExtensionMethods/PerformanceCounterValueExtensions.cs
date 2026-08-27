using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.Common.ExtensionMethods;

public static class PerformanceCounterValueExtensions
{
	public static PerformanceCounterValue FormatInstanceNameWithId(this PerformanceCounterValue performanceCounterValue)
	{
		performanceCounterValue.InstanceName = (PerformanceCounterInstance.InstanceNameContainsId(performanceCounterValue.InstanceName, out var id) ? ("[" + id.ToString() + "]") : performanceCounterValue.InstanceName);
		return performanceCounterValue;
	}
}
