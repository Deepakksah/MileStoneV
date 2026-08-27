using System;

namespace VideoOS.Installer.Sequencer;

public class MessageEventArgs : EventArgs
{
	public string Message { get; set; }

	public MessageEventArgs(string message)
	{
		Message = message;
	}
}
