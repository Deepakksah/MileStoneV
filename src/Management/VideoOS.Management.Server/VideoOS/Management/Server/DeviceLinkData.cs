using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public sealed class DeviceLinkData
{
	public Guid IdDevice { get; set; }

	public Guid IdDeviceLink { get; set; }

	public int Priority { get; set; }

	public DateTime LastModified { get; set; }

	public static IEnumerable<DeviceLinkData> QueryDeviceLinks(IGeminiRequestContext requestContext)
	{
		using IDeviceLinkDataComponent deviceLinkDataComponent = requestContext.CreateDeviceLinkDataComponent();
		return deviceLinkDataComponent.QueryDeviceLinks();
	}
}
