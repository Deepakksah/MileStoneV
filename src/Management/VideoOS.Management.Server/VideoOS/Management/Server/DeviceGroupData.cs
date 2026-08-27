using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public sealed class DeviceGroupData
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public Guid ParentDeviceGroupId { get; set; }

	public string DeviceType { get; set; }

	public static IEnumerable<DeviceGroupData> QueryDeviceGroupsAll(IGeminiRequestContext requestContext)
	{
		using IDeviceGroupDataComponent deviceGroupDataComponent = requestContext.CreateDeviceGroupDataComponent();
		return deviceGroupDataComponent.QueryDeviceGroupsAll();
	}

	public static Dictionary<Guid, IEnumerable<Guid>> QueryDeviceGroupMembers(IGeminiRequestContext requestContext)
	{
		using IDeviceGroupDataComponent deviceGroupDataComponent = requestContext.CreateDeviceGroupDataComponent();
		return deviceGroupDataComponent.QueryDeviceGroupMembersAll();
	}

	public static Dictionary<Guid, HashSet<Guid>> QueryDeviceGroupMembersNonAdmin(IGeminiRequestContext requestContext, DeviceType deviceType)
	{
		using IDeviceGroupDataComponent deviceGroupDataComponent = requestContext.CreateDeviceGroupDataComponent();
		return deviceGroupDataComponent.QueryDeviceGroupMembersAllNonAdmin(deviceType);
	}

	public static Dictionary<Guid, HashSet<Guid>> QueryDeviceGroupMembersForType(IGeminiRequestContext requestContext, DeviceType deviceType)
	{
		using IDeviceGroupDataComponent deviceGroupDataComponent = requestContext.CreateDeviceGroupDataComponent();
		return deviceGroupDataComponent.QueryDeviceGroupMembersAllForType(deviceType);
	}
}
