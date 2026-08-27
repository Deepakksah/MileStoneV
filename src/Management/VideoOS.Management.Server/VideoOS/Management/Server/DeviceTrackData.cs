using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public sealed class DeviceTrackData
{
	public Guid Id { get; set; }

	public Guid TrackId { get; set; }

	public bool DefaultPlayback { get; set; }

	public bool MergeEdge { get; set; }

	public bool IsPrimary { get; set; }

	public bool IsEdge { get; set; }

	public static Dictionary<Guid, IEnumerable<DeviceTrackData>> QueryDeviceTracksByDeviceIds(IGeminiRequestContext requestContext, Guid[] deviceIds)
	{
		using IDeviceTrackDataComponent deviceTrackDataComponent = requestContext.CreateDeviceTrackDataComponent();
		return deviceTrackDataComponent.QueryDeviceTracksByDeviceIds(deviceIds);
	}
}
