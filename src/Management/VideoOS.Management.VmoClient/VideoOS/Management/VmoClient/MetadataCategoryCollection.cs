using System;

namespace VideoOS.Management.VmoClient;

public class MetadataCategoryCollection : VmoObjectGuidBasedCollection<MetadataCategory>
{
	public MetadataCategoryCollection(LoadMethod loadCategories, VmoObject parent, bool isInitialized)
		: base((Func<bool, MetadataCategory[]>)loadCategories.Invoke, parent, isInitialized)
	{
	}
}
