using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class VideoWallRenameException : VmoException
{
	public VideoWallRenameException()
	{
	}

	public VideoWallRenameException(string message)
		: base(message)
	{
	}

	public VideoWallRenameException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected VideoWallRenameException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
