using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class PortIntervalCollection : Collection<PortInterval>
{
	public void Add(ushort port)
	{
		Add(new PortInterval(port));
	}

	public void Add(ushort firstPort, ushort lastPort)
	{
		Add(new PortInterval(firstPort, lastPort));
	}
}
