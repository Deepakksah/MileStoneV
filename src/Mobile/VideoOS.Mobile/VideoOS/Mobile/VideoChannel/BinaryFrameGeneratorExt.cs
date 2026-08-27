using VideoOS.Mobile.Portable.VideoChannel.Binary;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Platform.Live;

namespace VideoOS.Mobile.VideoChannel;

public class BinaryFrameGeneratorExt
{
	public static bool GenerateFromEvents(ref HeaderExtensionLiveEvents liveEvents, object events)
	{
		bool result = false;
		if (events != null && events is LiveStatusEventArgs e)
		{
			liveEvents.ChangedFlags = (uint)e.ChangedStatusFlags;
			liveEvents.CurrentFlags = (uint)e.CurrentStatusFlags;
			result = true;
		}
		return result;
	}

	public static bool GenerateFromEvents(ref HeaderExtensionCarouselInfo carouselEvents, object events)
	{
		bool result = false;
		if (events != null && events is CarouselInfoEventArgs e)
		{
			carouselEvents.HeaderSize = 24u;
			carouselEvents.Version = 0u;
			carouselEvents.ItemID = e.CameraId;
			result = true;
		}
		return result;
	}
}
