using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveNonExistingPatrollingProfileException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 64104;

	[SqlError(64104)]
	public RemoveNonExistingPatrollingProfileException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public RemoveNonExistingPatrollingProfileException()
	{
	}

	public RemoveNonExistingPatrollingProfileException(string message)
		: base(message)
	{
	}

	public RemoveNonExistingPatrollingProfileException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RemoveNonExistingPatrollingProfileException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
