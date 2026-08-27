using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class VideoWallNotFoundException : VmoException
{
	public VideoWallNotFoundException()
	{
	}

	public VideoWallNotFoundException(string message)
		: base(message)
	{
	}

	public VideoWallNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected VideoWallNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
