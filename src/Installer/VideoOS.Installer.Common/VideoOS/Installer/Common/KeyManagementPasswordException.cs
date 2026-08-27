using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.Common;

[Serializable]
public class KeyManagementPasswordException : Exception
{
	public KeyManagementPasswordException()
	{
	}

	public KeyManagementPasswordException(string message)
		: base(message)
	{
	}

	public KeyManagementPasswordException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected KeyManagementPasswordException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
