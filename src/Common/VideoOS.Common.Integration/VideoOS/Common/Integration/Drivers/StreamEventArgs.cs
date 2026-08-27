using System;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration.Drivers;

public class StreamEventArgs : EventArgs
{
	public StreamHwid Hwid { get; set; }

	public StreamState State { get; set; }

	public Exception Exception { get; set; }

	public StreamEventArgs(StreamHwid hwid, StreamState state, Exception exception)
	{
		Hwid = hwid;
		State = state;
		Exception = exception;
	}
}
