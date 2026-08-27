using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class MIPActionNotFoundException : VmoException
{
	public MIPActionNotFoundException()
	{
	}

	public MIPActionNotFoundException(string message)
		: base(message)
	{
	}

	public MIPActionNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected MIPActionNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
