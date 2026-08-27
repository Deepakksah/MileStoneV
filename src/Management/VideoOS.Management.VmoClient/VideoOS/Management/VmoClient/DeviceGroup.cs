using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("DeviceGroup")]
public abstract class DeviceGroup : VmoObjectGuidBased
{
	public string Name => GetPropertyValue<string>("Name");

	public string Description => GetPropertyValue<string>("Description");

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public override bool IsCustomPropertiesSupported => false;

	public DeviceGroup(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public void Refresh()
	{
		base.Repository.RefreshObject(base.InternalId);
	}
}
