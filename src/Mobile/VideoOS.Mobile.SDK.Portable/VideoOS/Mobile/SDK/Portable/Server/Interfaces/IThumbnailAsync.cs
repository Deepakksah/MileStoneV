using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IThumbnailAsync
{
	IAsyncResult GetThumbnail(VideoParams videoParams, OnResponse<GetThumbnailResponse> onSuccess = null, OnResponse<GetThumbnailResponse> onFail = null);

	IAsyncResult GetThumbnailByTime(VideoParams videoParams, PlaybackParams playbackParams, OnResponse<GetThumbnailResponse> onSuccess = null, OnResponse<GetThumbnailResponse> onFail = null);
}
