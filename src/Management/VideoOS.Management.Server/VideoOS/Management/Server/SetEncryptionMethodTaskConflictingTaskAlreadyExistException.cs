using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class SetEncryptionMethodTaskConflictingTaskAlreadyExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60138;

	protected override int ErrorCodeDefault => 60138;

	public SetEncryptionMethodTaskConflictingTaskAlreadyExistException()
		: base(VmoResources.VMO60138)
	{
	}

	[SqlError(60138)]
	public SetEncryptionMethodTaskConflictingTaskAlreadyExistException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public SetEncryptionMethodTaskConflictingTaskAlreadyExistException(string message)
		: base(message)
	{
	}

	public SetEncryptionMethodTaskConflictingTaskAlreadyExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SetEncryptionMethodTaskConflictingTaskAlreadyExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
