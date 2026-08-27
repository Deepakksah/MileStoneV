using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

[Serializable]
public class CreateAccessEntryNoObjectException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 60021;

	[SqlError(60021)]
	public CreateAccessEntryNoObjectException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
		base.MessageParameters.Add(0, new MessageParameter(0, "IDObject", databaseError.ExtractString("IDObject")));
	}

	public CreateAccessEntryNoObjectException()
	{
	}

	public CreateAccessEntryNoObjectException(string message)
		: base(message)
	{
	}

	public CreateAccessEntryNoObjectException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected CreateAccessEntryNoObjectException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
