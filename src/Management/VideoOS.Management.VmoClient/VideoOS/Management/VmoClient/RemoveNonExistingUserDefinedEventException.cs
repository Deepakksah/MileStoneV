using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RemoveNonExistingUserDefinedEventException : VmoException
{
	public RemoveNonExistingUserDefinedEventException()
	{
	}

	public RemoveNonExistingUserDefinedEventException(string message)
		: base(message)
	{
	}

	public RemoveNonExistingUserDefinedEventException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RemoveNonExistingUserDefinedEventException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
