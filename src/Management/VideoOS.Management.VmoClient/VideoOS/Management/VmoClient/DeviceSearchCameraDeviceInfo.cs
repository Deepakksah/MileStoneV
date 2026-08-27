using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

public class DeviceSearchCameraDeviceInfo : DeviceSearchDeviceInfo
{
	public override DeviceType DeviceType => DeviceType.Camera;

	public DeviceSearchCameraDeviceInfo(Guid id, string name, Guid hardwareId)
		: base(id, name, hardwareId)
	{
	}
}
