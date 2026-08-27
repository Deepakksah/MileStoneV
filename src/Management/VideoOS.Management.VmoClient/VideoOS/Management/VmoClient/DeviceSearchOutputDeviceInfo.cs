using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

public class DeviceSearchOutputDeviceInfo : DeviceSearchDeviceInfo
{
	public override DeviceType DeviceType => DeviceType.Output;

	public DeviceSearchOutputDeviceInfo(Guid id, string name, Guid hardwareId)
		: base(id, name, hardwareId)
	{
	}
}
