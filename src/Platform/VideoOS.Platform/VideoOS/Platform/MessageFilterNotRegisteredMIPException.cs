using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class MessageFilterNotRegisteredMIPException : MIPException
{
	public MessageFilterNotRegisteredMIPException(string message)
		: base(message)
	{
	}

	public MessageFilterNotRegisteredMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
