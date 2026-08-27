using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class BasicUserPasswordSameAsOldException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60456;

	public BasicUserPasswordSameAsOldException()
		: base(VmoResources.VMO60456)
	{
	}

	protected BasicUserPasswordSameAsOldException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
