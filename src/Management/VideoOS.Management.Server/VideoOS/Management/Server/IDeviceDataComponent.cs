using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IDeviceDataComponent : IDisposable
{
	IEnumerable<DeviceData> QueryEnabledDevicesByRecorderAdmin(Guid id);

	Dictionary<Guid, IEnumerable<DeviceData>> QueryEnabledDevicesByRoleId(string[] roleIds);

	IEnumerable<Guid> QueryDeviceIdsByHardware(Guid id);
}
