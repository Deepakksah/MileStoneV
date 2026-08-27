using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ViewNotFoundException : VmoException
{
	public ViewNotFoundException()
	{
	}

	public ViewNotFoundException(string message)
		: base(message)
	{
	}

	public ViewNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ViewNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
