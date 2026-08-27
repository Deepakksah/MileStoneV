using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IMulticameraPlaybackAsync
{
	IAsyncResult CreatePlaybackController(VideoParams videoParams, PlaybackParams playbackParams, OnResponse<RequestPlaybackControllerResponse> onSuccess = null, OnResponse<RequestPlaybackControllerResponse> onFail = null);

	IAsyncResult ChangeMultipleStreams(PlaybackParams playbackParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);
}
