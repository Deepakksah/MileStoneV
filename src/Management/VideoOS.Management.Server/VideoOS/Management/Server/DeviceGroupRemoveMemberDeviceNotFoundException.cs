using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupRemoveMemberDeviceNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60182;

	[SqlError(60182)]
	public DeviceGroupRemoveMemberDeviceNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public DeviceGroupRemoveMemberDeviceNotFoundException()
	{
	}

	public DeviceGroupRemoveMemberDeviceNotFoundException(string message)
		: base(message)
	{
	}

	public DeviceGroupRemoveMemberDeviceNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeviceGroupRemoveMemberDeviceNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
