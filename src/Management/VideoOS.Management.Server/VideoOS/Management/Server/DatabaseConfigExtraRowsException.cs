using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class DatabaseConfigExtraRowsException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61004;

	public DatabaseConfigExtraRowsException()
		: this(VmoResources.VMO61004)
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
