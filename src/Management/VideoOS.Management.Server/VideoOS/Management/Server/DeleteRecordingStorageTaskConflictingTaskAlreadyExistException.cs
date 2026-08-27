using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeleteRecordingStorageTaskConflictingTaskAlreadyExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60134;

	protected override int ErrorCodeDefault => 60134;

	public DeleteRecordingStorageTaskConflictingTaskAlreadyExistException()
		: base(VmoResources.VMO60134)
	{
	}

	[SqlError(60134)]
	public DeleteRecordingStorageTaskConflictingTaskAlreadyExistException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public DeleteRecordingStorageTaskConflictingTaskAlreadyExistException(string message)
		: base(message)
	{
	}

	public DeleteRecordingStorageTaskConflictingTaskAlreadyExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeleteRecordingStorageTaskConflictingTaskAlreadyExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
