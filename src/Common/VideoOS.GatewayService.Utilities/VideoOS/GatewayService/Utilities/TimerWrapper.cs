using System.Timers;

namespace VideoOS.GatewayService.Utilities;

public class TimerWrapper : Timer, ITimerWrapper
{
	void ITimerWrapper.Start()
	{
		Start();
	}

	void ITimerWrapper.Stop()
	{
		Stop();
	}
}
