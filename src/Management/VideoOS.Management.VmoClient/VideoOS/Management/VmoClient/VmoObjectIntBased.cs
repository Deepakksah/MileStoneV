namespace VideoOS.Management.VmoClient;

public abstract class VmoObjectIntBased : VmoObject
{
	public int Id => GetPropertyValue<int>("Id");

	public override bool IsCustomPropertiesSupported => false;

	public VmoObjectIntBased(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relationProperties, IVmoRepositories repositories, bool isInitialized, string customPropertyId)
		: base(internalId, objectProperties, relationProperties, repositories, customPropertyId)
	{
	}

	public VmoObjectIntBased(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relationProperties, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relationProperties, repositories)
	{
	}
}
