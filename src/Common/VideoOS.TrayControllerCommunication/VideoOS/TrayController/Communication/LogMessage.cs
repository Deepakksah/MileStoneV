using System;
using System.Diagnostics;

namespace VideoOS.TrayController.Communication;

[DebuggerDisplay("LogMessage, {SequenceNumber}; {Timestamp}; {Message}")]
public class LogMessage
{
	public int SequenceNumber { get; set; }

	public DateTime Timestamp { get; set; }

	public string Message { get; set; }
}
