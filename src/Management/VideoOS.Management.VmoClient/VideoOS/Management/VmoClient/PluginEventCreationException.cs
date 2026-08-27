using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class PluginEventCreationException : VmoException
{
	[ExcludeFromCodeCoverage]
	public PluginEventCreationException()
	{
	}

	[ExcludeFromCodeCoverage]
	public PluginEventCreationException(string message)
		: base(message)
	{
	}

	public PluginEventCreationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	[ExcludeFromCodeCoverage]
	protected PluginEventCreationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
