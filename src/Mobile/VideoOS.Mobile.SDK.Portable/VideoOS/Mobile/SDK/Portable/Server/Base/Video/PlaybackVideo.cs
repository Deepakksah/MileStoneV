using System;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Video;

public class PlaybackVideo : PlaybackStream<VideoParams, CameraCapabilities, VideoStreamControl>, IPlaybackAsync, IPlaybackSync
{
	public override VideoStreamControl CreateStreamControl(IConnection connection, Guid streamId)
	{
		return new VideoStreamControl(connection, streamId, base.OnSuccessChangeStream);
	}

	public PlaybackVideo(RequestStreamResponsePlayback response, IConnection iConnection, IOnVideoStream onVideoStream = null, string alias = "XProtectMobile/Video")
		: base((BaseRequestStreamResponsePlayback<VideoParams>)response, iConnection, onVideoStream, alias)
	{
	}

	public override void ExtendReconnectCommand(Command command)
	{
		if (base.PlaybackParams != null)
		{
			base.PlaybackParams.ConvertToCommand(ref command);
		}
		else
		{
			Logger.Instance.Log(LogContent.Error, "PlaybackVideo.ExtendReconnectCommand", "PlaybackParams is NULL !");
		}
	}

	protected override void OnNewFrame(VideoFrame frame)
	{
		if (frame != null && frame.Data != null && frame.Data.Length != 0)
		{
			base.PlaybackParams.SeekType = PlaybackParamsHelper.SeekType.Time;
			base.PlaybackParams.Time = TimeConverter.FromLong((long)frame.MainHeader.TimeStampUtcMs);
		}
	}
}
