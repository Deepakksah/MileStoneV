using System;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class PlaybackStatusEventArgs : EventArgs
{
	public PlaybackFlags ChangedStatusFlags;

	public PlaybackFlags CurrentStatusFlags;

	public PlaybackStatusEventArgs()
	{
		ChangedStatusFlags = PlaybackFlags.Nothing;
		CurrentStatusFlags = PlaybackFlags.Nothing;
	}
}
