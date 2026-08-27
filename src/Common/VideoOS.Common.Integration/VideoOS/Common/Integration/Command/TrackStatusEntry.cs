using System;

namespace VideoOS.Common.Integration.Command;

public class TrackStatusEntry
{
	public Guid TrackId { get; set; }

	public DateTime MediaTime { get; set; }

	public TrackStatusEntry(Guid trackId, DateTime mediaTime)
	{
		TrackId = trackId;
		MediaTime = mediaTime;
	}
}
