using System;
using System.Runtime.Serialization;

namespace VideoOS.Server.DataComponents;

[Serializable]
public class DatabaseCommunicationException : Exception
{
	public DatabaseCommunicationException()
	{
	}

	public DatabaseCommunicationException(string message)
		: base(message)
	{
	}

	public DatabaseCommunicationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DatabaseCommunicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
