using System;

namespace VideoOS.Management.VmoClient;

public abstract class VmoObjectGuidBased : VmoObject
{
	public Guid Id
	{
		get
		{
			if (GetPropertyValue<string>("Id") == null)
			{
				return Guid.Empty;
			}
			return new Guid(GetPropertyValue<string>("Id"));
		}
	}

	public VmoObjectGuidBased(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relationProperties, IVmoRepositories repositories, bool isInitialized, string customPropertyId)
		: base(internalId, objectProperties, relationProperties, repositories, customPropertyId)
	{
	}

	public VmoObjectGuidBased(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relationProperties, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relationProperties, repositories)
	{
	}
}
