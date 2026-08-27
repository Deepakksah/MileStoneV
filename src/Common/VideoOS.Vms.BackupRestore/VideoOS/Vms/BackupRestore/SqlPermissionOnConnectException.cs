using System;
using System.Runtime.Serialization;

namespace VideoOS.Vms.BackupRestore;

[Serializable]
public class SqlPermissionOnConnectException : Exception
{
	public SqlPermissionOnConnectException()
	{
	}

	public SqlPermissionOnConnectException(string message)
		: base(message)
	{
	}

	public SqlPermissionOnConnectException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SqlPermissionOnConnectException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
