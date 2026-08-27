using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class SecurityDatabaseConfigMissingRowException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 61005;

	public SecurityDatabaseConfigMissingRowException()
		: this(VmoResources.VMO61005)
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
