using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IDeviceMappingInfo
{
	Guid DeviceA { get; set; }

	Guid DeviceB { get; set; }
}
