using System;

namespace VideoOS.Management.VmoClient;

public sealed class SystemDashboardConfigurationCollection : VmoObjectIntBasedCollection<SystemDashboardConfiguration>
{
	public SystemDashboardConfigurationCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, SystemDashboardConfiguration[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
