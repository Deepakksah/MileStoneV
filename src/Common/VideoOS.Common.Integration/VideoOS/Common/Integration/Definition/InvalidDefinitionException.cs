using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Definition;

[Serializable]
public class InvalidDefinitionException : Exception
{
	public InvalidDefinitionException()
	{
	}

	public InvalidDefinitionException(string message)
		: base(message)
	{
	}

	public InvalidDefinitionException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected InvalidDefinitionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
