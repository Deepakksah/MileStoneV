using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MicrophoneDeviceGroup")]
public class MicrophoneDeviceGroup : DeviceGroup
{
	public MicrophoneDeviceGroupCollection MicrophoneDeviceGroupMembers { get; private set; }

	public MicrophoneDeviceCollection MicrophoneDeviceMembers { get; private set; }

	public MicrophoneDeviceGroup ParentMicrophoneDeviceGroup
	{
		get
		{
			if (!string.IsNullOrEmpty(GetPropertyValue<string>("ParentGroupId")))
			{
				return GetObjectById<MicrophoneDeviceGroup>(GetPropertyValue<string>("ParentGroupId"));
			}
			return null;
		}
	}

	public MicrophoneDeviceGroup(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		MicrophoneDeviceMembers = new MicrophoneDeviceCollection(LoadDevices, this, isInitialized);
		MicrophoneDeviceGroupMembers = new MicrophoneDeviceGroupCollection(LoadDeviceGroups, this, isInitialized);
	}

	public MicrophoneDeviceGroup[] LoadDeviceGroups(bool refreshCachedObjects)
	{
		try
		{
			return base.Repositories.GetObjectsByParentId<MicrophoneDeviceGroup, MicrophoneDeviceGroup>(base.Id, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new MicrophoneDeviceGroup[0];
		}
	}

	public MicrophoneDevice[] LoadDevices(bool refreshCachedObjects)
	{
		try
		{
			return base.Repositories.GetObjectsByParentId<MicrophoneDeviceGroup, MicrophoneDevice>(base.Id, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new MicrophoneDevice[0];
		}
	}
}
