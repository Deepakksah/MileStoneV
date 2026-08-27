using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MetadataProperty")]
public class MetadataProperty : VmoObjectGuidBased
{
	public string Name => GetPropertyValue<string>("Name");

	public MetadataProperty(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}
}
