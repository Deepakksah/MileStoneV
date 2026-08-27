using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class ThumbnailCommands : IErrorResult, IThumbnailAsync, IThumbnailSync
{
	private readonly IConnection _connection;

	private readonly IErrorResult _errorResult;

	public Command.CommandResultTypes Result => _errorResult.Result;

	public string ErrorMessage => _errorResult.ErrorMessage;

	public ErrorCodes ErrorCode => _errorResult.ErrorCode;

	public ThumbnailCommands(IConnection connection)
	{
		_connection = connection;
		_errorResult = connection as IErrorResult;
	}

	public IAsyncResult GetThumbnail(VideoParams videoParams, OnResponse<GetThumbnailResponse> onSuccess = null, OnResponse<GetThumbnailResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.GetThumbnail);
		videoParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new GetThumbnailResponse(response));
	}

	public IAsyncResult GetThumbnailByTime(VideoParams videoParams, PlaybackParams playbackParams, OnResponse<GetThumbnailResponse> onSuccess = null, OnResponse<GetThumbnailResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.GetThumbnailByTime);
		videoParams.ConvertToCommand(ref tCommand);
		playbackParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new GetThumbnailResponse(response));
	}

	public GetThumbnailResponse GetThumbnail(VideoParams videoParams, TimeSpan timeout)
	{
		IAsyncResult thumbnail = GetThumbnail(videoParams);
		return new GetThumbnailResponse(_connection.ProcessResult(thumbnail, timeout));
	}

	public GetThumbnailResponse GetThumbnailByTime(VideoParams videoParams, PlaybackParams playbackParams, TimeSpan timeout)
	{
		IAsyncResult thumbnailByTime = GetThumbnailByTime(videoParams, playbackParams);
		return new GetThumbnailResponse(_connection.ProcessResult(thumbnailByTime, timeout));
	}
}
