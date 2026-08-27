using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnsecureConnectionNotAllowedException : VmoException
{
	public UnsecureConnectionNotAllowedException()
	{
	}

	public UnsecureConnectionNotAllowedException(string message)
		: base(message)
	{
	}

	public UnsecureConnectionNotAllowedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected UnsecureConnectionNotAllowedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
