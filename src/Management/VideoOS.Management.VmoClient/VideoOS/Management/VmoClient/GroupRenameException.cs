using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class GroupRenameException : VmoException
{
	public GroupRenameException()
	{
	}

	public GroupRenameException(string message)
		: base(message)
	{
	}

	public GroupRenameException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected GroupRenameException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
