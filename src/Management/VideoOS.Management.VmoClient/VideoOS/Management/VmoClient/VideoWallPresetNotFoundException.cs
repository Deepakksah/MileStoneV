using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class VideoWallPresetNotFoundException : VmoException
{
	public VideoWallPresetNotFoundException()
	{
	}

	public VideoWallPresetNotFoundException(string message)
		: base(message)
	{
	}

	public VideoWallPresetNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected VideoWallPresetNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
