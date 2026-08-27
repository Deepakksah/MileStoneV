using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class VmoInsufficientPermissionsException : VmoException
{
	public VmoInsufficientPermissionsException()
	{
	}

	public VmoInsufficientPermissionsException(string message)
		: base(message)
	{
	}

	public VmoInsufficientPermissionsException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected VmoInsufficientPermissionsException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
