using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class VideoWallPresetNoVideoWallException : VmoException
{
	public VideoWallPresetNoVideoWallException()
	{
	}

	public VideoWallPresetNoVideoWallException(string message)
		: base(message)
	{
	}

	public VideoWallPresetNoVideoWallException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected VideoWallPresetNoVideoWallException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
