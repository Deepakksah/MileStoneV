using System;
using System.Runtime.Serialization;

namespace VideoOS.Vms.BackupRestore;

[Serializable]
public class BackupSqlDataException : Exception
{
	public BackupSqlDataException()
	{
	}

	public BackupSqlDataException(string message)
		: base(message)
	{
	}

	public BackupSqlDataException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected BackupSqlDataException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
