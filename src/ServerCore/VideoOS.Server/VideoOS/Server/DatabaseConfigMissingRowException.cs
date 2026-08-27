using System;
using System.Runtime.Serialization;
using VideoOS.Server.Properties;

namespace VideoOS.Server;

[Serializable]
public class DatabaseConfigMissingRowException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61005;

	public DatabaseConfigMissingRowException()
		: this(Resources.DatabaseConfigMissingRow)
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
