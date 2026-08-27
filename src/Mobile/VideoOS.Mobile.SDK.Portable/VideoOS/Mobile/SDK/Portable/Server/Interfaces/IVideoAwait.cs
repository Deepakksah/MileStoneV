using System;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IVideoAwait : IVideoControlAwait, IStreamControlAwait<VideoParams>
{
	Task<RequestStreamResponseDownload> RequestStreamAsync(VideoParams videoParams, PlaybackParams playbackParams, TimeSpan timeout);

	Task<RequestStreamResponseUpload> RequestUploadStreamAsync(TimeSpan timeout);
}
