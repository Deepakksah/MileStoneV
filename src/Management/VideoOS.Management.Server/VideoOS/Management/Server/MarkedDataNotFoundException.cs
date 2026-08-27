using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class MarkedDataNotFoundException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60163;

	protected override int ErrorCodeDefault => 60163;

	public MarkedDataNotFoundException()
		: this(VmoResources.VMO60163)
	{
	}

	public MarkedDataNotFoundException(string message)
		: base(message)
	{
	}

	public MarkedDataNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected MarkedDataNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
