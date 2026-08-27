using System;
using System.Runtime.Serialization;

namespace VideoOS.Vms.BackupRestore;

[Serializable]
public class CreateShareException : Exception
{
	public CreateShareException()
	{
	}

	public CreateShareException(string message)
		: base(message)
	{
	}

	public CreateShareException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected CreateShareException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
