using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class IdentityNotFoundException : VmoException
{
	public IdentityNotFoundException()
	{
	}

	public IdentityNotFoundException(string message)
		: base(message)
	{
	}

	public IdentityNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected IdentityNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
