using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class MoveDeviceStorageTaskConflictingTaskAlreadyExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60137;

	protected override int ErrorCodeDefault => 60137;

	public MoveDeviceStorageTaskConflictingTaskAlreadyExistException()
		: base(VmoResources.VMO60137)
	{
	}

	[SqlError(60137)]
	public MoveDeviceStorageTaskConflictingTaskAlreadyExistException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public MoveDeviceStorageTaskConflictingTaskAlreadyExistException(string message)
		: base(message)
	{
	}

	public MoveDeviceStorageTaskConflictingTaskAlreadyExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected MoveDeviceStorageTaskConflictingTaskAlreadyExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
