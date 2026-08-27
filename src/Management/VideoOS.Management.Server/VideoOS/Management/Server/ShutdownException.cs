using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[Serializable]
public class ShutdownException : VmoServiceException
{
	protected override int ErrorCodeDefault => -1;

	public ShutdownException()
	{
	}

	public ShutdownException(string message, int id)
		: base(message, id)
	{
	}

	public ShutdownException(string message, int id, Exception innerException)
		: base(message, id, innerException)
	{
	}
}
