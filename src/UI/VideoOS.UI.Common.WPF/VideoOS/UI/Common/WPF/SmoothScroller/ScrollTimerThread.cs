using System;
using System.Threading;
using System.Windows.Threading;

namespace VideoOS.UI.Common.WPF.SmoothScroller;

public class ScrollTimerThread
{
	private int _interval;

	private Delegate _tickCallbackDelegate;

	private Dispatcher _dispatcher;

	private Thread _thread;

	private volatile bool _stopThread;

	public ScrollTimerThread(int interval, Delegate tickCallbackDelegate, Dispatcher dispatcher)
	{
		_interval = interval;
		_tickCallbackDelegate = tickCallbackDelegate;
		_dispatcher = dispatcher;
		_stopThread = false;
		_thread = new Thread(ThreadFunction);
		_thread.Name = "SmoothScroller - ScrollTimerThread";
		_thread.IsBackground = true;
		_thread.Start();
	}

	public void Stop()
	{
		_stopThread = true;
	}

	private void ThreadFunction()
	{
		while (!_stopThread)
		{
			_dispatcher.BeginInvoke(DispatcherPriority.Input, _tickCallbackDelegate);
			Thread.Sleep(_interval);
		}
	}
}
