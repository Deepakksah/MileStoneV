using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Video;

public class LiveVideo : Video
{
	public StreamPtz Ptz { get; }

	public LiveVideo(RequestStreamResponseLive response, IConnection iConnection, IOnVideoStream onVideoStream = null, string alias = "XProtectMobile/Video")
		: base(response, iConnection, onVideoStream, alias)
	{
		Ptz = new StreamPtz(iConnection, response.VideoId);
	}
}
