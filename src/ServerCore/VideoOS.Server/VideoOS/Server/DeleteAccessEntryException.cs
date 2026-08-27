using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

[Serializable]
public class DeleteAccessEntryException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 60024;

	[SqlError(60024)]
	public DeleteAccessEntryException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
		base.MessageParameters.Add(0, new MessageParameter(0, "IDObject", databaseError.ExtractString("IDObject")));
		base.MessageParameters.Add(1, new MessageParameter(1, "IDAction", databaseError.ExtractString("IDAction")));
		base.MessageParameters.Add(2, new MessageParameter(2, "IDUser", databaseError.ExtractString("IDUser")));
	}

	public DeleteAccessEntryException()
	{
	}

	public DeleteAccessEntryException(string message)
		: base(message)
	{
	}

	public DeleteAccessEntryException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeleteAccessEntryException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
