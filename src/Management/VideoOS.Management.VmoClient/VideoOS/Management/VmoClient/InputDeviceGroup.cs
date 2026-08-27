using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("InputDeviceGroup")]
public class InputDeviceGroup : DeviceGroup
{
	public InputDeviceGroupCollection InputDeviceGroupMembers { get; private set; }

	public InputDeviceCollection InputDeviceMembers { get; private set; }

	public InputDeviceGroup ParentInputDeviceGroup
	{
		get
		{
			if (!string.IsNullOrEmpty(GetPropertyValue<string>("ParentGroupId")))
			{
				return GetObjectById<InputDeviceGroup>(GetPropertyValue<string>("ParentGroupId"));
			}
			return null;
		}
	}

	public InputDeviceGroup(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		InputDeviceMembers = new InputDeviceCollection(LoadDevices, this, isInitialized);
		InputDeviceGroupMembers = new InputDeviceGroupCollection(LoadDeviceGroups, this, isInitialized);
	}

	public InputDeviceGroup[] LoadDeviceGroups(bool refreshCachedObjects)
	{
		try
		{
			return base.Repositories.GetObjectsByParentId<InputDeviceGroup, InputDeviceGroup>(base.Id, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new InputDeviceGroup[0];
		}
	}

	public InputDevice[] LoadDevices(bool refreshCachedObjects)
	{
		try
		{
			return base.Repositories.GetObjectsByParentId<InputDeviceGroup, InputDevice>(base.Id, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new InputDevice[0];
		}
	}
}
