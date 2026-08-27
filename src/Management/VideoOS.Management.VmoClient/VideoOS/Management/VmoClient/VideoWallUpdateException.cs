using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class VideoWallUpdateException : VmoException
{
	public VideoWallUpdateException()
	{
	}

	public VideoWallUpdateException(string message)
		: base(message)
	{
	}

	public VideoWallUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected VideoWallUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
