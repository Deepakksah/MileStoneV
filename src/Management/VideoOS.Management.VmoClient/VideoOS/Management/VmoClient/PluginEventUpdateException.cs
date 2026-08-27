using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class PluginEventUpdateException : VmoException
{
	[ExcludeFromCodeCoverage]
	public PluginEventUpdateException()
	{
	}

	[ExcludeFromCodeCoverage]
	public PluginEventUpdateException(string message)
		: base(message)
	{
	}

	public PluginEventUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	[ExcludeFromCodeCoverage]
	protected PluginEventUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
