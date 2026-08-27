using System;
using System.Runtime.Serialization;

namespace VideoOS.Vms.BackupRestore;

[Serializable]
public class CreateShareUnknownErrorException : CreateShareException
{
	public CreateShareUnknownErrorException()
	{
	}

	public CreateShareUnknownErrorException(string message)
		: base(message)
	{
	}

	public CreateShareUnknownErrorException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected CreateShareUnknownErrorException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
