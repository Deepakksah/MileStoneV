using System;

namespace VideoOS.Common.Integration.Command;

public class DeviceTrackId
{
	public Guid DeviceId { get; set; }

	public Guid TrackId { get; set; }

	public DeviceTrackId(Guid deviceId, Guid trackId)
	{
		DeviceId = deviceId;
		TrackId = trackId;
	}
}
