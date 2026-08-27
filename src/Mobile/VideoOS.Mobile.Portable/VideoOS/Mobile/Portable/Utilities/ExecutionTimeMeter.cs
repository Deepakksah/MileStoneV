using System;

namespace VideoOS.Mobile.Portable.Utilities;

public class ExecutionTimeMeter
{
	private TimeSpan _totalTime;

	private long _totalMeasures;

	private TimeSpan _lastMeasaure;

	private TimeSpan _maxMeasure;

	private TimeSpan _minMeasure;

	private DateTime _starTime;

	public TimeSpan TotalTimeSpan => new TimeSpan(_totalTime.Ticks);

	public long TotalMeasures => _totalMeasures;

	public TimeSpan LastMeasure => new TimeSpan(_lastMeasaure.Ticks);

	public TimeSpan MaxMeasure => new TimeSpan(_maxMeasure.Ticks);

	public TimeSpan MinMeasure => new TimeSpan(_minMeasure.Ticks);

	public TimeSpan AverageMeasure => TimeSpan.FromMilliseconds(_totalTime.TotalMilliseconds / (double)_totalMeasures);

	public ExecutionTimeMeter()
	{
		_totalMeasures = 0L;
		_totalTime = new TimeSpan(0L);
		_lastMeasaure = new TimeSpan(0L);
		_maxMeasure = new TimeSpan(0L);
		_minMeasure = TimeSpan.MaxValue;
	}

	public void StartMeasure()
	{
		_starTime = DateTime.UtcNow;
	}

	public void EndMeasure()
	{
		_lastMeasaure = DateTime.UtcNow - _starTime;
		_totalMeasures++;
		_totalTime = _totalTime.Add(_lastMeasaure);
		if (_minMeasure > _lastMeasaure)
		{
			_minMeasure = _lastMeasaure;
		}
		if (_maxMeasure < _lastMeasaure)
		{
			_maxMeasure = _lastMeasaure;
		}
	}
}
