using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class MIPObjectNotFoundException : VmoException
{
	public MIPObjectNotFoundException()
	{
	}

	public MIPObjectNotFoundException(string message)
		: base(message)
	{
	}

	public MIPObjectNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected MIPObjectNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
