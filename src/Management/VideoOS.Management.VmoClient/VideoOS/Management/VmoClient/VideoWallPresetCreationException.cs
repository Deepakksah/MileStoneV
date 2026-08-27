using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class VideoWallPresetCreationException : VmoException
{
	public VideoWallPresetCreationException()
	{
	}

	public VideoWallPresetCreationException(string message)
		: base(message)
	{
	}

	public VideoWallPresetCreationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected VideoWallPresetCreationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
