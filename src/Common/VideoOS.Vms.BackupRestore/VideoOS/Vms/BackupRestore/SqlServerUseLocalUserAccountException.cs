using System;
using System.Runtime.Serialization;

namespace VideoOS.Vms.BackupRestore;

[Serializable]
public class SqlServerUseLocalUserAccountException : Exception
{
	public SqlServerUseLocalUserAccountException()
	{
	}

	public SqlServerUseLocalUserAccountException(string message)
		: base(message)
	{
	}

	public SqlServerUseLocalUserAccountException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SqlServerUseLocalUserAccountException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
