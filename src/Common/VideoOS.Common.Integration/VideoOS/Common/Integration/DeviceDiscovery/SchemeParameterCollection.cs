using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class SchemeParameterCollection : Collection<SchemeParameter>
{
	public void Add(string name, string value)
	{
		Add(new SchemeParameter(name, value));
	}
}
