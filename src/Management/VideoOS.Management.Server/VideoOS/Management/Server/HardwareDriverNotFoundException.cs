using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class HardwareDriverNotFoundException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60157;

	protected override int ErrorCodeDefault => 60157;

	public HardwareDriverNotFoundException()
		: this(VmoResources.VMO60157)
	{
	}

	public HardwareDriverNotFoundException(string message)
		: base(message)
	{
	}

	public HardwareDriverNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HardwareDriverNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
