using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Threading;

[Serializable]
public class TaskFailedException : Exception
{
	public TaskFailedException()
	{
	}

	public TaskFailedException(string message)
		: base(message)
	{
	}

	public TaskFailedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected TaskFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
