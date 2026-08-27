using System;

namespace VideoOS.Mobile.Utilities;

public interface IClock
{
	DateTime StartTime { get; set; }

	ClockDirection ClockDirection { get; set; }

	int TicksPerSecond { get; set; }

	bool ClockIsTicking { get; }

	event Tick Tick;

	void Start();

	void Stop();

	void RestartFromTime(DateTime startTime, ClockDirection direction, int ticksPerSecond);

	void JumpToTime(DateTime time);
}
