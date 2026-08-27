using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

public class DeviceSearchMicrophoneDeviceInfo : DeviceSearchDeviceInfo
{
	public override DeviceType DeviceType => DeviceType.Microphone;

	public DeviceSearchMicrophoneDeviceInfo(Guid id, string name, Guid hardwareId)
		: base(id, name, hardwareId)
	{
	}
}
