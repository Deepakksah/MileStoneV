using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.LogServerWarningPlugin;

[Serializable]
public class OemResourceInvalidException : Exception
{
	public OemResourceInvalidException()
	{
	}

	public OemResourceInvalidException(string message)
		: base(message)
	{
	}

	public OemResourceInvalidException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected OemResourceInvalidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
