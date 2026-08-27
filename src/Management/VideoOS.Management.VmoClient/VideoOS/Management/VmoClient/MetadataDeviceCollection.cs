using System;

namespace VideoOS.Management.VmoClient;

public class MetadataDeviceCollection : VmoObjectGuidBasedCollection<MetadataDevice>
{
	public MetadataDeviceCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, MetadataDevice[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
