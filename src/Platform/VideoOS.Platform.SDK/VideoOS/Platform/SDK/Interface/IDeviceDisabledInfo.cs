using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IDeviceDisabledInfo
{
	Guid DeviceId { get; set; }

	string DeviceName { get; set; }

	string DeviceType { get; set; }

	Guid HardwareId { get; set; }

	Guid RecorderId { get; set; }
}
