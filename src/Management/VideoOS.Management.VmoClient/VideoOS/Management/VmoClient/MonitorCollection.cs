using System;

namespace VideoOS.Management.VmoClient;

public class MonitorCollection : VmoObjectGuidBasedCollection<Monitor>
{
	public MonitorCollection(LoadMethod loadMonitors, VmoObject parent, bool isInitialized)
		: base((Func<bool, Monitor[]>)loadMonitors.Invoke, parent, isInitialized)
	{
	}
}
