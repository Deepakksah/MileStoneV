using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace VideoOS.Central.Util;

public sealed class Debouncer<TEventArgs> : IDisposable
{
	private readonly List<Tuple<DateTime, TEventArgs>> _eventsList = new List<Tuple<DateTime, TEventArgs>>();

	private readonly object _lock = new object();

	private readonly Timer _timer;

	private readonly TimeSpan _delayTimeout;

	public EventHandler<DebouncerEventArgs<TEventArgs>> EventHandler;

	public Debouncer(TimeSpan debounceTimeout, TimeSpan delayTimeout)
	{
		_delayTimeout = delayTimeout;
		_timer = new Timer(debounceTimeout.TotalMilliseconds);
		_timer.Elapsed += OnTimer;
		_timer.AutoReset = true;
		_timer.Start();
	}

	public void Dispose()
	{
		EventHandler = null;
		_timer.Elapsed -= OnTimer;
		_timer.Stop();
		_timer.Dispose();
	}

	private void OnTimer(object sender, EventArgs evArgs)
	{
		DateTime now = DateTime.UtcNow;
		List<Tuple<DateTime, TEventArgs>> list;
		lock (_lock)
		{
			list = _eventsList.Where((Tuple<DateTime, TEventArgs> tuple) => tuple.Item1 < now).ToList();
			if (list.Count != 0)
			{
				_eventsList.RemoveAll((Tuple<DateTime, TEventArgs> tuple) => tuple.Item1 < now);
			}
		}
		if (list.Count != 0)
		{
			DebouncerEventArgs<TEventArgs> e = new DebouncerEventArgs<TEventArgs>
			{
				eventArgs = list.Select((Tuple<DateTime, TEventArgs> tuple) => tuple.Item2)
			};
			EventHandler?.Invoke(this, e);
		}
	}

	public void OnEvent(object sender, TEventArgs evArgs)
	{
		lock (_lock)
		{
			_eventsList.Add(new Tuple<DateTime, TEventArgs>(DateTime.UtcNow + _delayTimeout, evArgs));
		}
	}
}
