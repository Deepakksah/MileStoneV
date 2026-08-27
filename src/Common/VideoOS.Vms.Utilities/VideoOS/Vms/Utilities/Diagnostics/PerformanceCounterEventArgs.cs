using System;
using System.Collections.Generic;

namespace VideoOS.Vms.Utilities.Diagnostics;

public class PerformanceCounterEventArgs : EventArgs
{
	public string PerformanceCategoryName { get; }

	public string PerformanceCounterName { get; }

	public string PerformanceCounterInstanceName { get; }

	public IEnumerable<string> PerformanceCounterInstanceIds { get; }

	public PerformanceCounterEventArgs(string categoryName, string counterName, string instanceName, IEnumerable<string> instanceIds)
	{
		PerformanceCategoryName = categoryName;
		PerformanceCounterName = counterName;
		PerformanceCounterInstanceName = instanceName;
		PerformanceCounterInstanceIds = instanceIds;
	}
}
