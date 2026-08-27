using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UserDefinedEventNotFoundException : VmoException
{
	[ExcludeFromCodeCoverage]
	public UserDefinedEventNotFoundException()
	{
	}

	[ExcludeFromCodeCoverage]
	public UserDefinedEventNotFoundException(string message)
		: base(message)
	{
	}

	public UserDefinedEventNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	[ExcludeFromCodeCoverage]
	protected UserDefinedEventNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
