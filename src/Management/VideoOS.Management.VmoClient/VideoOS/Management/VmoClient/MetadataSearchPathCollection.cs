using System;

namespace VideoOS.Management.VmoClient;

public class MetadataSearchPathCollection : VmoObjectGuidBasedCollection<MetadataSearchPath>
{
	public MetadataSearchPathCollection(LoadMethod loadSearchPaths, VmoObject parent, bool isInitialized)
		: base((Func<bool, MetadataSearchPath[]>)loadSearchPaths.Invoke, parent, isInitialized)
	{
	}
}
