using System;
using System.Threading;

namespace VideoOS.Mobile.Portable.Utilities;

public class Waiter
{
	public enum Result
	{
		ConditionMet,
		TimeoutExpired
	}

	public static Result WaitForCondition(Func<bool> condition, TimeSpan timeout)
	{
		return WaitForCondition(condition, timeout, TimeSpan.FromMilliseconds(10.0));
	}

	public static Result WaitForCondition(Func<bool> condition, TimeSpan timeout, TimeSpan cycleTimeout)
	{
		EventWaitHandle eventWaitHandle = new ManualResetEvent(initialState: false);
		DateTime utcNow = DateTime.UtcNow;
		try
		{
			while (true)
			{
				if (condition())
				{
					return Result.ConditionMet;
				}
				if (DateTime.UtcNow - utcNow > timeout)
				{
					break;
				}
				eventWaitHandle.WaitOne(cycleTimeout);
			}
			return Result.TimeoutExpired;
		}
		finally
		{
			eventWaitHandle.Dispose();
		}
	}
}
