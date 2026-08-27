using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class DatabaseConfigMissingRowException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61005;

	public DatabaseConfigMissingRowException()
		: this(VmoResources.VMO61005)
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
