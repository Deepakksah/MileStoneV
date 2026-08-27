using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MetadataObjectDefinition")]
public class MetadataObjectDefinition : VmoObjectGuidBased
{
	public MetadataSearchPathCollection SearchPaths { get; private set; }

	public string ObjectRoot => GetPropertyValue<string>("ObjectRoot");

	public MetadataObjectDefinition(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		SearchPaths = new MetadataSearchPathCollection(LoadSearchPaths, this, isInitialized);
	}

	private MetadataSearchPath[] LoadSearchPaths(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<MetadataObjectDefinition, MetadataSearchPath>(base.Id, refreshCachedObjects);
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}
}
