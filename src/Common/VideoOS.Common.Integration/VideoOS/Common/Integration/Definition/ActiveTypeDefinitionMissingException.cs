using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Definition;

[Serializable]
public class ActiveTypeDefinitionMissingException : Exception
{
	protected ActiveTypeDefinitionMissingException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public ActiveTypeDefinitionMissingException()
	{
	}

	public ActiveTypeDefinitionMissingException(string message)
		: base(message)
	{
	}

	public ActiveTypeDefinitionMissingException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
