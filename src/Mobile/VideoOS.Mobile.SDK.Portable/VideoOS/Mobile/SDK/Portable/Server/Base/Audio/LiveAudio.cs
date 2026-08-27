using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Base.Video;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Audio;

public class LiveAudio : LiveStream<AudioParams, MicrophoneCapabilities, AudioStreamControl>
{
	public LiveAudio(BaseRequestStreamResponseDownload<AudioParams> response, IConnection iConnection, IOnVideoStream onVideoStream = null, string alias = "XProtectMobile/Video")
		: base(response, iConnection, onVideoStream, alias)
	{
	}

	public override AudioStreamControl CreateStreamControl(IConnection connection, Guid streamId)
	{
		return new AudioStreamControl(connection, streamId, base.OnSuccessChangeStream);
	}
}
