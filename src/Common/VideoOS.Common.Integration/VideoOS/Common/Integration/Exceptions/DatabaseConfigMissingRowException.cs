using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class DatabaseConfigMissingRowException : AdministrationSubsystemException
{
	public DatabaseConfigMissingRowException()
		: this(string.Empty)
	{
	}

	public DatabaseConfigMissingRowException(string message)
		: base(message)
	{
	}

	public DatabaseConfigMissingRowException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DatabaseConfigMissingRowException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
