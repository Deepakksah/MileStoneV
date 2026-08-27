using System;
using System.Windows;

namespace VideoOS.UI.Common.WPF.Utils.TimePicker;

public class TimePickerSpan : TimePickerBase
{
	private TimeSpan _maxTime = new TimeSpan(24, 0, 0);

	private TimeSpan _minTime = new TimeSpan(0, 0, 0);

	private bool _isValidating;

	public TimeSpan Time
	{
		get
		{
			return new TimeSpan(_hour.Value, _minutes.Value, _seconds.Value);
		}
		set
		{
			int value2 = value.Hours + 24 * value.Days;
			_hour.Value = value2;
			_minutes.Value = value.Minutes;
			_seconds.Value = value.Seconds;
			InvalidateTimePicker();
		}
	}

	public TimeSpan MaxTime
	{
		get
		{
			return new TimeSpan(_hour.MaxValue, _minutes.MaxValue, _seconds.MaxValue);
		}
		set
		{
			_hour.MaxValue = value.Hours + 24 * value.Days;
			_minutes.MaxValue = 59;
			_seconds.MaxValue = 59;
			_maxTime = value;
			InvalidateTimePicker();
		}
	}

	public TimeSpan MinTime
	{
		get
		{
			return new TimeSpan(_hour.MinValue, _minutes.MinValue, _seconds.MinValue);
		}
		set
		{
			_hour.MinValue = value.Hours;
			_minutes.MinValue = 0;
			_seconds.MinValue = 0;
			_minTime = value;
			InvalidateTimePicker();
		}
	}

	public TimePickerSpan()
	{
		_designator.Visibility = Visibility.Collapsed;
		_hour.OnValidated += TimeValidated;
		_minutes.OnValidated += TimeValidated;
		_seconds.OnValidated += TimeValidated;
		_designator.OnValidated += TimeValidated;
	}

	protected override void DoFocus()
	{
		_textboxInFocus = _seconds;
	}

	private void TimeValidated(object sender, EventArgs e)
	{
		if (!_isValidating)
		{
			_isValidating = true;
			TimeSpan time = Time;
			if (_hour.Value == _minTime.Hours && _minutes.Value == _minTime.Minutes)
			{
				_seconds.MinValue = _minTime.Seconds;
			}
			else
			{
				_seconds.MinValue = 0;
			}
			if (time > _maxTime)
			{
				Time = _maxTime;
			}
			_isValidating = false;
		}
	}
}
