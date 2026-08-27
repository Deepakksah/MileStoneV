using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class SecurityDatabaseConfigExtraRowsException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 61004;

	public SecurityDatabaseConfigExtraRowsException()
		: this(VmoResources.VMO61004)
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
