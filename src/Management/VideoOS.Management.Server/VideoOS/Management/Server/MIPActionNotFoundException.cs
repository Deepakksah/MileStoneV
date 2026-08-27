using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class MIPActionNotFoundException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60471;

	protected override int ErrorCodeDefault => 60471;

	public MIPActionNotFoundException()
		: base(VmoResources.VMO60471)
	{
	}

	public MIPActionNotFoundException(string message)
		: base(message)
	{
	}

	public MIPActionNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected MIPActionNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
