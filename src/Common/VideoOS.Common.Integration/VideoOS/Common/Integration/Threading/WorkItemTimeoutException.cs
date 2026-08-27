using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Threading;

[Serializable]
public sealed class WorkItemTimeoutException : Exception
{
	public WorkItemTimeoutException()
	{
	}

	public WorkItemTimeoutException(string message)
		: base(message)
	{
	}

	public WorkItemTimeoutException(string message, Exception exception)
		: base(message, exception)
	{
	}

	public WorkItemTimeoutException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
