using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupAddMembersToReadOnlyGroupException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60176;

	[SqlError(60176)]
	public DeviceGroupAddMembersToReadOnlyGroupException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public DeviceGroupAddMembersToReadOnlyGroupException()
	{
	}

	public DeviceGroupAddMembersToReadOnlyGroupException(string message)
		: base(message)
	{
	}

	public DeviceGroupAddMembersToReadOnlyGroupException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeviceGroupAddMembersToReadOnlyGroupException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
