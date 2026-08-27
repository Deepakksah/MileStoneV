using System;
using System.Threading;

namespace VideoOS.Central.Util;

public static class Waiter
{
	public static void WaitForCondition(Func<bool> exitCondition, TimeSpan maxTimeout, TimeSpan sleepTime)
	{
		DateTime utcNow = DateTime.UtcNow;
		while (!exitCondition())
		{
			Thread.Sleep(sleepTime);
			if (DateTime.UtcNow - utcNow > maxTimeout)
			{
				break;
			}
		}
	}
}
