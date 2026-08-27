using VideoOS.Platform.Util.AdaptiveStreaming;
using VideoOS.Toolkit;

namespace VideoOS.Platform.Common;

public static class StreamSelectionParamsExtensions
{
	public static LiveStreamSelection ToLiveStreamSelection(this IStreamSelectionParams streamSelectionParams)
	{
		LiveStreamSelection liveStreamSelection = null;
		if (streamSelectionParams != null)
		{
			switch (streamSelectionParams.StreamSelectionType)
			{
			case StreamSelectionType.MaximalResolution:
				liveStreamSelection = LiveStreamSelection.CreateLiveStreamSelection_MaxResolution();
				break;
			case StreamSelectionType.AdaptiveToResolution:
			{
				streamSelectionParams.GetStreamAdaptiveResolution(out var width, out var height);
				liveStreamSelection = LiveStreamSelection.CreateLiveStreamSelection_AdaptToResolution(width, height);
				break;
			}
			}
		}
		if (liveStreamSelection == null)
		{
			liveStreamSelection = LiveStreamSelection.CreateLiveStreamSelection_DefaultStream();
		}
		return liveStreamSelection;
	}
}
