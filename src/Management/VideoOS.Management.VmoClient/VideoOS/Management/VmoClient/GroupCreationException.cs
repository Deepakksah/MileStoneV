using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class GroupCreationException : VmoException
{
	public GroupCreationException()
	{
	}

	public GroupCreationException(string message)
		: base(message)
	{
	}

	public GroupCreationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected GroupCreationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
