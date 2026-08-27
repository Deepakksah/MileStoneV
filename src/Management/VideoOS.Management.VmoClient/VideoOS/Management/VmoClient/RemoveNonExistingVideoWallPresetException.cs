using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RemoveNonExistingVideoWallPresetException : VmoException
{
	public RemoveNonExistingVideoWallPresetException()
	{
	}

	public RemoveNonExistingVideoWallPresetException(string message)
		: base(message)
	{
	}

	public RemoveNonExistingVideoWallPresetException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RemoveNonExistingVideoWallPresetException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
