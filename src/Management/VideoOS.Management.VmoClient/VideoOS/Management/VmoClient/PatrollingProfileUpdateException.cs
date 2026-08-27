using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class PatrollingProfileUpdateException : VmoException
{
	public PatrollingProfileUpdateException()
	{
	}

	public PatrollingProfileUpdateException(string message)
		: base(message)
	{
	}

	public PatrollingProfileUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected PatrollingProfileUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
