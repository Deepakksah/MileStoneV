using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
public class InvalidTaskXmlException : Exception
{
	public InvalidTaskXmlException()
	{
	}

	public InvalidTaskXmlException(string message)
		: base(message)
	{
	}

	public InvalidTaskXmlException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected InvalidTaskXmlException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
