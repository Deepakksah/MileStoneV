using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class PlaybackControlCommands : PlaybackBase, IPlaybackControlAsync, IPlaybackBaseAsync, IPlaybackControlSync, IPlaybackBaseSync
{
	private readonly Guid _videoId;

	public PlaybackControlCommands(IConnection connection, Guid videoId)
		: base(connection, Command.CommandKinds.ChangeStream, null, delegate(Command command)
		{
			command.InputParams.Add(new Parameter("VideoId", videoId.ToString()));
		})
	{
		_videoId = videoId;
	}

	public IAsyncResult GoToDbPosition(PlaybackParamsHelper.SeekType seekType, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		return SendChangeStreamsCommand(delegate(PlaybackParams playbackParams)
		{
			playbackParams.SeekType = seekType;
		}, onSuccess, onFail);
	}

	public BaseCommandResponse GoToDbPosition(PlaybackParamsHelper.SeekType seekType, TimeSpan timeout)
	{
		IAsyncResult iResult = GoToDbPosition(seekType);
		return _connection.ProcessResult(iResult, timeout);
	}
}
