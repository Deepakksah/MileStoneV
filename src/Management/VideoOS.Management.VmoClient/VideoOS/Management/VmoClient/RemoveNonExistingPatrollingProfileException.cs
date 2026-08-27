using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RemoveNonExistingPatrollingProfileException : VmoException
{
	public RemoveNonExistingPatrollingProfileException()
	{
	}

	public RemoveNonExistingPatrollingProfileException(string message)
		: base(message)
	{
	}

	public RemoveNonExistingPatrollingProfileException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RemoveNonExistingPatrollingProfileException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
