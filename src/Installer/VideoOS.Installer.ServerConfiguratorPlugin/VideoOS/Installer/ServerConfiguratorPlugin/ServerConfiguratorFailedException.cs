using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.ServerConfiguratorPlugin;

[Serializable]
public class ServerConfiguratorFailedException : Exception
{
	public ServerConfiguratorFailedException()
	{
	}

	public ServerConfiguratorFailedException(string message)
		: base(message)
	{
	}

	public ServerConfiguratorFailedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ServerConfiguratorFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
