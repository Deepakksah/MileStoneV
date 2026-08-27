using System;

namespace VideoOS.Management.VmoClient;

public class MetadataFilterCollection : VmoObjectGuidBasedCollection<MetadataFilter>
{
	public MetadataFilterCollection(LoadMethod loadFilters, VmoObject parent, bool isInitialized)
		: base((Func<bool, MetadataFilter[]>)loadFilters.Invoke, parent, isInitialized)
	{
	}
}
