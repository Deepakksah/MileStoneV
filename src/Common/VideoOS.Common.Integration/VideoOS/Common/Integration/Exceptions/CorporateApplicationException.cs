using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class CorporateApplicationException : Exception
{
	public CorporateApplicationException()
	{
	}

	public CorporateApplicationException(string message)
		: base(message)
	{
	}

	public CorporateApplicationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected CorporateApplicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
