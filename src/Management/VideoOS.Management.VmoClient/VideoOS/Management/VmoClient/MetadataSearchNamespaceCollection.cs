using System;

namespace VideoOS.Management.VmoClient;

[Obsolete("Metadata search namespaces are not used anymore. Query methods on this class will return null or empty arrays. We are keeping the method for backwards compatibility reasons.")]
public class MetadataSearchNamespaceCollection : VmoObjectGuidBasedCollection<MetadataSearchNamespace>
{
	public MetadataSearchNamespaceCollection(LoadMethod loadNamespaces, VmoObject parent, bool isInitialized)
		: base((Func<bool, MetadataSearchNamespace[]>)loadNamespaces.Invoke, parent, isInitialized)
	{
	}
}
