using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class PatrollingProfileNotFoundException : VmoException
{
	public PatrollingProfileNotFoundException()
	{
	}

	public PatrollingProfileNotFoundException(string message)
		: base(message)
	{
	}

	public PatrollingProfileNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected PatrollingProfileNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
