using System;

namespace VideoOS.UI.Common.WPF.Timeline.Events;

public class PlaybackStateEventArgs : EventArgs
{
	public PlaybackState PlaybackState { get; set; }

	public PlaybackStateEventArgs(PlaybackState playbackState)
	{
		PlaybackState = playbackState;
	}
}
