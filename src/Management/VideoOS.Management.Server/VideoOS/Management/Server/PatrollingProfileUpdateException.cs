using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class PatrollingProfileUpdateException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 64102;

	[SqlError(64102)]
	public PatrollingProfileUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public PatrollingProfileUpdateException()
	{
	}

	public PatrollingProfileUpdateException(string message)
		: base(message)
	{
	}

	public PatrollingProfileUpdateException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected PatrollingProfileUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
