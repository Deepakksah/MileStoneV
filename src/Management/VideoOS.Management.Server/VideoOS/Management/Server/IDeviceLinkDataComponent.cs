using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IDeviceLinkDataComponent : IDisposable
{
	IEnumerable<DeviceLinkData> QueryDeviceLinks();

	void AddDeviceLink(string deviceId, string relatedDeviceId);

	void RemoveDeviceLink(string deviceId, string relatedDeviceId);

	IEnumerable<DeviceLinkData> QueryDeviceLinksFromId(string deviceId);

	IEnumerable<DeviceLinkData> QueryDeviceLinksFromLinkedId(string linkedDeviceId);
}
