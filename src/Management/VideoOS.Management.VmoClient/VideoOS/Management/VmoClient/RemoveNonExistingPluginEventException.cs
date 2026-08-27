using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RemoveNonExistingPluginEventException : VmoException
{
	[ExcludeFromCodeCoverage]
	public RemoveNonExistingPluginEventException()
	{
	}

	[ExcludeFromCodeCoverage]
	public RemoveNonExistingPluginEventException(string message)
		: base(message)
	{
	}

	public RemoveNonExistingPluginEventException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	[ExcludeFromCodeCoverage]
	protected RemoveNonExistingPluginEventException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
