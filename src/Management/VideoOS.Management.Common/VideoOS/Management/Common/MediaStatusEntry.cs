using System;
using System.Collections.Generic;

namespace VideoOS.Management.Common;

[Serializable]
public class MediaStatusEntry
{
	private List<MediaDataEntry> mediaData = new List<MediaDataEntry>();

	public Guid DeviceId { get; set; }

	public DateTime MediaRetentionTime { get; set; }

	public MediaStatusEntry(Guid deviceId, DateTime mediaRetentionTime)
	{
		DeviceId = deviceId;
		MediaRetentionTime = mediaRetentionTime;
	}

	public MediaStatusEntry(Guid deviceId, DateTime mediaRetentionTime, MediaDataEntry[] mediaData)
	{
		DeviceId = deviceId;
		MediaRetentionTime = mediaRetentionTime;
		this.mediaData.AddRange(mediaData);
	}

	public MediaDataEntry[] GetMediaData()
	{
		return mediaData.ToArray();
	}
}
