using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MetadataCategory")]
public class MetadataCategory : VmoObjectGuidBased, IUpdatable
{
	public MetadataFilterCollection MetadataFilters { get; private set; }

	public MetadataClassCandidateCollection MetadataClassCandidates { get; private set; }

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

	public MetadataCategory(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		MetadataFilters = new MetadataFilterCollection(LoadFilters, this, isInitialized);
		MetadataClassCandidates = new MetadataClassCandidateCollection(LoadClassCandidates, this, isInitialized);
	}

	private MetadataFilter[] LoadFilters(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<MetadataCategory, MetadataFilter>(base.Id, refreshCachedObjects);
	}

	private MetadataClassCandidate[] LoadClassCandidates(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<MetadataCategory, MetadataClassCandidate>(base.Id, refreshCachedObjects);
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
