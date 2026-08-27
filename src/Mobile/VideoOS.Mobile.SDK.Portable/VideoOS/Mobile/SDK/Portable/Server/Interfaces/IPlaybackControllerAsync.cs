using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IPlaybackControllerAsync : IPlaybackBaseAsync
{
	IAsyncResult RequestPlaybackStream(VideoParams videoParams, PlaybackParams playbackParams = null, OnResponse<RequestStreamResponsePlayback> onSuccess = null, OnResponse<RequestStreamResponsePlayback> onFail = null);
}
