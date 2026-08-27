using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class CustomPropertyNotFoundException : VmoException
{
	public CustomPropertyNotFoundException()
	{
	}

	public CustomPropertyNotFoundException(string message)
		: base(message)
	{
	}

	public CustomPropertyNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected CustomPropertyNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
