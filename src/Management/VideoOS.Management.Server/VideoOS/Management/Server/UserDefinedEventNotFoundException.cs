using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UserDefinedEventNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61029;

	public UserDefinedEventNotFoundException()
		: base(VmoResources.VMO61029)
	{
	}

	[ExcludeFromCodeCoverage]
	public UserDefinedEventNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[ExcludeFromCodeCoverage]
	protected UserDefinedEventNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
