using System;
using System.Windows.Threading;

namespace VideoOS.Utilities.Threading;

public static class TimedAction
{
	public static void ExecuteWithDelay(Action action, TimeSpan delay)
	{
		DispatcherTimer dispatcherTimer = new DispatcherTimer();
		dispatcherTimer.Interval = delay;
		dispatcherTimer.Tag = action;
		dispatcherTimer.Tick += timer_Tick;
		dispatcherTimer.Start();
	}

	private static void timer_Tick(object sender, EventArgs e)
	{
		DispatcherTimer obj = (DispatcherTimer)sender;
		obj.Stop();
		obj.Tick -= timer_Tick;
		((Action)obj.Tag)();
	}
}
