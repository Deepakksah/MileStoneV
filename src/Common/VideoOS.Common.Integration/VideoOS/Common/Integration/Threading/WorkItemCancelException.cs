using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Threading;

[Serializable]
public sealed class WorkItemCancelException : Exception
{
	public WorkItemCancelException()
	{
	}

	public WorkItemCancelException(string message)
		: base(message)
	{
	}

	public WorkItemCancelException(string message, Exception exception)
		: base(message, exception)
	{
	}

	public WorkItemCancelException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
