using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ViewCreationException : VmoException
{
	public ViewCreationException()
	{
	}

	public ViewCreationException(string message)
		: base(message)
	{
	}

	public ViewCreationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ViewCreationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
