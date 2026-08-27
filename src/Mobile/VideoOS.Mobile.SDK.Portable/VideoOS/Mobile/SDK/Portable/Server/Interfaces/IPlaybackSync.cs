using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IPlaybackSync
{
	BaseCommandResponse ChangePlayback(PlaybackParams playbackParams, TimeSpan timeout);
}
