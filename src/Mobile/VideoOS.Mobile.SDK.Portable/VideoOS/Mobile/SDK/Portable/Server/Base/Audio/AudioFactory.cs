using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommonComponents;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Audio;

public class AudioFactory : BaseStreamConsumerFactory, IAudioFactory, IStreamConsumerFactory
{
	public AudioFactory(IConnection iConnection, IOnVideoStream onVideoStream, string audioAlias)
		: base(iConnection, onVideoStream, audioAlias)
	{
	}

	public LiveAudio CreateLiveAudio(BaseRequestStreamResponseDownload<AudioParams> response)
	{
		return new LiveAudio(response, _iConnection, _onVideoStream, _streamAlias);
	}

	public PlaybackAudio CreatePlaybackAudio(BaseRequestStreamResponsePlayback<AudioParams> response)
	{
		return new PlaybackAudio(response, _iConnection, _onVideoStream, _streamAlias);
	}

	public PushToTalk CreatePushToTalk(RequestStreamResponseUpload response)
	{
		return new PushToTalk(response, _iConnection, _onVideoStream, _streamAlias);
	}
}
