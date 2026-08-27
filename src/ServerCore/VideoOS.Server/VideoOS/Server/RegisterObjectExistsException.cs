using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

[Serializable]
public class RegisterObjectExistsException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 60016;

	[SqlError(60016)]
	public RegisterObjectExistsException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
		base.MessageParameters.Add(0, new MessageParameter(0, "IDObject", databaseError.ExtractString("IDObject")));
	}

	public RegisterObjectExistsException()
	{
	}

	public RegisterObjectExistsException(string message)
		: base(message)
	{
	}

	public RegisterObjectExistsException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RegisterObjectExistsException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
