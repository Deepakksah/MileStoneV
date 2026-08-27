using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupAddMembersDeviceNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60177;

	[SqlError(60177)]
	public DeviceGroupAddMembersDeviceNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public DeviceGroupAddMembersDeviceNotFoundException()
	{
	}

	public DeviceGroupAddMembersDeviceNotFoundException(string message)
		: base(message)
	{
	}

	public DeviceGroupAddMembersDeviceNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeviceGroupAddMembersDeviceNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
