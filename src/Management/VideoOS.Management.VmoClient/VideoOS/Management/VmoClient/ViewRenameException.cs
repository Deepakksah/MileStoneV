using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ViewRenameException : VmoException
{
	public ViewRenameException()
	{
	}

	public ViewRenameException(string message)
		: base(message)
	{
	}

	public ViewRenameException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ViewRenameException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
