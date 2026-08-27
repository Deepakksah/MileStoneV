using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.Logging;

[Serializable]
public class LogManagerFailedException : Exception
{
	public LogManagerFailedException()
	{
	}

	public LogManagerFailedException(string message)
		: base(message)
	{
	}

	public LogManagerFailedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected LogManagerFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
