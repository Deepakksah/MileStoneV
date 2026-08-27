using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;

namespace VideoOS.Server;

[Serializable]
public abstract class AdministrationSubsystemException : VmoServiceException
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
