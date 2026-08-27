using System;

namespace VideoOS.Platform.Messaging;

public class ConnectionStateEventArgs : EventArgs
{
	public bool IsConnected;

	public ConnectionStateEventArgs(bool up)
	{
		IsConnected = up;
	}
}
