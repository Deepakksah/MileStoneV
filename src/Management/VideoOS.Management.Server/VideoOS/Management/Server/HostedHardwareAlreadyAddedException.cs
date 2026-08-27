using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HostedHardwareAlreadyAddedException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60616;

	protected override int ErrorCodeDefault => 60616;

	public HostedHardwareAlreadyAddedException()
		: base(VmoResources.VMO60613)
	{
	}

	[SqlError(60616)]
	public HostedHardwareAlreadyAddedException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public HostedHardwareAlreadyAddedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HostedHardwareAlreadyAddedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
