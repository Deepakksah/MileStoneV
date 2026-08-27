using System;
using System.Runtime.Serialization;

namespace VideoOS.Server;

[Serializable]
public class LogDefinitionFileInvalidException : Exception
{
	public LogDefinitionFileInvalidException()
	{
	}

	public LogDefinitionFileInvalidException(string message)
		: base(message)
	{
	}

	public LogDefinitionFileInvalidException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected LogDefinitionFileInvalidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
