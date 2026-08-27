using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UserDefinedEventRenameException : VmoException
{
	[ExcludeFromCodeCoverage]
	public UserDefinedEventRenameException()
	{
	}

	[ExcludeFromCodeCoverage]
	public UserDefinedEventRenameException(string message)
		: base(message)
	{
	}

	public UserDefinedEventRenameException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	[ExcludeFromCodeCoverage]
	protected UserDefinedEventRenameException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
