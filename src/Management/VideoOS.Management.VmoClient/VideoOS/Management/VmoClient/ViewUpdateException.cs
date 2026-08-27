using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ViewUpdateException : VmoException
{
	public ViewUpdateException()
	{
	}

	public ViewUpdateException(string message)
		: base(message)
	{
	}

	public ViewUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ViewUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
