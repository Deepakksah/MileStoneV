using System;

namespace VideoOS.UI.Common.WPF.Timeline.Events;

public class PlaybackNavigationEventArgs : EventArgs
{
	public enum PlaybackNavigationType
	{
		First,
		PreviousSequence,
		Previous,
		Next,
		NextSequence,
		Last
	}

	public PlaybackNavigationType PlaybackNavigation { get; set; }

	public PlaybackNavigationEventArgs(PlaybackNavigationType playbackNavigation)
	{
		PlaybackNavigation = playbackNavigation;
	}
}
