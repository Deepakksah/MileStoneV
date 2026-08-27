using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DevicePtzPresetNotUpdatableException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60197;

	protected override int ErrorCodeDefault => 60197;

	public DevicePtzPresetNotUpdatableException()
		: base(VmoResources.VMO60197)
	{
	}

	public DevicePtzPresetNotUpdatableException(string message)
		: base(message)
	{
	}

	public DevicePtzPresetNotUpdatableException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[SqlError(60197)]
	public DevicePtzPresetNotUpdatableException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected DevicePtzPresetNotUpdatableException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
