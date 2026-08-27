using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RemoveNonExistingVideoWallException : VmoException
{
	public RemoveNonExistingVideoWallException()
	{
	}

	public RemoveNonExistingVideoWallException(string message)
		: base(message)
	{
	}

	public RemoveNonExistingVideoWallException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RemoveNonExistingVideoWallException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
