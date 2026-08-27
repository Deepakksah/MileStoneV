using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IThumbnailSync
{
	GetThumbnailResponse GetThumbnail(VideoParams videoParams, TimeSpan timeout);

	GetThumbnailResponse GetThumbnailByTime(VideoParams videoParams, PlaybackParams playbackParams, TimeSpan timeout);
}
