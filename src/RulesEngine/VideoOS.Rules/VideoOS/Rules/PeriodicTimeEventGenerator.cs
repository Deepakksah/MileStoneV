using System;
using System.Threading;
using Microsoft.Win32;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public class PeriodicTimeEventGenerator : IDisposable
{
	public delegate void PeriodicTimerEventHandler(object sender, PeriodicTimerEventArgs e);

	private AutoResetEvent waitObject = new AutoResetEvent(initialState: false);

	private RegisteredWaitHandle waitHandle;

	private object waitHandleSync = new object();

	private PeriodicTimeGranularity granularity;

	private int interval;

	private DateTime nextEventTime;

	public event PeriodicTimerEventHandler Timer;

	public PeriodicTimeEventGenerator(int interval, PeriodicTimeGranularity granularity)
	{
		if (interval != 1 && interval != 2 && interval != 3 && interval != 4 && interval != 5 && interval != 6 && interval != 10 && interval != 12 && interval != 15 && interval != 20 && interval != 30 && interval != 60)
		{
			throw new ArgumentException("The interval passed to the PeriodicTimeEventGenerator constructor must be a number by which 60 is divisible");
		}
		this.granularity = granularity;
		this.interval = interval;
		SystemEvents.TimeChanged += SystemEvents_TimeChanged;
	}

	protected virtual void OnTimer(PeriodicTimerEventArgs e)
	{
		if (this.Timer != null)
		{
			this.Timer(this, e);
		}
	}

	public void Start()
	{
		Stop();
		StartTimer(first: true);
	}

	public void Stop()
	{
		lock (waitHandleSync)
		{
			if (waitHandle != null)
			{
				waitHandle.Unregister(null);
				waitHandle = null;
				SystemEvents.TimeChanged -= SystemEvents_TimeChanged;
			}
		}
	}

	private void StartTimer(bool first)
	{
		int millisecondsTimeOutInterval = CalculateNextWaitIntervalValues();
		lock (waitHandleSync)
		{
			waitHandle = ThreadPool.RegisterWaitForSingleObject(waitObject, WaitCallBack, null, millisecondsTimeOutInterval, executeOnlyOnce: true);
		}
	}

	private int CalculateNextWaitIntervalValues()
	{
		int num;
		DateTime utcNow;
		int num2;
		switch (granularity)
		{
		case PeriodicTimeGranularity.Seconds:
			num = interval;
			utcNow = DateTime.UtcNow;
			num2 = utcNow.Second;
			break;
		case PeriodicTimeGranularity.Minutes:
			num = interval * 60;
			utcNow = DateTime.UtcNow;
			num2 = utcNow.Minute * 60 + utcNow.Second;
			break;
		default:
			throw new TimeProfileApplicationException("Unexpected granularity specifier for PeriodicTimeEventGenerator");
		}
		int num3 = (num2 / num * num + num - num2) * 1000;
		nextEventTime = utcNow.AddMilliseconds(num3 - utcNow.Millisecond);
		return num3 - DateTime.UtcNow.Millisecond + 200;
	}

	public void GenerateEventNow()
	{
		OnTimer(new PeriodicTimerEventArgs(DateTime.UtcNow, TimerReason.ManualRequest));
	}

	private void WaitCallBack(object state, bool timedout)
	{
		try
		{
			DateTime eventTime = nextEventTime;
			StartTimer(first: false);
			OnTimer(new PeriodicTimerEventArgs(eventTime, TimerReason.TimerElapsed));
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}

	private void SystemEvents_TimeChanged(object sender, EventArgs e)
	{
		lock (waitHandleSync)
		{
			if (waitHandle != null)
			{
				OnTimer(new PeriodicTimerEventArgs(DateTime.UtcNow, TimerReason.SystemTimeChanged));
				Start();
			}
		}
	}

	public void Dispose()
	{
		waitObject.Dispose();
	}
}
