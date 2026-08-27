using System;

namespace VideoOS.Platform.Client;

public class ConnectResponseEventArgs : EventArgs
{
	public bool ConnectionGranted { get; set; }

	public ConnectResponseEventArgs(bool connected)
	{
		ConnectionGranted = connected;
	}
}
