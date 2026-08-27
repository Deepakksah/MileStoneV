using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupRemoveMemberReadOnlyGroupException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60181;

	[SqlError(60181)]
	public DeviceGroupRemoveMemberReadOnlyGroupException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public DeviceGroupRemoveMemberReadOnlyGroupException()
	{
	}

	public DeviceGroupRemoveMemberReadOnlyGroupException(string message)
		: base(message)
	{
	}

	public DeviceGroupRemoveMemberReadOnlyGroupException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeviceGroupRemoveMemberReadOnlyGroupException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
