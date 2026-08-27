using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RemoveNonExistingViewException : VmoException
{
	public RemoveNonExistingViewException()
	{
	}

	public RemoveNonExistingViewException(string message)
		: base(message)
	{
	}

	public RemoveNonExistingViewException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RemoveNonExistingViewException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
