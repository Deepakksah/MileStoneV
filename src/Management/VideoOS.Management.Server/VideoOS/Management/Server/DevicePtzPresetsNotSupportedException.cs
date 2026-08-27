using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DevicePtzPresetsNotSupportedException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60167;

	protected override int ErrorCodeDefault => 60167;

	public DevicePtzPresetsNotSupportedException()
		: base(VmoResources.VMO60167)
	{
	}

	public DevicePtzPresetsNotSupportedException(string message)
		: base(message)
	{
	}

	public DevicePtzPresetsNotSupportedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[SqlError(60167)]
	public DevicePtzPresetsNotSupportedException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected DevicePtzPresetsNotSupportedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
