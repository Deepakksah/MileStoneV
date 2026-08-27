using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class PluginEventRenameException : VmoException
{
	[ExcludeFromCodeCoverage]
	public PluginEventRenameException()
	{
	}

	[ExcludeFromCodeCoverage]
	public PluginEventRenameException(string message)
		: base(message)
	{
	}

	public PluginEventRenameException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	[ExcludeFromCodeCoverage]
	protected PluginEventRenameException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
