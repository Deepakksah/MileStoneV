using System;
using System.Runtime.Serialization;
using VideoOS.Server.Properties;

namespace VideoOS.Server;

[Serializable]
public class DatabaseConfigExtraRowsException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61004;

	public DatabaseConfigExtraRowsException()
		: this(Resources.DatabaseConfigExtraRows)
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
