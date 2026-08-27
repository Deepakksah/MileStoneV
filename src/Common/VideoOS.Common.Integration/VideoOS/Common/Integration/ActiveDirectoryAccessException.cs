using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class ActiveDirectoryAccessException : ServerServiceException
{
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
