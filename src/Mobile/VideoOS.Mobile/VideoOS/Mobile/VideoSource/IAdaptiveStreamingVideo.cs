using VideoOS.Platform.Util.AdaptiveStreaming;

namespace VideoOS.Mobile.VideoSource;

public interface IAdaptiveStreamingVideo
{
	IStreamSelectionParams StreamSelectionParams { get; }
}
