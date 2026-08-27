using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DevicePtzPresetNotFoundException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60168;

	protected override int ErrorCodeDefault => 60168;

	public DevicePtzPresetNotFoundException()
		: base(VmoResources.VMO60168)
	{
	}

	public DevicePtzPresetNotFoundException(string message)
		: base(message)
	{
	}

	public DevicePtzPresetNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[SqlError(60168)]
	public DevicePtzPresetNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected DevicePtzPresetNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
