using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Mobile.Portable.Utilities;

public class FpsMeter : IFpsMeter, IFpsMeterMeasure, IFramesCounter
{
	private readonly TimeSpan _timeSpan;

	private readonly Queue<DateTime> _dateTimes;

	private DateTime _startTimeUtc;

	private readonly object _dequeueLock = new object();

	public int Count
	{
		get
		{
			lock (_dequeueLock)
			{
				DequeueToCurrTime();
				return _dateTimes.Count;
			}
		}
	}

	public double FPS
	{
		get
		{
			lock (_dequeueLock)
			{
				TimeSpan timeSpan = DequeueToCurrTime() - _startTimeUtc;
				if (timeSpan > _timeSpan)
				{
					timeSpan = _timeSpan;
				}
				if (_dateTimes.Count == 0)
				{
					return 0.0;
				}
				return (double)_dateTimes.Count / timeSpan.TotalMilliseconds * 1000.0;
			}
		}
	}

	public double Fps => FPS;

	public FpsMeter(TimeSpan timespan)
	{
		_timeSpan = new TimeSpan(timespan.Ticks);
		_dateTimes = new Queue<DateTime>();
		InitStartTime();
	}

	public void InitStartTime()
	{
		_startTimeUtc = DateTime.UtcNow;
	}

	public void Reset()
	{
		InitStartTime();
	}

	public void OnFrame()
	{
		lock (_dequeueLock)
		{
			DateTime item = DequeueToCurrTime();
			_dateTimes.Enqueue(item);
		}
	}

	private DateTime DequeueToCurrTime()
	{
		DateTime utcNow = DateTime.UtcNow;
		while (_dateTimes.Count > 0 && utcNow - _dateTimes.ElementAt(0) > _timeSpan)
		{
			_dateTimes.Dequeue();
		}
		return utcNow;
	}
}
