using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingServerMoveException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60079;

	protected override int ErrorCodeDefault => 60079;

	public RecordingServerMoveException()
		: base(VmoResources.VMO60079)
	{
	}

	[SqlError(60079)]
	public RecordingServerMoveException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public RecordingServerMoveException(string message)
		: base(message)
	{
	}

	public RecordingServerMoveException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingServerMoveException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
