using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Threading;

[Serializable]
public sealed class WorkItemResultException : Exception
{
	public WorkItemResultException()
	{
	}

	public WorkItemResultException(string message)
		: base(message)
	{
	}

	public WorkItemResultException(string message, Exception exception)
		: base(message, exception)
	{
	}

	public WorkItemResultException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
