using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class VideoWallPresetUpdateException : VmoException
{
	public VideoWallPresetUpdateException()
	{
	}

	public VideoWallPresetUpdateException(string message)
		: base(message)
	{
	}

	public VideoWallPresetUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected VideoWallPresetUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
