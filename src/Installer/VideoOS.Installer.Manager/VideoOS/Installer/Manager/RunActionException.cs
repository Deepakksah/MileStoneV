using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.Manager;

[Serializable]
public class RunActionException : Exception
{
	public RunActionException()
	{
	}

	public RunActionException(string message)
		: base(message)
	{
	}

	public RunActionException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RunActionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
