using System;

namespace VideoOS.Management.VmoClient;

public class AxisTunnelServerCollection : VmoObjectIntBasedCollection<AxisTunnelServer>
{
	public AxisTunnelServerCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, AxisTunnelServer[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
