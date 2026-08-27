using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class BasicUserPasswordException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60453;

	public BasicUserPasswordException()
		: base(VmoResources.VMO60453)
	{
	}

	protected BasicUserPasswordException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
