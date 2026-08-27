using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingServerUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60047;

	protected override int ErrorCodeDefault => 60047;

	public RecordingServerUpdateException()
		: base(VmoResources.VMO60047)
	{
	}

	[SqlError(60047)]
	public RecordingServerUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public RecordingServerUpdateException(string message)
		: base(message)
	{
	}

	public RecordingServerUpdateException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingServerUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
