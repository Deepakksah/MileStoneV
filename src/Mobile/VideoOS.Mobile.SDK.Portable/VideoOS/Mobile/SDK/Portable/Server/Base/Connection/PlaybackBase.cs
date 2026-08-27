using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class PlaybackBase : IPlaybackBaseAsync, IPlaybackBaseSync
{
	protected readonly IConnection _connection;

	private readonly Command.CommandKinds _changeStreamCommand;

	private readonly Action<PlaybackParams> _onParamsGlobal;

	private readonly Action<Command> _onCommandGlobal;

	public PlaybackBase(IConnection connection, Command.CommandKinds changeStreamCommand, Action<PlaybackParams> onParams, Action<Command> onCommand)
	{
		_connection = connection;
		_changeStreamCommand = changeStreamCommand;
		_onParamsGlobal = onParams;
		_onCommandGlobal = onCommand;
	}

	protected IAsyncResult SendChangeStreamsCommand(Action<PlaybackParams> onParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(_changeStreamCommand);
		PlaybackParams playbackParams = new PlaybackParams();
		onParams?.Invoke(playbackParams);
		_onParamsGlobal?.Invoke(playbackParams);
		playbackParams.ConvertToCommand(ref tCommand);
		_onCommandGlobal?.Invoke(tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail);
	}

	public IAsyncResult ChangeSpeed(double speed, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		return SendChangeStreamsCommand(delegate(PlaybackParams playbackParams)
		{
			playbackParams.Speed = speed;
		}, onSuccess, onFail);
	}

	public IAsyncResult GoToTime(DateTime utcTime, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		return SendChangeStreamsCommand(delegate(PlaybackParams playbackParams)
		{
			playbackParams.SeekType = PlaybackParamsHelper.SeekType.Time;
			playbackParams.Time = utcTime;
		}, onSuccess, onFail);
	}

	public IAsyncResult GotoNextSequence(OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		return SendChangeStreamsCommand(delegate(PlaybackParams playbackParams)
		{
			playbackParams.SeekType = PlaybackParamsHelper.SeekType.SequenceNext;
		}, onSuccess, onFail);
	}

	public IAsyncResult GotoPreviousSequence(OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		return SendChangeStreamsCommand(delegate(PlaybackParams playbackParams)
		{
			playbackParams.SeekType = PlaybackParamsHelper.SeekType.SequencePrevious;
		}, onSuccess, onFail);
	}

	public IAsyncResult GotoNextFrame(OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		return SendChangeStreamsCommand(delegate(PlaybackParams playbackParams)
		{
			playbackParams.SeekType = PlaybackParamsHelper.SeekType.FrameNext;
		}, onSuccess, onFail);
	}

	public IAsyncResult GotoPreviousFrame(OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		return SendChangeStreamsCommand(delegate(PlaybackParams playbackParams)
		{
			playbackParams.SeekType = PlaybackParamsHelper.SeekType.FramePrevious;
		}, onSuccess, onFail);
	}

	public IAsyncResult ChangePlayback(Action<PlaybackParams> onPlaybackParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		return SendChangeStreamsCommand(onPlaybackParams, onSuccess, onFail);
	}

	public BaseCommandResponse ChangeSpeed(double speed, TimeSpan timeout)
	{
		IAsyncResult iResult = ChangeSpeed(speed);
		return _connection.ProcessResult(iResult, timeout);
	}

	public BaseCommandResponse GoToTime(DateTime utcTime, TimeSpan timeout)
	{
		IAsyncResult iResult = GoToTime(utcTime);
		return _connection.ProcessResult(iResult, timeout);
	}

	public BaseCommandResponse GotoPreviousSequence(TimeSpan timeout)
	{
		IAsyncResult iResult = GotoPreviousSequence();
		return _connection.ProcessResult(iResult, timeout);
	}

	public BaseCommandResponse GotoNextSequence(TimeSpan timeout)
	{
		IAsyncResult iResult = GotoNextSequence();
		return _connection.ProcessResult(iResult, timeout);
	}

	public BaseCommandResponse GotoPreviousFrame(TimeSpan timeout)
	{
		IAsyncResult iResult = GotoPreviousFrame();
		return _connection.ProcessResult(iResult, timeout);
	}

	public BaseCommandResponse GotoNextFrame(TimeSpan timeout)
	{
		IAsyncResult iResult = GotoNextFrame();
		return _connection.ProcessResult(iResult, timeout);
	}

	public BaseCommandResponse ChangePlayback(Action<PlaybackParams> onPlaybackParams, TimeSpan timeout)
	{
		IAsyncResult iResult = ChangePlayback(onPlaybackParams);
		return _connection.ProcessResult(iResult, timeout);
	}
}
