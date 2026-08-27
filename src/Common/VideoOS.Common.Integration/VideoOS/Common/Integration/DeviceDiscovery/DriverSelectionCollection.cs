using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class DriverSelectionCollection : Collection<DriverSelection>
{
	public void Add(int number, string name, string groupName, int useCount, bool selected, bool visible, bool newDriver)
	{
		Add(new DriverSelection(number, name, groupName, useCount, selected, visible, newDriver, string.Empty));
	}
}
