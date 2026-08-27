using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public interface IDeviceGroupDataComponent : IDisposable
{
	IEnumerable<DeviceGroupData> QueryDeviceGroupsAll();

	Dictionary<Guid, IEnumerable<Guid>> QueryDeviceGroupMembersAll();

	Dictionary<Guid, HashSet<Guid>> QueryDeviceGroupMembersAllForType(DeviceType deviceType);

	Dictionary<Guid, HashSet<Guid>> QueryDeviceGroupMembersAllNonAdmin(DeviceType deviceType);
}
