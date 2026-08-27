using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

[Serializable]
public class GenericSqlException : VmoServiceException
{
	protected override int ErrorCodeDefault => 60000;

	[SqlError(60000)]
	public GenericSqlException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public GenericSqlException()
	{
	}

	public GenericSqlException(string message)
		: base(message)
	{
	}

	public GenericSqlException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected GenericSqlException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
