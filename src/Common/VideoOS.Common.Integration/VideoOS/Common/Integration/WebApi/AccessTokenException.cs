using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.WebApi;

[Serializable]
public class AccessTokenException : Exception
{
	public AccessTokenException()
	{
	}

	public AccessTokenException(string message)
		: base(message)
	{
	}

	public AccessTokenException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected AccessTokenException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
