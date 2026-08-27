using System;
using System.Runtime.Serialization;
using VideoOS.Server.Properties;

namespace VideoOS.Server;

[Serializable]
public class SecurityDatabaseConfigMissingRowException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 61005;

	public SecurityDatabaseConfigMissingRowException()
		: this(Resources.DatabaseConfigMissingRow)
	{
	}

	public SecurityDatabaseConfigMissingRowException(string message)
		: base(message)
	{
	}

	public SecurityDatabaseConfigMissingRowException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SecurityDatabaseConfigMissingRowException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
