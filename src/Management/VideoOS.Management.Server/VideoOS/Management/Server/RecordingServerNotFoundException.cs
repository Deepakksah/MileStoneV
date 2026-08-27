using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingServerNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61013;

	[SqlError(61013)]
	[SqlError(60248)]
	[SqlError(60251)]
	public RecordingServerNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public RecordingServerNotFoundException()
		: base(VmoResources.VMO61013)
	{
	}

	public RecordingServerNotFoundException(string message)
		: base(message)
	{
	}

	public RecordingServerNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingServerNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
