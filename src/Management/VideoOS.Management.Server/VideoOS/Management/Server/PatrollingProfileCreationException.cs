using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class PatrollingProfileCreationException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 64100;

	[SqlError(64100)]
	public PatrollingProfileCreationException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public PatrollingProfileCreationException()
	{
	}

	public PatrollingProfileCreationException(string message)
		: base(message)
	{
	}

	public PatrollingProfileCreationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected PatrollingProfileCreationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
