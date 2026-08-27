using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.VideoSource;

public interface IVideoSourceQuality
{
	ConnectionQuality GetInputConnectionQuality();
}
