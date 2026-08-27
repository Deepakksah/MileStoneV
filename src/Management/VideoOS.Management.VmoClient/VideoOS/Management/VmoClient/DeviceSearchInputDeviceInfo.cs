using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

public class DeviceSearchInputDeviceInfo : DeviceSearchDeviceInfo
{
	public override DeviceType DeviceType => DeviceType.Input;

	public DeviceSearchInputDeviceInfo(Guid id, string name, Guid hardwareId)
		: base(id, name, hardwareId)
	{
	}
}
