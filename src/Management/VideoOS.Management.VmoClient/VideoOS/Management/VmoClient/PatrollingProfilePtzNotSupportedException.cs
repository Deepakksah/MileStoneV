using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class PatrollingProfilePtzNotSupportedException : VmoException
{
	public PatrollingProfilePtzNotSupportedException()
	{
	}

	public PatrollingProfilePtzNotSupportedException(string message)
		: base(message)
	{
	}

	public PatrollingProfilePtzNotSupportedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected PatrollingProfilePtzNotSupportedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
