using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class ParserApplicationException : CorporateApplicationException
{
	public ParserApplicationException()
	{
	}

	public ParserApplicationException(string message)
		: base(message)
	{
	}

	public ParserApplicationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ParserApplicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
