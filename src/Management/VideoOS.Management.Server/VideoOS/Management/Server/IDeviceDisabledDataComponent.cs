using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IDeviceDisabledDataComponent : IDisposable
{
	IEnumerable<DeviceDisabledData> QueryDisabledDevices();
}
