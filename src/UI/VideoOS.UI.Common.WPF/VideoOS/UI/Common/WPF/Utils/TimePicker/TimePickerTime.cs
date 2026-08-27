using System;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace VideoOS.UI.Common.WPF.Utils.TimePicker;

public class TimePickerTime : TimePickerBase
{
	private bool Is24Hour => new DateTime(2000, 1, 1, 23, 0, 0).ToString("t", CultureInfo.CurrentCulture).Contains("23");

	public TimeSpan Time
	{
		get
		{
			int num = _hour.Value;
			if (!Is24Hour)
			{
				num %= 12;
				if (_designator.Text == Thread.CurrentThread.CurrentCulture.DateTimeFormat.PMDesignator)
				{
					num += 12;
				}
			}
			return new TimeSpan(num, _minutes.Value, _seconds.Value);
		}
		set
		{
			int num = value.Hours;
			if (!Is24Hour)
			{
				if (value.Hours >= 12)
				{
					_designator.SetSelected(Thread.CurrentThread.CurrentCulture.DateTimeFormat.PMDesignator);
				}
				else
				{
					_designator.SetSelected(Thread.CurrentThread.CurrentCulture.DateTimeFormat.AMDesignator);
				}
				num %= 12;
				if (num == 0)
				{
					num += 12;
				}
			}
			_hour.Value = num;
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
			_hour.MaxValue = value.Hours;
			_minutes.MaxValue = value.Minutes;
			_seconds.MaxValue = value.Seconds;
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
			_minutes.MinValue = value.Minutes;
			_seconds.MinValue = value.Seconds;
			InvalidateTimePicker();
		}
	}

	public TimePickerTime()
	{
		_designator.SetSelected(Thread.CurrentThread.CurrentCulture.DateTimeFormat.AMDesignator);
		if (Is24Hour)
		{
			MaxTime = new TimeSpan(23, 59, 59);
			MinTime = new TimeSpan(0, 0, 0);
			_designator.Visibility = Visibility.Collapsed;
		}
		else
		{
			MaxTime = new TimeSpan(12, 59, 59);
			MinTime = new TimeSpan(1, 0, 0);
			_designator.Visibility = Visibility.Visible;
		}
	}

	public void FocusOnHours()
	{
		if (!base.IsLoaded)
		{
			base.Loaded += TimePickerTime_Loaded;
		}
		else
		{
			_hour.Focus();
		}
	}

	private void TimePickerTime_Loaded(object sender, RoutedEventArgs e)
	{
		_hour.Focus();
	}

	protected override void DoFocus()
	{
		_textboxInFocus = _hour;
	}
}
