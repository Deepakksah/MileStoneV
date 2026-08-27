using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UserDefinedEventCreationException : VmoException
{
	[ExcludeFromCodeCoverage]
	public UserDefinedEventCreationException()
	{
	}

	[ExcludeFromCodeCoverage]
	public UserDefinedEventCreationException(string message)
		: base(message)
	{
	}

	public UserDefinedEventCreationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	[ExcludeFromCodeCoverage]
	protected UserDefinedEventCreationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
