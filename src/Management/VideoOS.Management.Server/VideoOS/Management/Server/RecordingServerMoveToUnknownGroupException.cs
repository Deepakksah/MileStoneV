using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingServerMoveToUnknownGroupException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60054;

	protected override int ErrorCodeDefault => 60054;

	public RecordingServerMoveToUnknownGroupException()
		: base(VmoResources.VMO60054)
	{
	}

	[SqlError(60054)]
	public RecordingServerMoveToUnknownGroupException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public RecordingServerMoveToUnknownGroupException(string message)
		: base(message)
	{
	}

	public RecordingServerMoveToUnknownGroupException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingServerMoveToUnknownGroupException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
