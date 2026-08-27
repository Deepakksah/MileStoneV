using System;

namespace VideoOS.UI.Common.WPF.Timeline.Events;

public class PlaybackSpeedEventArgs : EventArgs
{
	public double PlaybackSpeed { get; set; }

	public PlaybackSpeedEventArgs(double playbackSpeed)
	{
		PlaybackSpeed = playbackSpeed;
	}
}
