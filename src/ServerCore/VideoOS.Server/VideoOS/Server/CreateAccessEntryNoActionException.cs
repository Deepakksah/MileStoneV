using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

[Serializable]
public class CreateAccessEntryNoActionException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 60022;

	[SqlError(60022)]
	public CreateAccessEntryNoActionException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public CreateAccessEntryNoActionException()
	{
	}

	public CreateAccessEntryNoActionException(string message)
		: base(message)
	{
	}

	public CreateAccessEntryNoActionException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected CreateAccessEntryNoActionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
