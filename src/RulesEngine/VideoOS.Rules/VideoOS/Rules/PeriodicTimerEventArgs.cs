using System;

namespace VideoOS.Rules;

public class PeriodicTimerEventArgs : EventArgs
{
	private DateTime eventTime;

	private TimerReason timerReason;

	public DateTime EventTime => eventTime;

	public TimerReason TimerReason => timerReason;

	public PeriodicTimerEventArgs(DateTime eventTime, TimerReason timerReason)
	{
		this.eventTime = eventTime;
		this.timerReason = timerReason;
	}
}
