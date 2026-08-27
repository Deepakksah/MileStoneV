using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceStreamsNotSupportedException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60225;

	protected override int ErrorCodeDefault => 60225;

	public DeviceStreamsNotSupportedException()
		: base(VmoResources.VMO60225)
	{
	}

	public DeviceStreamsNotSupportedException(string message)
		: base(message)
	{
	}

	public DeviceStreamsNotSupportedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[SqlError(60225)]
	public DeviceStreamsNotSupportedException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected DeviceStreamsNotSupportedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
