using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ViewItemUpdateException : VmoException
{
	public ViewItemUpdateException()
	{
	}

	public ViewItemUpdateException(string message)
		: base(message)
	{
	}

	public ViewItemUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ViewItemUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
