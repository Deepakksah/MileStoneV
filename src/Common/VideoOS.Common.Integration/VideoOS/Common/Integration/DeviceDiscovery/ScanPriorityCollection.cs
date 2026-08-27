using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class ScanPriorityCollection : Collection<ScanPriority>
{
	public void Add(int driverNumber, int priority)
	{
		Add(new ScanPriority(driverNumber, priority));
	}
}
