using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IVideoAsync : IVideoControlAsync, IStreamControlAsync<VideoParams>
{
	IAsyncResult RequestStream(VideoParams videoParams, PlaybackParams playbackParams = null, OnResponse<RequestStreamResponseDownload> onSuccess = null, OnResponse<RequestStreamResponseDownload> onFail = null);

	IAsyncResult RequestUploadStream(OnResponse<RequestStreamResponseUpload> onSuccess = null, OnResponse<RequestStreamResponseUpload> onFail = null);
}
