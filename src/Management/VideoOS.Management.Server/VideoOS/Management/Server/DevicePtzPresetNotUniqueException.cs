using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DevicePtzPresetNotUniqueException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60169;

	protected override int ErrorCodeDefault => 60169;

	public DevicePtzPresetNotUniqueException()
		: base(VmoResources.VMO60169)
	{
	}

	public DevicePtzPresetNotUniqueException(string message)
		: base(message)
	{
	}

	public DevicePtzPresetNotUniqueException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[SqlError(60169)]
	public DevicePtzPresetNotUniqueException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected DevicePtzPresetNotUniqueException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
