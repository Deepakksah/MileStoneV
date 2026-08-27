using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class HandlerNameNotFoundException : CorporateApplicationException
{
	public HandlerNameNotFoundException()
	{
	}

	public HandlerNameNotFoundException(string message)
		: base(message)
	{
	}

	public HandlerNameNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HandlerNameNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
