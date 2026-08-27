using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Definition;

[Serializable]
public class SchemaViolationException : Exception
{
	public SchemaViolationException()
	{
	}

	public SchemaViolationException(string message)
		: base(message)
	{
	}

	public SchemaViolationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public SchemaViolationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
