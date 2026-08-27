using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupAddMembersGroupNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60175;

	[SqlError(60175)]
	public DeviceGroupAddMembersGroupNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public DeviceGroupAddMembersGroupNotFoundException()
	{
	}

	public DeviceGroupAddMembersGroupNotFoundException(string message)
		: base(message)
	{
	}

	public DeviceGroupAddMembersGroupNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeviceGroupAddMembersGroupNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
