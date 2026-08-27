using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("SpeakerDeviceGroup")]
public class SpeakerDeviceGroup : DeviceGroup
{
	public SpeakerDeviceGroupCollection SpeakerDeviceGroupMembers { get; private set; }

	public SpeakerDeviceCollection SpeakerDeviceMembers { get; private set; }

	public SpeakerDeviceGroup ParentSpeakerDeviceGroup
	{
		get
		{
			if (!string.IsNullOrEmpty(GetPropertyValue<string>("ParentGroupId")))
			{
				return GetObjectById<SpeakerDeviceGroup>(GetPropertyValue<string>("ParentGroupId"));
			}
			return null;
		}
	}

	public SpeakerDeviceGroup(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		SpeakerDeviceMembers = new SpeakerDeviceCollection(LoadDevices, this, isInitialized);
		SpeakerDeviceGroupMembers = new SpeakerDeviceGroupCollection(LoadDeviceGroups, this, isInitialized);
	}

	public SpeakerDeviceGroup[] LoadDeviceGroups(bool refreshCachedObjects)
	{
		try
		{
			return base.Repositories.GetObjectsByParentId<SpeakerDeviceGroup, SpeakerDeviceGroup>(base.Id, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new SpeakerDeviceGroup[0];
		}
	}

	public SpeakerDevice[] LoadDevices(bool refreshCachedObjects)
	{
		try
		{
			return base.Repositories.GetObjectsByParentId<SpeakerDeviceGroup, SpeakerDevice>(base.Id, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new SpeakerDevice[0];
		}
	}
}
