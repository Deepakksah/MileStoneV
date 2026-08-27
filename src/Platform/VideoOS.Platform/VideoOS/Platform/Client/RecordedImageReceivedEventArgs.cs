using System;

namespace VideoOS.Platform.Client;

public class RecordedImageReceivedEventArgs : EventArgs
{
	public DateTime DateTime { get; set; }
}
