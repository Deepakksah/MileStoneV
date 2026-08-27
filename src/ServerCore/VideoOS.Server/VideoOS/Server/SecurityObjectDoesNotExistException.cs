using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

[Serializable]
public class SecurityObjectDoesNotExistException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 60012;

	[SqlError(60012)]
	public SecurityObjectDoesNotExistException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
		base.MessageParameters.Add(0, new MessageParameter(0, "IDObject", databaseError.ExtractString("IDObject")));
	}

	public SecurityObjectDoesNotExistException()
	{
	}

	public SecurityObjectDoesNotExistException(string message)
		: base(message)
	{
	}

	public SecurityObjectDoesNotExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SecurityObjectDoesNotExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
