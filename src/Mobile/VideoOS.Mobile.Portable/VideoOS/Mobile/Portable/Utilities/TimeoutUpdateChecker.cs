using System;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.Utilities;

public class TimeoutUpdateChecker
{
	public TimeSpan Timeout { get; private set; }

	public DateTime LastUpdateUtc { get; private set; }

	public TimeoutUpdateChecker(TimeSpan timeout)
	{
		Timeout = timeout;
		LastUpdateUtc = DateTime.MinValue;
	}

	public void Update(Action action)
	{
		DateTime utcNow = DateTime.UtcNow;
		if (utcNow - LastUpdateUtc >= Timeout)
		{
			LastUpdateUtc = utcNow;
			try
			{
				action();
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, "TimeoutUpdateChecker.Update()", ex.Message);
			}
		}
	}

	public void Update(Func<bool> function)
	{
		DateTime utcNow = DateTime.UtcNow;
		if (!(utcNow - LastUpdateUtc >= Timeout))
		{
			return;
		}
		try
		{
			if (function())
			{
				LastUpdateUtc = utcNow;
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "TimeoutUpdateChecker.Update()", ex.Message);
		}
	}
}
