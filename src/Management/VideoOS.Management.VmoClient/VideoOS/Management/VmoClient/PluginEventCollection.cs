using System;

namespace VideoOS.Management.VmoClient;

public class PluginEventCollection : VmoObjectGuidBasedCollection<PluginEvent>
{
	public PluginEventCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, PluginEvent[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
