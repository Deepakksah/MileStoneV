using System.Timers;

namespace VideoOS.GatewayService.Utilities;

public interface ITimerWrapper
{
	double Interval { get; set; }

	event ElapsedEventHandler Elapsed;

	void Start();

	void Stop();
}
