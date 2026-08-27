using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.WebSockets;

public class WebSocketReaderException : Exception
{
	public WebSocketReaderException()
	{
	}

	public WebSocketReaderException(string message)
		: base(message)
	{
	}

	public WebSocketReaderException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected WebSocketReaderException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
