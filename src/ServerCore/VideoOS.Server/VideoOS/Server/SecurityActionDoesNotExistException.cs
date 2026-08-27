using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

[Serializable]
public class SecurityActionDoesNotExistException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 60013;

	[SqlError(60013)]
	public SecurityActionDoesNotExistException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
		base.MessageParameters.Add(0, new MessageParameter(0, "IDAction", databaseError.ExtractString("IDAction")));
	}

	public SecurityActionDoesNotExistException()
	{
	}

	public SecurityActionDoesNotExistException(string message)
		: base(message)
	{
	}

	public SecurityActionDoesNotExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SecurityActionDoesNotExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
