using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.Audio;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IAudioFactory : IStreamConsumerFactory
{
	LiveAudio CreateLiveAudio(BaseRequestStreamResponseDownload<AudioParams> response);

	PlaybackAudio CreatePlaybackAudio(BaseRequestStreamResponsePlayback<AudioParams> response);

	PushToTalk CreatePushToTalk(RequestStreamResponseUpload response);
}
