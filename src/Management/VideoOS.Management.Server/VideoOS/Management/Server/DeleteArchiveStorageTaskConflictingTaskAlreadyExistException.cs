using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeleteArchiveStorageTaskConflictingTaskAlreadyExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60135;

	protected override int ErrorCodeDefault => 60135;

	public DeleteArchiveStorageTaskConflictingTaskAlreadyExistException()
		: base(VmoResources.VMO60135)
	{
	}

	[SqlError(60135)]
	public DeleteArchiveStorageTaskConflictingTaskAlreadyExistException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public DeleteArchiveStorageTaskConflictingTaskAlreadyExistException(string message)
		: base(message)
	{
	}

	public DeleteArchiveStorageTaskConflictingTaskAlreadyExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeleteArchiveStorageTaskConflictingTaskAlreadyExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
