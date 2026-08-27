using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class MoveRecordingStorageTaskConflictingTaskAlreadyExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60132;

	protected override int ErrorCodeDefault => 60132;

	public MoveRecordingStorageTaskConflictingTaskAlreadyExistException()
		: base(VmoResources.VMO60132)
	{
	}

	[SqlError(60132)]
	public MoveRecordingStorageTaskConflictingTaskAlreadyExistException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public MoveRecordingStorageTaskConflictingTaskAlreadyExistException(string message)
		: base(message)
	{
	}

	public MoveRecordingStorageTaskConflictingTaskAlreadyExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected MoveRecordingStorageTaskConflictingTaskAlreadyExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
