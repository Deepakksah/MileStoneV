using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.ServerConfiguratorPlugin;

[Serializable]
public class ServerConfigFileUpdaterFailedException : Exception
{
	public ServerConfigFileUpdaterFailedException()
	{
	}

	public ServerConfigFileUpdaterFailedException(string message)
		: base(message)
	{
	}

	public ServerConfigFileUpdaterFailedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ServerConfigFileUpdaterFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
