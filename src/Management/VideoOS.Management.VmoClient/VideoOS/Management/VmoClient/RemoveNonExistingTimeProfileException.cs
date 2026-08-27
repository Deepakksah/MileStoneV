using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RemoveNonExistingTimeProfileException : VmoException
{
	public RemoveNonExistingTimeProfileException()
	{
	}

	public RemoveNonExistingTimeProfileException(string message)
		: base(message)
	{
	}

	public RemoveNonExistingTimeProfileException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RemoveNonExistingTimeProfileException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
