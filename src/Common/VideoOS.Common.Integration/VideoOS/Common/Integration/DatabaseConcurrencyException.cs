using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class DatabaseConcurrencyException : ServerServiceException
{
	public DatabaseConcurrencyException()
	{
	}

	public DatabaseConcurrencyException(string message)
		: base(message)
	{
	}

	public DatabaseConcurrencyException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected DatabaseConcurrencyException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
