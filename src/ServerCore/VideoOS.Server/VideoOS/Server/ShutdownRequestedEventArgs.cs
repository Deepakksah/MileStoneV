using System;

namespace VideoOS.Server;

public sealed class ShutdownRequestedEventArgs : EventArgs
{
	public string Message { get; private set; }

	public ShutdownRequestedEventArgs(string message)
	{
		Message = message;
	}
}
