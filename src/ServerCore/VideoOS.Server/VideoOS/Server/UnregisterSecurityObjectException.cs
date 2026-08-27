using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

[Serializable]
public class UnregisterSecurityObjectException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 65001;

	[SqlError(65001)]
	public UnregisterSecurityObjectException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public UnregisterSecurityObjectException()
	{
	}

	public UnregisterSecurityObjectException(string message)
		: base(message)
	{
	}

	public UnregisterSecurityObjectException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UnregisterSecurityObjectException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
