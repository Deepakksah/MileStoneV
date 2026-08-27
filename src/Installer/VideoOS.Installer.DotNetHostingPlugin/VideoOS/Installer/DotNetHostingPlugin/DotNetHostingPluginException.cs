using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.DotNetHostingPlugin;

[Serializable]
public class DotNetHostingPluginException : Exception
{
	public DotNetHostingPluginException()
	{
	}

	public DotNetHostingPluginException(string message)
		: base(message)
	{
	}

	public DotNetHostingPluginException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected DotNetHostingPluginException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
