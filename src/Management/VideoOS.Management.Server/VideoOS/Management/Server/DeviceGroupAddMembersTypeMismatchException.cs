using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupAddMembersTypeMismatchException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60178;

	[SqlError(60178)]
	public DeviceGroupAddMembersTypeMismatchException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public DeviceGroupAddMembersTypeMismatchException()
	{
	}

	public DeviceGroupAddMembersTypeMismatchException(string message)
		: base(message)
	{
	}

	public DeviceGroupAddMembersTypeMismatchException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeviceGroupAddMembersTypeMismatchException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
