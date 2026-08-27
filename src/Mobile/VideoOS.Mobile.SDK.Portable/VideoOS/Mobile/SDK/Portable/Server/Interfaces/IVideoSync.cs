using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IVideoSync : IVideoControlSync, IStreamControlSync<VideoParams>
{
	RequestStreamResponseDownload RequestStream(VideoParams videoParams, PlaybackParams playbackParams, TimeSpan timeout);

	RequestStreamResponseUpload RequestUploadStream(TimeSpan timeout);
}
