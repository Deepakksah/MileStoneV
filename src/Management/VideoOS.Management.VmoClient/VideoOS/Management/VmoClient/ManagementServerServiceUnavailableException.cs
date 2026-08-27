using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ManagementServerServiceUnavailableException : VmoException
{
	public ManagementServerServiceUnavailableException()
	{
	}

	public ManagementServerServiceUnavailableException(string address)
		: base(address)
	{
	}

	public ManagementServerServiceUnavailableException(string address, Exception innerException)
		: base(address, innerException)
	{
	}

	protected ManagementServerServiceUnavailableException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
