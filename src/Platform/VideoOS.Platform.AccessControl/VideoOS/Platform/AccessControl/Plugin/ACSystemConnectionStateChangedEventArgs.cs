using System;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACSystemConnectionStateChangedEventArgs : EventArgs
{
	public bool Connected { get; private set; }

	public ACSystemConnectionStateChangedEventArgs(bool connected)
	{
		Connected = connected;
	}
}
