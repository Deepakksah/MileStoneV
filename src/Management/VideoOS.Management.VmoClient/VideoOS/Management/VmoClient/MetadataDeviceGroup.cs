using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MetadataDeviceGroup")]
public class MetadataDeviceGroup : DeviceGroup
{
	public MetadataDeviceGroupCollection MetadataDeviceGroupMembers { get; private set; }

	public MetadataDeviceCollection MetadataDeviceMembers { get; private set; }

	public MetadataDeviceGroup ParentMetadataDeviceGroup
	{
		get
		{
			if (!string.IsNullOrEmpty(GetPropertyValue<string>("ParentGroupId")))
			{
				return GetObjectById<MetadataDeviceGroup>(GetPropertyValue<string>("ParentGroupId"));
			}
			return null;
		}
	}

	public MetadataDeviceGroup(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		MetadataDeviceMembers = new MetadataDeviceCollection(LoadDevices, this, isInitialized);
		MetadataDeviceGroupMembers = new MetadataDeviceGroupCollection(LoadDeviceGroups, this, isInitialized);
	}

	public MetadataDeviceGroup[] LoadDeviceGroups(bool refreshCachedObjects)
	{
		try
		{
			return base.Repositories.GetObjectsByParentId<MetadataDeviceGroup, MetadataDeviceGroup>(base.Id, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new MetadataDeviceGroup[0];
		}
	}

	public MetadataDevice[] LoadDevices(bool refreshCachedObjects)
	{
		try
		{
			return base.Repositories.GetObjectsByParentId<MetadataDeviceGroup, MetadataDevice>(base.Id, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new MetadataDevice[0];
		}
	}
}
