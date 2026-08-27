using System;
using System.Runtime.Serialization;

namespace VideoOS.Vms.BackupRestore;

[Serializable]
public class DatabaseInUseSqlException : Exception
{
	public DatabaseInUseSqlException()
	{
	}

	public DatabaseInUseSqlException(string message)
		: base(message)
	{
	}

	public DatabaseInUseSqlException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DatabaseInUseSqlException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
