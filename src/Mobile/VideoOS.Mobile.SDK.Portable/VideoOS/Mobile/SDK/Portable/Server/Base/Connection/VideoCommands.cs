using System;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class VideoCommands : VideoControl, IVideoAsync, IVideoControlAsync, IStreamControlAsync<VideoParams>, IVideoSync, IVideoControlSync, IStreamControlSync<VideoParams>, IVideoAwait, IVideoControlAwait, IStreamControlAwait<VideoParams>
{
	public VideoCommands(IConnection connection)
		: this(connection, Guid.Empty, null)
	{
	}

	public VideoCommands(IConnection connection, Guid videoId, OnResponse<BaseCommandResponse> onSuccessChangeStream)
		: base(connection, videoId, onSuccessChangeStream)
	{
	}

	public IAsyncResult RequestStream(VideoParams videoParams, PlaybackParams playbackParams = null, OnResponse<RequestStreamResponseDownload> onSuccess = null, OnResponse<RequestStreamResponseDownload> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.RequestStream);
		videoParams.ConvertToCommand(ref tCommand);
		playbackParams?.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new RequestStreamResponseDownload(response)
		{
			StreamParams = videoParams
		});
	}

	public IAsyncResult RequestUploadStream(OnResponse<RequestStreamResponseUpload> onSuccess = null, OnResponse<RequestStreamResponseUpload> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.RequestStream);
		command.InputParams.Add(new Parameter("SignalType", EnumsHelper.GetDescription(StreamParamsHelper.SignalType.Upload)));
		return _connection.SendCommand(command, onSuccess, onFail, (BaseCommandResponse response) => new RequestStreamResponseUpload(response));
	}

	public RequestStreamResponseDownload RequestStream(VideoParams videoParams, PlaybackParams playbackParams, TimeSpan timeout)
	{
		IAsyncResult iResult = RequestStream(videoParams, playbackParams);
		return new RequestStreamResponseDownload(_connection.ProcessResult(iResult, timeout))
		{
			StreamParams = videoParams
		};
	}

	public RequestStreamResponseUpload RequestUploadStream(TimeSpan timeout)
	{
		IAsyncResult iResult = RequestUploadStream();
		return new RequestStreamResponseUpload(_connection.ProcessResult(iResult, timeout));
	}

	public async Task<RequestStreamResponseDownload> RequestStreamAsync(VideoParams videoParams, PlaybackParams playbackParams, TimeSpan timeout)
	{
		IAsyncResult asyncResult = RequestStream(videoParams, playbackParams);
		return new RequestStreamResponseDownload(await _connection.ProcessResultTask(asyncResult, timeout))
		{
			StreamParams = videoParams
		};
	}

	public async Task<RequestStreamResponseUpload> RequestUploadStreamAsync(TimeSpan timeout)
	{
		IAsyncResult asyncResult = RequestUploadStream();
		return new RequestStreamResponseUpload(await _connection.ProcessResultTask(asyncResult, timeout));
	}
}
