using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Video;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IVideoFactory : IStreamConsumerFactory
{
	LiveVideo CreateLiveVideo(RequestStreamResponseLive response);

	PlaybackVideo CreatePlaybackVideo(RequestStreamResponsePlayback response);

	PlaybackController CreatePlaybackController(RequestPlaybackControllerResponse response);

	PushVideo CreatePushVideo(RequestStreamResponseUpload response);
}
