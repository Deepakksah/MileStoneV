using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class PatrollingProfileCreationException : VmoException
{
	public PatrollingProfileCreationException()
	{
	}

	public PatrollingProfileCreationException(string message)
		: base(message)
	{
	}

	public PatrollingProfileCreationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected PatrollingProfileCreationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
