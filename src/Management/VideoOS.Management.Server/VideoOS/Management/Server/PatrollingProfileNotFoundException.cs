using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class PatrollingProfileNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 64105;

	[SqlError(64105)]
	public PatrollingProfileNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public PatrollingProfileNotFoundException()
	{
	}

	public PatrollingProfileNotFoundException(string message)
		: base(message)
	{
	}

	public PatrollingProfileNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected PatrollingProfileNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
