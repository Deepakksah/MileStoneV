using System;

namespace VideoOS.Management.VmoClient;

public class MetadataPropertyCollection : VmoObjectGuidBasedCollection<MetadataProperty>
{
	public MetadataPropertyCollection(LoadMethod loadProperties, VmoObject parent, bool isInitialized)
		: base((Func<bool, MetadataProperty[]>)loadProperties.Invoke, parent, isInitialized)
	{
	}
}
