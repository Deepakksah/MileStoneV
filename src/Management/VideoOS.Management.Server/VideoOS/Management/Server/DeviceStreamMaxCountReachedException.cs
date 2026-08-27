using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceStreamMaxCountReachedException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60226;

	protected override int ErrorCodeDefault => 60226;

	public DeviceStreamMaxCountReachedException()
		: base(VmoResources.VMO60226)
	{
	}

	public DeviceStreamMaxCountReachedException(string message)
		: base(message)
	{
	}

	public DeviceStreamMaxCountReachedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[SqlError(60226)]
	public DeviceStreamMaxCountReachedException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected DeviceStreamMaxCountReachedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
