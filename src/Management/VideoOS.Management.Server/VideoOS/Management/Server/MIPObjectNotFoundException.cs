using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class MIPObjectNotFoundException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60470;

	protected override int ErrorCodeDefault => 60470;

	public MIPObjectNotFoundException()
		: base(VmoResources.VMO60470)
	{
	}

	public MIPObjectNotFoundException(string message)
		: base(message)
	{
	}

	public MIPObjectNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected MIPObjectNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
