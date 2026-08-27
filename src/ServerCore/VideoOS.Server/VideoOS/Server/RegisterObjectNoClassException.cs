using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

[Serializable]
public class RegisterObjectNoClassException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 60017;

	[SqlError(60017)]
	public RegisterObjectNoClassException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
		base.MessageParameters.Add(0, new MessageParameter(0, "IDClass", databaseError.ExtractString("IDClass")));
	}

	public RegisterObjectNoClassException()
	{
	}

	public RegisterObjectNoClassException(string message)
		: base(message)
	{
	}

	public RegisterObjectNoClassException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RegisterObjectNoClassException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
