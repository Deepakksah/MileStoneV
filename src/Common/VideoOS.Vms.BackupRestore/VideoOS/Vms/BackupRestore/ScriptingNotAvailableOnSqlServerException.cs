using System;
using System.Runtime.Serialization;

namespace VideoOS.Vms.BackupRestore;

[Serializable]
public class ScriptingNotAvailableOnSqlServerException : Exception
{
	public ScriptingNotAvailableOnSqlServerException()
	{
	}

	public ScriptingNotAvailableOnSqlServerException(string message)
		: base(message)
	{
	}

	public ScriptingNotAvailableOnSqlServerException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ScriptingNotAvailableOnSqlServerException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
