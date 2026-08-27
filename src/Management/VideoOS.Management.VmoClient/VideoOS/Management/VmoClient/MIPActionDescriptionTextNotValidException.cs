using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class MIPActionDescriptionTextNotValidException : VmoException
{
	public MIPActionDescriptionTextNotValidException()
	{
	}

	public MIPActionDescriptionTextNotValidException(string message)
		: base(message)
	{
	}

	public MIPActionDescriptionTextNotValidException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected MIPActionDescriptionTextNotValidException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
