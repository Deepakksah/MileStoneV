using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class SchemeCollection : Collection<Scheme>
{
	public void AddRange(IEnumerable<Scheme> schemes)
	{
		if (schemes == null)
		{
			return;
		}
		foreach (Scheme scheme in schemes)
		{
			Add(scheme);
		}
	}
}
