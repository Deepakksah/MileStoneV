using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class MulticameraPlayback : IMulticameraPlaybackAsync, IMulticameraPlaybackSync
{
	private readonly IConnection _connection;

	public MulticameraPlayback(IConnection connection)
	{
		_connection = connection;
	}

	public IAsyncResult CreatePlaybackController(VideoParams videoParams, PlaybackParams playbackParams, OnResponse<RequestPlaybackControllerResponse> onSuccess = null, OnResponse<RequestPlaybackControllerResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.CreatePlaybackController);
		videoParams.ConvertToCommand(ref tCommand);
		playbackParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new RequestPlaybackControllerResponse(response));
	}

	public IAsyncResult ChangeMultipleStreams(PlaybackParams playbackParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.ChangeMultipleStreams);
		playbackParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail);
	}

	public RequestPlaybackControllerResponse CreatePlaybackController(VideoParams videoParams, PlaybackParams playbackParams, TimeSpan timeout)
	{
		IAsyncResult iResult = CreatePlaybackController(videoParams, playbackParams);
		return new RequestPlaybackControllerResponse(_connection.ProcessResult(iResult, timeout));
	}

	public BaseCommandResponse ChangeMultipleStreams(PlaybackParams playbackParams, TimeSpan timeout)
	{
		IAsyncResult iResult = ChangeMultipleStreams(playbackParams);
		return new RequestPlaybackControllerResponse(_connection.ProcessResult(iResult, timeout));
	}
}
