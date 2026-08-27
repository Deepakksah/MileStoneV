using System;

namespace VideoOS.Platform.Util;

public interface IWaitUtil
{
	void Wait(Func<bool> finishCondition, TimeSpan interval, TimeSpan timeout, string timeoutMessage);
}
