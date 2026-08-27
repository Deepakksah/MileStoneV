using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ViewItemCreationException : VmoException
{
	public ViewItemCreationException()
	{
	}

	public ViewItemCreationException(string message)
		: base(message)
	{
	}

	public ViewItemCreationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ViewItemCreationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
