using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

public class DeviceSearchSpeakerDeviceInfo : DeviceSearchDeviceInfo
{
	public override DeviceType DeviceType => DeviceType.Speaker;

	public DeviceSearchSpeakerDeviceInfo(Guid id, string name, Guid hardwareId)
		: base(id, name, hardwareId)
	{
	}
}
