using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class IdpOfflineException : VmoException
{
	public IdpOfflineException()
	{
	}

	public IdpOfflineException(string message)
		: base(message)
	{
	}

	public IdpOfflineException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected IdpOfflineException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
