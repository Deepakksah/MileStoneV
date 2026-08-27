using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;

namespace VideoOS.Server;

[Serializable]
public abstract class SecuritySubsystemException : VmoServiceException
{
	protected SecuritySubsystemException()
	{
	}

	protected SecuritySubsystemException(string message)
		: base(message)
	{
	}

	protected SecuritySubsystemException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SecuritySubsystemException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
