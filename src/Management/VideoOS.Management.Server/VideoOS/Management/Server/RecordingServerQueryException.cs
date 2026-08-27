using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingServerQueryException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60089;

	protected override int ErrorCodeDefault => 60089;

	public RecordingServerQueryException()
		: base(VmoResources.VMO60089)
	{
	}

	[SqlError(60089)]
	public RecordingServerQueryException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public RecordingServerQueryException(string message)
		: base(message)
	{
	}

	public RecordingServerQueryException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingServerQueryException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
