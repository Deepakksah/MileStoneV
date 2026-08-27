using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public abstract class AdministrationSubsystemException : Exception
{
	protected AdministrationSubsystemException()
	{
	}

	protected AdministrationSubsystemException(string message)
		: base(message)
	{
	}

	protected AdministrationSubsystemException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected AdministrationSubsystemException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
