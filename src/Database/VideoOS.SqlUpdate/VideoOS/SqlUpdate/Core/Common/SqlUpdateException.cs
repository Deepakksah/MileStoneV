using System;
using System.Runtime.Serialization;

namespace VideoOS.SqlUpdate.Core.Common;

[Serializable]
public class SqlUpdateException : Exception
{
	public SqlUpdateException()
	{
	}

	public SqlUpdateException(string message)
		: base(message)
	{
	}

	public SqlUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SqlUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
