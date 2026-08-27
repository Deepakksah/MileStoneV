using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class TruncateDeviceDataTaskConflictingTaskAlreadyExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60136;

	protected override int ErrorCodeDefault => 60136;

	public TruncateDeviceDataTaskConflictingTaskAlreadyExistException()
		: base(VmoResources.VMO60136)
	{
	}

	[SqlError(60136)]
	public TruncateDeviceDataTaskConflictingTaskAlreadyExistException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public TruncateDeviceDataTaskConflictingTaskAlreadyExistException(string message)
		: base(message)
	{
	}

	public TruncateDeviceDataTaskConflictingTaskAlreadyExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected TruncateDeviceDataTaskConflictingTaskAlreadyExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
