using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

[Serializable]
public class InternalStoredProcedureException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 60008;

	[SqlError(60008)]
	public InternalStoredProcedureException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public InternalStoredProcedureException()
	{
	}

	public InternalStoredProcedureException(string message)
		: base(message)
	{
	}

	public InternalStoredProcedureException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected InternalStoredProcedureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
