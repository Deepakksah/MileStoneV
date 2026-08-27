using System;
using System.Runtime.Serialization;
using VideoOS.Server.Properties;

namespace VideoOS.Server;

[Serializable]
public class SecurityDatabaseConfigExtraRowsException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 61004;

	public SecurityDatabaseConfigExtraRowsException()
		: this(Resources.DatabaseConfigExtraRows)
	{
	}

	public SecurityDatabaseConfigExtraRowsException(string message)
		: base(message)
	{
	}

	public SecurityDatabaseConfigExtraRowsException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SecurityDatabaseConfigExtraRowsException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
