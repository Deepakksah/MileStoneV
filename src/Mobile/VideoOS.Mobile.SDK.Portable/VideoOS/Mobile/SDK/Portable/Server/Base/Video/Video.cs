using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Video;

public class Video : LiveStream<VideoParams, CameraCapabilities, VideoStreamControl>
{
	public Video(RequestStreamResponseDownload response, IConnection iConnection, IOnVideoStream onVideoStream = null, string alias = "XProtectMobile/Video")
		: base((BaseRequestStreamResponseDownload<VideoParams>)response, iConnection, onVideoStream, alias)
	{
	}

	public override VideoStreamControl CreateStreamControl(IConnection connection, Guid streamId)
	{
		return new VideoStreamControl(connection, streamId, base.OnSuccessChangeStream);
	}
}
