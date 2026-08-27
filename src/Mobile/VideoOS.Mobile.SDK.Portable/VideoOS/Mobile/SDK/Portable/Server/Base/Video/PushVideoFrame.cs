using VideoOS.Mobile.Portable.VideoChannel.Binary;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Video;

public class PushVideoFrame : PushStreamData
{
	public LocationInfo LocationInfo;

	public PushVideoFrame()
	{
		LocationInfo = null;
	}
}
