using System;

namespace VideoOS.Platform.Client;

public class PassRequestEventArgs : EventArgs
{
	public bool PassToNewImageViewer { get; set; }
}
