using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class BasicUserRenameException : VmoException
{
	public BasicUserRenameException()
	{
	}

	public BasicUserRenameException(string message)
		: base(message)
	{
	}

	public BasicUserRenameException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected BasicUserRenameException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
