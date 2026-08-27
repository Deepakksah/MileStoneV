using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class MaximumFileSizeApplicationException : CorporateApplicationException
{
	public MaximumFileSizeApplicationException()
	{
	}

	public MaximumFileSizeApplicationException(string message)
		: base(message)
	{
	}

	public MaximumFileSizeApplicationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected MaximumFileSizeApplicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
