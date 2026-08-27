using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MetadataSearchPath")]
public class MetadataSearchPath : VmoObjectGuidBased
{
	public MetadataProperty Property
	{
		get
		{
			string propertyValue = GetPropertyValue<string>("PropertyId");
			return GetObjectById<MetadataProperty>(propertyValue);
		}
	}

	public string Path => GetPropertyValue<string>("SearchPath");

	public string ObjectDefinitionId => GetPropertyValue<string>("ObjectDefinitionId");

	public MetadataSearchPath(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}
}
