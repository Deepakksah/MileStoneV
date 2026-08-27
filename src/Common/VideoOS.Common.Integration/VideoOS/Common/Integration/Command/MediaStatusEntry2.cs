using System;

namespace VideoOS.Common.Integration.Command;

public class MediaStatusEntry2
{
	public Guid DeviceId { get; set; }

	public DateTime MediaRetentionTime { get; set; }

	public MediaStatusEntry2(Guid deviceId, DateTime mediaRetentionTime)
	{
		DeviceId = deviceId;
		MediaRetentionTime = mediaRetentionTime;
	}
}
