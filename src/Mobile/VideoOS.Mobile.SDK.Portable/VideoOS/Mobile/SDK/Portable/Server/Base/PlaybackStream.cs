using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Base.Video;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public abstract class PlaybackStream<TParams, TCapabilities, TControl> : LiveStream<TParams, TCapabilities, TControl>, IPlaybackAsync, IPlaybackSync where TParams : StreamParams, new() where TCapabilities : StreamingDeviceCapabilities, new() where TControl : StreamControl<TParams>
{
	public PlaybackParams PlaybackParams { get; }

	public PlaybackControlCommands PlaybackControl { get; }

	public PlaybackStream(BaseRequestStreamResponsePlayback<TParams> response, IConnection iConnection, IOnVideoStream onVideoStream = null, string alias = "XProtectMobile/Video")
		: base((BaseRequestStreamResponseDownload<TParams>)response, iConnection, onVideoStream, alias)
	{
		PlaybackParams = response.PlaybackParams ?? new PlaybackParams();
		PlaybackControl = new PlaybackControlCommands(iConnection, base.StreamId);
	}

	public IAsyncResult ChangePlayback(PlaybackParams tPlaybackParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.ChangeStream);
		tCommand.InputParams.Add(new Parameter("VideoId", base.StreamId.ToString()));
		tPlaybackParams.ConvertToCommand(ref tCommand);
		PlaybackParams.InitFromAnother(tPlaybackParams);
		return base.Connection.SendCommand(tCommand, onSuccess, onFail);
	}

	public BaseCommandResponse ChangePlayback(PlaybackParams tPlaybackParams, TimeSpan timeout)
	{
		IAsyncResult iResult = ChangePlayback(tPlaybackParams);
		return base.Connection.ProcessResult(iResult, timeout);
	}
}
