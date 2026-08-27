using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class PatrollingProfilePtzNotSupportedException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 64101;

	[SqlError(64101)]
	public PatrollingProfilePtzNotSupportedException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public PatrollingProfilePtzNotSupportedException()
	{
	}

	public PatrollingProfilePtzNotSupportedException(string message)
		: base(message)
	{
	}

	public PatrollingProfilePtzNotSupportedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected PatrollingProfilePtzNotSupportedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
