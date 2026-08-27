using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[Serializable]
public class ActiveDirectoryAccessException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 0;

	public ActiveDirectoryAccessException()
	{
	}

	public ActiveDirectoryAccessException(string message)
		: base(message)
	{
	}

	public ActiveDirectoryAccessException(string message, Exception ex)
		: base(message, ex)
	{
	}

	protected ActiveDirectoryAccessException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
