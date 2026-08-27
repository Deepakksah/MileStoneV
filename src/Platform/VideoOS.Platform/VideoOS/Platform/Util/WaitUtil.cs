using System;
using System.Threading;

namespace VideoOS.Platform.Util;

public class WaitUtil : IWaitUtil
{
	public void Wait(Func<bool> finishCondition, TimeSpan interval, TimeSpan timeout, string timeoutMessage)
	{
		DateTime utcNow = DateTime.UtcNow;
		while (!finishCondition())
		{
			Thread.Sleep(interval);
			if (timeout > TimeSpan.Zero && DateTime.UtcNow - utcNow >= timeout)
			{
				throw new TimeoutException(string.IsNullOrEmpty(timeoutMessage) ? "Timeout reached while waiting" : timeoutMessage);
			}
		}
	}
}
