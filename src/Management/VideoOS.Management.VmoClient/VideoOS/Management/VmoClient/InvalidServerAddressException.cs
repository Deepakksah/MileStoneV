using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class InvalidServerAddressException : VmoException
{
	public InvalidServerAddressException()
	{
	}

	public InvalidServerAddressException(string address)
		: base(address)
	{
	}

	public InvalidServerAddressException(string address, Exception innerException)
		: base(address, innerException)
	{
	}

	protected InvalidServerAddressException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
