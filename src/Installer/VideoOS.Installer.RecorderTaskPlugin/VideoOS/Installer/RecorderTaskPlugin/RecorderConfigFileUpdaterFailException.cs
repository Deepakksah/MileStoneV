using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.RecorderTaskPlugin;

[Serializable]
public class RecorderConfigFileUpdaterFailException : Exception
{
	public RecorderConfigFileUpdaterFailException()
	{
	}

	public RecorderConfigFileUpdaterFailException(string message)
		: base(message)
	{
	}

	public RecorderConfigFileUpdaterFailException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecorderConfigFileUpdaterFailException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
