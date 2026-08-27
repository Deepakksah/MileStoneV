using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Definition;

[Serializable]
public class IncompatibleDefinitionException : Exception
{
	public IncompatibleDefinitionException()
	{
	}

	public IncompatibleDefinitionException(string message)
		: base(message)
	{
	}

	public IncompatibleDefinitionException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public IncompatibleDefinitionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
