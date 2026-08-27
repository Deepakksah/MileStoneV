using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommonComponents;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Video;

public class VideoFactory : BaseStreamConsumerFactory, IVideoFactory, IStreamConsumerFactory
{
	public VideoFactory(IConnection iConnection, IOnVideoStream onVideoStream, string streamAlias)
		: base(iConnection, onVideoStream, streamAlias)
	{
	}

	public LiveVideo CreateLiveVideo(RequestStreamResponseLive response)
	{
		return new LiveVideo(response, _iConnection, _onVideoStream, _streamAlias);
	}

	public PlaybackVideo CreatePlaybackVideo(RequestStreamResponsePlayback response)
	{
		return new PlaybackVideo(response, _iConnection, _onVideoStream, _streamAlias);
	}

	public PlaybackController CreatePlaybackController(RequestPlaybackControllerResponse response)
	{
		return new PlaybackController(response, base.IConnection, base.StreamAlias);
	}

	public PushVideo CreatePushVideo(RequestStreamResponseUpload response)
	{
		return new PushVideo(response, base.IConnection, base.OnVideoStream, base.StreamAlias);
	}
}
