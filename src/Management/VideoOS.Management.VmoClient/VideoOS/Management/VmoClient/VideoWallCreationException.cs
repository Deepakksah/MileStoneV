using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class VideoWallCreationException : VmoException
{
	public VideoWallCreationException()
	{
	}

	public VideoWallCreationException(string message)
		: base(message)
	{
	}

	public VideoWallCreationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected VideoWallCreationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
