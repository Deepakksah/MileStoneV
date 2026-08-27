using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

[Serializable]
public class RegisterObjectNoParentException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 60018;

	[SqlError(60018)]
	public RegisterObjectNoParentException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
		base.MessageParameters.Add(0, new MessageParameter(0, "IDParentObject", databaseError.ExtractString("IDParentObject")));
	}

	public RegisterObjectNoParentException()
	{
	}

	public RegisterObjectNoParentException(string message)
		: base(message)
	{
	}

	public RegisterObjectNoParentException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RegisterObjectNoParentException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
