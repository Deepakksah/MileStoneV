using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class TaskCleanupNotAllowedException : VmoException
{
	public TaskCleanupNotAllowedException()
	{
	}

	public TaskCleanupNotAllowedException(string message)
		: base(message)
	{
	}

	public TaskCleanupNotAllowedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected TaskCleanupNotAllowedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
