using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class ExceptionDialogApplicationException : CorporateApplicationException
{
	public ExceptionDialogApplicationException()
	{
	}

	public ExceptionDialogApplicationException(string message)
		: base(message)
	{
	}

	public ExceptionDialogApplicationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ExceptionDialogApplicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
