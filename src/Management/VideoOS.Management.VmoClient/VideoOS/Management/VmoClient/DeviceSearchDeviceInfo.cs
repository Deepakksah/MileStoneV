using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

public abstract class DeviceSearchDeviceInfo
{
	public Guid Id { get; }

	public string Name { get; }

	public Guid HardwareId { get; }

	public abstract DeviceType DeviceType { get; }

	protected DeviceSearchDeviceInfo(Guid id, string name, Guid hardwareId)
	{
		Id = id;
		Name = name;
		HardwareId = hardwareId;
	}
}
