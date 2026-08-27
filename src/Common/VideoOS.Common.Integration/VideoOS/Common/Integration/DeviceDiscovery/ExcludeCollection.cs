using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class ExcludeCollection : Collection<Exclude>
{
	public void Add(string ipAddress)
	{
		Add(new Exclude(ipAddress));
	}

	public void Add(string ipAddress, ushort port)
	{
		Add(new Exclude(ipAddress, port));
	}
}
