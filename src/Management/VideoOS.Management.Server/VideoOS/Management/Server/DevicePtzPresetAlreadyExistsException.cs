using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DevicePtzPresetAlreadyExistsException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60199;

	protected override int ErrorCodeDefault => 60199;

	public DevicePtzPresetAlreadyExistsException()
		: base(VmoResources.VMO60199)
	{
	}

	public DevicePtzPresetAlreadyExistsException(string message)
		: base(message)
	{
	}

	public DevicePtzPresetAlreadyExistsException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[SqlError(60199)]
	public DevicePtzPresetAlreadyExistsException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected DevicePtzPresetAlreadyExistsException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
