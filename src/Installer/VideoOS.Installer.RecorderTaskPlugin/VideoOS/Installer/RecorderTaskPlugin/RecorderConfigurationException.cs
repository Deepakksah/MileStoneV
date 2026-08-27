using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.RecorderTaskPlugin;

[Serializable]
public class RecorderConfigurationException : Exception
{
	public RecorderConfigurationException()
	{
	}

	public RecorderConfigurationException(string message)
		: base(message)
	{
	}

	public RecorderConfigurationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RecorderConfigurationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
