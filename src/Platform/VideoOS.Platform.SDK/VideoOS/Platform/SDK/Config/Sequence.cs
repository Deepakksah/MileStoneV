using System;

namespace VideoOS.Platform.SDK.Config;

public class Sequence
{
	private DateTime _timeBegin;

	private DateTime _timeTrigged;

	private DateTime _timeEnd;

	public DateTime BeginTime
	{
		get
		{
			return _timeBegin;
		}
		set
		{
			_timeBegin = value;
		}
	}

	public DateTime TriggerTime => _timeTrigged;

	public DateTime EndTime
	{
		get
		{
			return _timeEnd;
		}
		set
		{
			_timeEnd = value;
		}
	}

	public Sequence()
	{
	}

	public Sequence(DateTime begin, DateTime trigger, DateTime end)
	{
		_timeBegin = begin;
		_timeTrigged = trigger;
		_timeEnd = end;
	}
}
