using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.RecorderPlugins;

[Serializable]
public class RecorderInstallerException : Exception
{
	public RecorderInstallerException()
	{
	}

	public RecorderInstallerException(string message)
		: base(message)
	{
	}

	public RecorderInstallerException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecorderInstallerException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
