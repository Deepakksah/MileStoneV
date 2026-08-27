using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public sealed class DeviceDisabledData
{
	public Guid RecorderId { get; set; }

	public Guid HardwareId { get; set; }

	public Guid Id { get; set; }

	public string Type { get; set; }

	public string Name { get; set; }

	public static IEnumerable<DeviceDisabledData> QueryDisabledDevicesAdmin(IGeminiRequestContext requestContext)
	{
		using IDeviceDisabledDataComponent deviceDisabledDataComponent = requestContext.CreateDeviceDisabledDataComponent();
		return deviceDisabledDataComponent.QueryDisabledDevices();
	}
}
