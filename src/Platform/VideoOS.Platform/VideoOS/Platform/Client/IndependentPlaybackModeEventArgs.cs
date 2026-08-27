using System;

namespace VideoOS.Platform.Client;

public class IndependentPlaybackModeEventArgs : EventArgs
{
	public bool IndependentPlaybackEnabled { get; set; }

	public IndependentPlaybackModeEventArgs(bool enabled)
	{
		IndependentPlaybackEnabled = enabled;
	}
}
