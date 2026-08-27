using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MetadataClassCandidate")]
public class MetadataClassCandidate : VmoObjectGuidBased
{
	public string Name => GetPropertyValue<string>("Name");

	public MetadataClassCandidate(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}
}
