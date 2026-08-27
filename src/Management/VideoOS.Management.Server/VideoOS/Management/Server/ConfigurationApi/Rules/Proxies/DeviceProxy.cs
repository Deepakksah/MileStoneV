using VideoOS.Common.Integration;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class DeviceProxy : IDeviceProxy
{
	public Device QueryDeviceById(IGeminiRequestContext requestContext, string id)
	{
		return Device.QueryDeviceById(requestContext, id);
	}

	public bool DeviceExists(IGeminiRequestContext requestContext, string id)
	{
		return Device.DeviceExists(requestContext, id);
	}

	public Device[] QueryDevicesByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		return Device.QueryDevicesByIds(requestContext, ids);
	}

	public DeviceGroup QueryDeviceGroupById(IGeminiRequestContext requestContext, string id)
	{
		return DeviceGroup.QueryDeviceGroupById(requestContext, id);
	}

	public bool DeviceGroupExists(IGeminiRequestContext requestContext, string id)
	{
		return DeviceGroup.DeviceGroupExists(requestContext, id);
	}

	public DeviceGroup[] QueryDeviceGroups(IGeminiRequestContext requestContext, string[] ids)
	{
		return DeviceGroup.QueryDeviceGroups(requestContext, ids);
	}

	public PtzPresetCollection QueryDevicePtzPresets(IGeminiRequestContext requestContext, string id)
	{
		return DevicePtzPresets.QueryPtzPresets(requestContext, id);
	}

	public string[] QueryDeviceIdsByDeviceGroup(IGeminiRequestContext requestContext, string deviceGroupId, DeviceTypeSearch deviceTypeSearch)
	{
		return Device.QueryDeviceIdsByDeviceGroup(requestContext, deviceGroupId, deviceTypeSearch);
	}
}
