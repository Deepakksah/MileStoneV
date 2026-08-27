using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class TaskCleanupNotAllowedException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60158;

	protected override int ErrorCodeDefault => 60158;

	public TaskCleanupNotAllowedException()
		: base(VmoResources.VMO60158)
	{
	}

	[SqlError(60158)]
	public TaskCleanupNotAllowedException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public TaskCleanupNotAllowedException(string message)
		: base(message)
	{
	}

	public TaskCleanupNotAllowedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected TaskCleanupNotAllowedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
