using System;

namespace VideoOS.Common.SharedComponent;

[Serializable]
public class SharedComponentException : Exception
{
	public SharedComponentException(string message)
		: base(message)
	{
	}

	public SharedComponentException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
