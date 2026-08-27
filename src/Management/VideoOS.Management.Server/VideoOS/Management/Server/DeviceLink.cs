using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public class DeviceLink
{
	public Guid DeviceId { get; set; }

	public Guid[] RelatedDeviceIdList { get; set; }

	public DeviceLink(Guid deviceId, Guid[] relatedDeviceIdList)
	{
		DeviceId = deviceId;
		RelatedDeviceIdList = relatedDeviceIdList;
	}

	public static Guid[] QueryDeviceLink(IGeminiRequestContext requestContext, string deviceIdString)
	{
		using IDeviceLinkDataComponent deviceLinkDataComponent = requestContext.CreateDeviceLinkDataComponent();
		IEnumerable<DeviceLinkData> enumerable = deviceLinkDataComponent.QueryDeviceLinksFromId(deviceIdString);
		List<Guid> list = new List<Guid>();
		foreach (DeviceLinkData item in enumerable)
		{
			list.Add(item.IdDeviceLink);
		}
		return list.ToArray();
	}

	public static Guid[] QueryRelatedDeviceLink(IGeminiRequestContext requestContext, string relatedIdString)
	{
		using IDeviceLinkDataComponent deviceLinkDataComponent = requestContext.CreateDeviceLinkDataComponent();
		IEnumerable<DeviceLinkData> enumerable = deviceLinkDataComponent.QueryDeviceLinksFromLinkedId(relatedIdString);
		List<Guid> list = new List<Guid>();
		foreach (DeviceLinkData item in enumerable)
		{
			list.Add(item.IdDevice);
		}
		return list.ToArray();
	}

	public static void AddDeviceLink(IGeminiRequestContext requestContext, string deviceId, string relatedDeviceId)
	{
		using (IDeviceLinkDataComponent deviceLinkDataComponent = requestContext.CreateDeviceLinkDataComponent())
		{
			deviceLinkDataComponent.AddDeviceLink(deviceId, relatedDeviceId);
		}
		Integration.TryNotifyServerAsync();
	}

	public static void RemoveDeviceLink(IGeminiRequestContext requestContext, string deviceId, string relatedDeviceId)
	{
		using (IDeviceLinkDataComponent deviceLinkDataComponent = requestContext.CreateDeviceLinkDataComponent())
		{
			deviceLinkDataComponent.RemoveDeviceLink(deviceId, relatedDeviceId);
		}
		Integration.TryNotifyServerAsync();
	}
}
