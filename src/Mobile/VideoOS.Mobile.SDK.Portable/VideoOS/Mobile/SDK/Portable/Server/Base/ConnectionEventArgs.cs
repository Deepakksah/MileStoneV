using System;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public class ConnectionEventArgs : EventArgs
{
	public ConnectionStates State { get; private set; }

	public ConnectionEventArgs(ConnectionStates state)
	{
		State = state;
	}
}
