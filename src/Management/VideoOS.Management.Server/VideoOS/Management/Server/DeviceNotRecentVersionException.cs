using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceNotRecentVersionException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60198;

	protected override int ErrorCodeDefault => 60198;

	public DeviceNotRecentVersionException()
		: base(VmoResources.VMO60198)
	{
	}

	public DeviceNotRecentVersionException(string message)
		: base(message)
	{
	}

	public DeviceNotRecentVersionException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[SqlError(60198)]
	public DeviceNotRecentVersionException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected DeviceNotRecentVersionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
