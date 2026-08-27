using System;

namespace VideoOS.Platform.SDK.Interface;

public interface ITrackInfo
{
	Guid TrackId { get; set; }

	bool Edge { get; set; }
}
