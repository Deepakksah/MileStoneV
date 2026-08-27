using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class PlaybackControllerCommands : PlaybackBase, IPlaybackControllerAsync, IPlaybackBaseAsync, IPlaybackControllerSync, IPlaybackBaseSync
{
	private readonly Guid _controllerId;

	private readonly VideoCommands _videoCommands;

	public PlaybackControllerCommands(IConnection connection, VideoCommands videoCommands, Guid playbackControllerId)
		: base(connection, Command.CommandKinds.ChangeMultipleStreams, delegate(PlaybackParams playbackParams)
		{
			playbackParams.ControllerId = playbackControllerId;
		}, null)
	{
		_controllerId = playbackControllerId;
		_videoCommands = videoCommands;
	}

	private void CheckSetRequestStreamParams(ref VideoParams videoParams, ref PlaybackParams playbackParams)
	{
		videoParams.SignalType = StreamParamsHelper.SignalType.Playback;
		if (playbackParams == null)
		{
			playbackParams = new PlaybackParams();
		}
		playbackParams.ControllerId = _controllerId;
	}

	public IAsyncResult RequestPlaybackStream(VideoParams videoParams, PlaybackParams playbackParams = null, OnResponse<RequestStreamResponsePlayback> onSuccess = null, OnResponse<RequestStreamResponsePlayback> onFail = null)
	{
		CheckSetRequestStreamParams(ref videoParams, ref playbackParams);
		return _videoCommands.RequestStream(videoParams, playbackParams, delegate(RequestStreamResponseDownload response)
		{
			onSuccess?.Invoke(new RequestStreamResponsePlayback(response));
		}, delegate(RequestStreamResponseDownload response)
		{
			onFail?.Invoke(new RequestStreamResponsePlayback(response));
		});
	}

	public RequestStreamResponsePlayback RequestPlaybackStream(VideoParams videoParams, PlaybackParams playbackParams, TimeSpan timeout)
	{
		CheckSetRequestStreamParams(ref videoParams, ref playbackParams);
		return new RequestStreamResponsePlayback(_videoCommands.RequestStream(videoParams, playbackParams, timeout));
	}
}
