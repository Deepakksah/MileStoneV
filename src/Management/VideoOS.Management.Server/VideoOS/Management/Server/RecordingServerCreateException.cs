using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingServerCreateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 65022;

	protected override int ErrorCodeDefault => 65022;

	public RecordingServerCreateException()
		: base(VmoResources.VMO65022)
	{
	}

	[SqlError(65022)]
	public RecordingServerCreateException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public RecordingServerCreateException(string message)
		: base(message)
	{
	}

	public RecordingServerCreateException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingServerCreateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
