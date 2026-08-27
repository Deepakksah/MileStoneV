using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("OutputDeviceGroup")]
public class OutputDeviceGroup : DeviceGroup
{
	public OutputDeviceGroupCollection OutputDeviceGroupMembers { get; private set; }

	public OutputDeviceCollection OutputDeviceMembers { get; private set; }

	public OutputDeviceGroup ParentOutputDeviceGroup
	{
		get
		{
			if (!string.IsNullOrEmpty(GetPropertyValue<string>("ParentGroupId")))
			{
				return GetObjectById<OutputDeviceGroup>(GetPropertyValue<string>("ParentGroupId"));
			}
			return null;
		}
	}

	public OutputDeviceGroup(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		OutputDeviceMembers = new OutputDeviceCollection(LoadDevices, this, isInitialized);
		OutputDeviceGroupMembers = new OutputDeviceGroupCollection(LoadDeviceGroups, this, isInitialized);
	}

	public OutputDeviceGroup[] LoadDeviceGroups(bool refreshCachedObjects)
	{
		try
		{
			return base.Repositories.GetObjectsByParentId<OutputDeviceGroup, OutputDeviceGroup>(base.Id, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new OutputDeviceGroup[0];
		}
	}

	public OutputDevice[] LoadDevices(bool refreshCachedObjects)
	{
		try
		{
			return base.Repositories.GetObjectsByParentId<OutputDeviceGroup, OutputDevice>(base.Id, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new OutputDevice[0];
		}
	}
}
