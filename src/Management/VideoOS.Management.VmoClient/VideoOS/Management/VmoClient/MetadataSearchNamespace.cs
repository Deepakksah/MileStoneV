using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MetadataSearchNamespace")]
[Obsolete("Metadata search namespaces are not used anymore. Query methods on this class will return null or empty arrays. We are keeping the method for backwards compatibility reasons.")]
public class MetadataSearchNamespace : VmoObjectGuidBased
{
	public string Prefix => GetPropertyValue<string>("Prefix");

	public string Uri => GetPropertyValue<string>("Uri");

	public MetadataSearchNamespace(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}
}
