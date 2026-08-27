using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class VmoFunctionNotSupportedException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60311;

	[SqlError(60311)]
	public VmoFunctionNotSupportedException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public VmoFunctionNotSupportedException()
	{
	}

	public VmoFunctionNotSupportedException(string message)
		: base(message)
	{
	}

	public VmoFunctionNotSupportedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected VmoFunctionNotSupportedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
