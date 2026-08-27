using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class PluginEventNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60480;

	public PluginEventNotFoundException()
		: base(VmoResources.VMO60480)
	{
	}

	[ExcludeFromCodeCoverage]
	public PluginEventNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[ExcludeFromCodeCoverage]
	protected PluginEventNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
