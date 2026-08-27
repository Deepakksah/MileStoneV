using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public class DeviceTrackIdDto
{
	public Guid DeviceId { get; set; }

	public Guid TrackId { get; set; }

	public DeviceTrackIdDto(Guid deviceId, Guid trackId)
	{
		DeviceId = deviceId;
		TrackId = trackId;
	}
}
