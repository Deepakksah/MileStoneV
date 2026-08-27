using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Definition;

[Serializable]
public class NoDefinitionChangesException : Exception
{
	public NoDefinitionChangesException()
	{
	}

	public NoDefinitionChangesException(string message)
		: base(message)
	{
	}

	public NoDefinitionChangesException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected NoDefinitionChangesException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
