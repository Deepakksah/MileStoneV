using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class SetEncryptionPasswordTaskConflictingTaskAlreadyExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60139;

	protected override int ErrorCodeDefault => 60139;

	public SetEncryptionPasswordTaskConflictingTaskAlreadyExistException()
		: base(VmoResources.VMO60139)
	{
	}

	[SqlError(60139)]
	public SetEncryptionPasswordTaskConflictingTaskAlreadyExistException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public SetEncryptionPasswordTaskConflictingTaskAlreadyExistException(string message)
		: base(message)
	{
	}

	public SetEncryptionPasswordTaskConflictingTaskAlreadyExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SetEncryptionPasswordTaskConflictingTaskAlreadyExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
