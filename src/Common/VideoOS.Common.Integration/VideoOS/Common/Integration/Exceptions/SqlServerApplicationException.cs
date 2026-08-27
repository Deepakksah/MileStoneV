using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class SqlServerApplicationException : CorporateApplicationException
{
	public SqlServerApplicationException()
	{
	}

	public SqlServerApplicationException(string message)
		: base(message)
	{
	}

	public SqlServerApplicationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SqlServerApplicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
