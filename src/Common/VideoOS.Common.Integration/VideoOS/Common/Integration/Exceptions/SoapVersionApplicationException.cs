using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class SoapVersionApplicationException : CorporateApplicationException
{
	public SoapVersionApplicationException()
	{
	}

	public SoapVersionApplicationException(string message)
		: base(message)
	{
	}

	public SoapVersionApplicationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SoapVersionApplicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
