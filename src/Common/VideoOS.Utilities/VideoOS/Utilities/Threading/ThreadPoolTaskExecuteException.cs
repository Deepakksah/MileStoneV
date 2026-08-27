using System;

namespace VideoOS.Utilities.Threading;

public class ThreadPoolTaskExecuteException : Exception
{
	public ThreadPoolTaskExecuteException()
	{
	}

	public ThreadPoolTaskExecuteException(string message)
		: base(message)
	{
	}

	public ThreadPoolTaskExecuteException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
