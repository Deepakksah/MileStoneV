using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[Serializable]
public abstract class RecorderSubsystemException : VmoServiceException
{
	public RecorderSubsystemException()
	{
	}

	public RecorderSubsystemException(string message)
		: base(message)
	{
	}

	public RecorderSubsystemException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RecorderSubsystemException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
