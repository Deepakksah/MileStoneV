using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Audio;

public class AudioStreamControl : StreamControl<AudioParams>
{
	public AudioStreamControl(IConnection connection)
		: this(connection, Guid.Empty, null)
	{
	}

	public AudioStreamControl(IConnection connection, Guid videoId, OnResponse<BaseCommandResponse> onSuccessChangeStream)
		: base(connection, videoId, onSuccessChangeStream)
	{
	}
}
