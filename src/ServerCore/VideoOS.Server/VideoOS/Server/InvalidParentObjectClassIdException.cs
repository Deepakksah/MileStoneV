using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

[Serializable]
public class InvalidParentObjectClassIdException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 60019;

	[SqlError(60019)]
	public InvalidParentObjectClassIdException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
		base.MessageParameters.Add(0, new MessageParameter(0, "IDObject", databaseError.ExtractString("IDObject")));
		base.MessageParameters.Add(1, new MessageParameter(1, "IDParentObject", databaseError.ExtractString("IDParentObject")));
	}

	public InvalidParentObjectClassIdException()
	{
	}

	public InvalidParentObjectClassIdException(string message)
		: base(message)
	{
	}

	public InvalidParentObjectClassIdException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected InvalidParentObjectClassIdException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
