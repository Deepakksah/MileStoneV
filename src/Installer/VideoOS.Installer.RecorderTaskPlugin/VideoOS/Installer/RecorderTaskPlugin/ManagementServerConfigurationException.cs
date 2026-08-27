using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.RecorderTaskPlugin;

[Serializable]
public class ManagementServerConfigurationException : Exception
{
	public ManagementServerConfigurationException()
	{
	}

	public ManagementServerConfigurationException(string message)
		: base(message)
	{
	}

	public ManagementServerConfigurationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ManagementServerConfigurationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
