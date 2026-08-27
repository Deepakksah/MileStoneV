using VideoOS.Common.Integration;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface IDeviceProxy
{
	Device QueryDeviceById(IGeminiRequestContext requestContext, string id);

	bool DeviceExists(IGeminiRequestContext requestContext, string id);

	Device[] QueryDevicesByIds(IGeminiRequestContext requestContext, string[] ids);

	DeviceGroup QueryDeviceGroupById(IGeminiRequestContext requestContext, string id);

	bool DeviceGroupExists(IGeminiRequestContext requestContext, string id);

	DeviceGroup[] QueryDeviceGroups(IGeminiRequestContext requestContext, string[] ids);

	PtzPresetCollection QueryDevicePtzPresets(IGeminiRequestContext requestContext, string id);

	string[] QueryDeviceIdsByDeviceGroup(IGeminiRequestContext requestContext, string deviceGroupId, DeviceTypeSearch deviceTypeSearch);
}
