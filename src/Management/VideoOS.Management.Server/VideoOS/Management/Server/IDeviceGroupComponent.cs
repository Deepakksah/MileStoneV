using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public interface IDeviceGroupComponent : IDisposable
{
	void AddDeviceGroupMembers(string deviceGroupId, string[] deviceIds);

	void CreateDeviceGroup(string id, string parentId, DeviceType deviceType, string name, string description);

	void DeleteDeviceGroup(string id);

	DeviceGroup QueryDeviceGroup(string deviceGroupid);

	string[] QueryDeviceGroupIds(string parentGroupId, DeviceType type);

	DeviceGroup[] QueryDeviceGroups(string[] ids);

	string[] QueryDeviceGroupMembers(string[] deviceGroupIds);

	void RemoveDeviceGroupMembers(string deviceGroupId, string[] deviceIds);

	void UpdateDeviceGroup(string id, string name, string description);

	DeviceGroup[] QueryDeviceGroups(DeviceType type);

	DeviceGroup QueryDeviceGroupNonAdmin(string deviceGroupid, DeviceType type);

	List<Tuple<Guid, Guid>> QueryDeviceGroupParentAndGroupIdsNonAdmin(string parentGroupId, DeviceType type);
}
