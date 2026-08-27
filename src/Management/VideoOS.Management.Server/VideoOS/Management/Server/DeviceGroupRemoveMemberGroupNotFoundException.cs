using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupRemoveMemberGroupNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60180;

	[SqlError(60180)]
	public DeviceGroupRemoveMemberGroupNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public DeviceGroupRemoveMemberGroupNotFoundException()
	{
	}

	public DeviceGroupRemoveMemberGroupNotFoundException(string message)
		: base(message)
	{
	}

	public DeviceGroupRemoveMemberGroupNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeviceGroupRemoveMemberGroupNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
