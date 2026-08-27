using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MetadataFilter")]
public class MetadataFilter : VmoObjectGuidBased, IUpdatable
{
	public MetadataPropertyCollection MetadataProperties { get; private set; }

	public bool Visibility
	{
		get
		{
			return GetPropertyValue<bool>("Visibility");
		}
		set
		{
			SetPropertyValue("Visibility", value);
		}
	}

	public MetadataFilter(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		MetadataProperties = new MetadataPropertyCollection(LoadProperties, this, isInitialized);
	}

	private MetadataProperty[] LoadProperties(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<MetadataFilter, MetadataProperty>(base.Id, refreshCachedObjects);
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}
}
