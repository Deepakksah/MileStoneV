using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class PatrollingProfileRenameException : VmoException
{
	public PatrollingProfileRenameException()
	{
	}

	public PatrollingProfileRenameException(string message)
		: base(message)
	{
	}

	public PatrollingProfileRenameException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected PatrollingProfileRenameException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
