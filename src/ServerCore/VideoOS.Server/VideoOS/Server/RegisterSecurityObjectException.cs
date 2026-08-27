using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

[Serializable]
public class RegisterSecurityObjectException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 65000;

	[SqlError(65000)]
	public RegisterSecurityObjectException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public RegisterSecurityObjectException()
	{
	}

	public RegisterSecurityObjectException(string message)
		: base(message)
	{
	}

	public RegisterSecurityObjectException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RegisterSecurityObjectException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
