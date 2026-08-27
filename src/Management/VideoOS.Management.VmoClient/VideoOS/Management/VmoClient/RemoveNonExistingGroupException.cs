using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RemoveNonExistingGroupException : VmoException
{
	public RemoveNonExistingGroupException()
	{
	}

	public RemoveNonExistingGroupException(string message)
		: base(message)
	{
	}

	public RemoveNonExistingGroupException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RemoveNonExistingGroupException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
