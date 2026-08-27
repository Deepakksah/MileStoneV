using System;

namespace VideoOS.Server;

public sealed class StatusMessageEventArgs : EventArgs
{
	public DateTime Timestamp { get; private set; }

	public string Message { get; private set; }

	public StatusMessageEventArgs(DateTime timestamp, string message)
	{
		Timestamp = timestamp;
		Message = message;
	}
}
