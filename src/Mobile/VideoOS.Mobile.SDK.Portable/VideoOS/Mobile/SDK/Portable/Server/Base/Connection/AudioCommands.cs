using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class AudioCommands
{
	private readonly IConnection _connection;

	public AudioCommands(IConnection connection)
	{
		_connection = connection;
	}

	public IAsyncResult RequestAudioStream(AudioParams audioParams, PlaybackParams playbackParams = null, OnResponse<BaseRequestStreamResponseDownload<AudioParams>> onSuccess = null, OnResponse<BaseRequestStreamResponseDownload<AudioParams>> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.RequestAudioStream);
		playbackParams?.ConvertToCommand(ref tCommand);
		audioParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new BaseRequestStreamResponseDownload<AudioParams>(response)
		{
			StreamParams = audioParams
		});
	}

	public IAsyncResult RequestAudioStreamIn(AudioParams audioParams, OnResponse<RequestStreamResponseUpload> onSuccess = null, OnResponse<RequestStreamResponseUpload> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.RequestAudioStreamIn);
		audioParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new RequestStreamResponseUpload(response));
	}

	public IAsyncResult RequestAudioPush(AudioParams audioParams, OnResponse<RequestStreamResponseUpload> onSuccess = null, OnResponse<RequestStreamResponseUpload> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.RequestAudioPush);
		audioParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new RequestStreamResponseUpload(response));
	}

	public BaseRequestStreamResponseDownload<AudioParams> RequestAudioStream(AudioParams audioParams, PlaybackParams playbackParams, TimeSpan timeout)
	{
		IAsyncResult iResult = RequestAudioStream(audioParams, playbackParams);
		return new BaseRequestStreamResponseDownload<AudioParams>(_connection.ProcessResult(iResult, timeout))
		{
			StreamParams = audioParams
		};
	}

	public RequestStreamResponseUpload RequestAudioStreamIn(AudioParams audioParams, TimeSpan timeout)
	{
		IAsyncResult iResult = RequestAudioStreamIn(audioParams);
		return new RequestStreamResponseUpload(_connection.ProcessResult(iResult, timeout));
	}

	public RequestStreamResponseUpload RequestAudioPush(AudioParams audioParams, TimeSpan timeout)
	{
		IAsyncResult iResult = RequestAudioPush(audioParams);
		return new RequestStreamResponseUpload(_connection.ProcessResult(iResult, timeout));
	}
}
