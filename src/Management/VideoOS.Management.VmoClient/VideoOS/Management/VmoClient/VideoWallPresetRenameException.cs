using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class VideoWallPresetRenameException : VmoException
{
	public VideoWallPresetRenameException()
	{
	}

	public VideoWallPresetRenameException(string message)
		: base(message)
	{
	}

	public VideoWallPresetRenameException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected VideoWallPresetRenameException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
