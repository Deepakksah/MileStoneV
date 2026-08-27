using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupAddMembersAlreadyExistException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60179;

	[SqlError(60179)]
	public DeviceGroupAddMembersAlreadyExistException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public DeviceGroupAddMembersAlreadyExistException()
	{
	}

	public DeviceGroupAddMembersAlreadyExistException(string message)
		: base(message)
	{
	}

	public DeviceGroupAddMembersAlreadyExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeviceGroupAddMembersAlreadyExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
