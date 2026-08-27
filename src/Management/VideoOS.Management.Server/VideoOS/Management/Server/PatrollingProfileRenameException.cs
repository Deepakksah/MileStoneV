using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class PatrollingProfileRenameException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 64103;

	[SqlError(64103)]
	public PatrollingProfileRenameException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public PatrollingProfileRenameException()
	{
	}

	public PatrollingProfileRenameException(string message)
		: base(message)
	{
	}

	public PatrollingProfileRenameException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected PatrollingProfileRenameException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
