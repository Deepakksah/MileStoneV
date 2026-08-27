using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceStreamRemoveNotFoundException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60227;

	protected override int ErrorCodeDefault => 60227;

	public DeviceStreamRemoveNotFoundException()
		: base(VmoResources.VMO60227)
	{
	}

	public DeviceStreamRemoveNotFoundException(string message)
		: base(message)
	{
	}

	public DeviceStreamRemoveNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[SqlError(60227)]
	public DeviceStreamRemoveNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected DeviceStreamRemoveNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
