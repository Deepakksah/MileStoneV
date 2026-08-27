using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ServerGroupDeleteHasMembersException : VmoException
{
	public ServerGroupDeleteHasMembersException()
	{
	}

	public ServerGroupDeleteHasMembersException(string message)
		: base(message)
	{
	}

	public ServerGroupDeleteHasMembersException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ServerGroupDeleteHasMembersException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
