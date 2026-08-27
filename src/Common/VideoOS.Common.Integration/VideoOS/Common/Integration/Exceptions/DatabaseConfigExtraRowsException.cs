using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class DatabaseConfigExtraRowsException : AdministrationSubsystemException
{
	public DatabaseConfigExtraRowsException()
		: this(string.Empty)
	{
	}

	public DatabaseConfigExtraRowsException(string message)
		: base(message)
	{
	}

	public DatabaseConfigExtraRowsException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DatabaseConfigExtraRowsException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
