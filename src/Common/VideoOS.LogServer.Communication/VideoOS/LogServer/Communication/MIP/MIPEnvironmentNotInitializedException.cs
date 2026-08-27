using System;

namespace VideoOS.LogServer.Communication.MIP;

[Serializable]
public class MIPEnvironmentNotInitializedException : Exception
{
	public MIPEnvironmentNotInitializedException()
	{
	}

	public MIPEnvironmentNotInitializedException(string message)
		: base(message)
	{
	}

	public MIPEnvironmentNotInitializedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
