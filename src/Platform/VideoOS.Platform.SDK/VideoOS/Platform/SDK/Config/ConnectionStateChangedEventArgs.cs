using System;

namespace VideoOS.Platform.SDK.Config;

public class ConnectionStateChangedEventArgs : EventArgs
{
	private ConnectStates _connectionState;

	public ConnectStates ConnectionState => _connectionState;

	public ConnectionStateChangedEventArgs(ConnectStates connectState)
	{
		_connectionState = connectState;
	}
}
