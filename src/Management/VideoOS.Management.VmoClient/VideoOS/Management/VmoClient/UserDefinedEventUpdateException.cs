using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UserDefinedEventUpdateException : VmoException
{
	[ExcludeFromCodeCoverage]
	public UserDefinedEventUpdateException()
	{
	}

	[ExcludeFromCodeCoverage]
	public UserDefinedEventUpdateException(string message)
		: base(message)
	{
	}

	public UserDefinedEventUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	[ExcludeFromCodeCoverage]
	protected UserDefinedEventUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
