using System;
using System.Threading;
using System.Windows.Threading;

namespace VideoOS.UI.Common.WPF.Tween;

public class TweenThread
{
	private int _interval;

	private Delegate _tickCallbackDelegate;

	private Dispatcher _dispatcher;

	private volatile bool _stopThread;

	public TweenThread(int interval, Delegate tickCallbackDelegate, Dispatcher dispatcher)
	{
		_interval = interval;
		_tickCallbackDelegate = tickCallbackDelegate;
		_dispatcher = dispatcher;
		_stopThread = false;
		ThreadPool.QueueUserWorkItem(ThreadFunction);
	}

	public void Stop()
	{
		_stopThread = true;
	}

	private void ThreadFunction(object state)
	{
		while (!_stopThread)
		{
			_dispatcher.BeginInvoke(DispatcherPriority.Loaded, _tickCallbackDelegate);
			Thread.Sleep(_interval);
		}
	}
}
