using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class MoveArchiveStorageTaskConflictingTaskAlreadyExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60133;

	protected override int ErrorCodeDefault => 60133;

	public MoveArchiveStorageTaskConflictingTaskAlreadyExistException()
		: base(VmoResources.VMO60133)
	{
	}

	[SqlError(60133)]
	public MoveArchiveStorageTaskConflictingTaskAlreadyExistException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public MoveArchiveStorageTaskConflictingTaskAlreadyExistException(string message)
		: base(message)
	{
	}

	public MoveArchiveStorageTaskConflictingTaskAlreadyExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected MoveArchiveStorageTaskConflictingTaskAlreadyExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
