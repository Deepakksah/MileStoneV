using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class PluginEventNotFoundException : VmoException
{
	[ExcludeFromCodeCoverage]
	public PluginEventNotFoundException()
	{
	}

	[ExcludeFromCodeCoverage]
	public PluginEventNotFoundException(string message)
		: base(message)
	{
	}

	public PluginEventNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	[ExcludeFromCodeCoverage]
	protected PluginEventNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
