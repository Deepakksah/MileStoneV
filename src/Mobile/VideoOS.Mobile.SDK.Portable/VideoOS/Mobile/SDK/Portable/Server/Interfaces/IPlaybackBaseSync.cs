using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IPlaybackBaseSync
{
	BaseCommandResponse ChangeSpeed(double speed, TimeSpan timeout);

	BaseCommandResponse GoToTime(DateTime utcTime, TimeSpan timeout);

	BaseCommandResponse GotoPreviousSequence(TimeSpan timeout);

	BaseCommandResponse GotoNextSequence(TimeSpan timeout);

	BaseCommandResponse GotoPreviousFrame(TimeSpan timeout);

	BaseCommandResponse GotoNextFrame(TimeSpan timeout);

	BaseCommandResponse ChangePlayback(Action<PlaybackParams> onPlaybackParams, TimeSpan timeout);
}
