using System;
using System.ComponentModel;

namespace VideoOS.Platform.Client;

public class Model : INotifyPropertyChanged
{
	private int _timeSpanInMinutes = 60;

	public int TimeSpanInMinutes
	{
		get
		{
			return _timeSpanInMinutes;
		}
		set
		{
			_timeSpanInMinutes = (int)GetNearbyTimeSpanInMinutes(value);
			OnPropertyChanged("TimeSpanInMinutes");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	private static double GetNearbyTimeSpanInMinutes(int value)
	{
		TimeSpan timeSpan = new TimeSpan(0, value, 0);
		if (timeSpan.TotalDays >= 7.0)
		{
			double num = timeSpan.TotalDays / 7.0;
			timeSpan = ((num >= 3.0) ? new TimeSpan(28, 0, 0, 0) : ((!(num >= 1.5)) ? new TimeSpan(7, 0, 0, 0) : new TimeSpan(14, 0, 0, 0)));
		}
		else if (timeSpan.TotalDays >= 1.0)
		{
			double totalDays = timeSpan.TotalDays;
			timeSpan = ((totalDays >= 3.5) ? new TimeSpan(4, 0, 0, 0) : ((!(totalDays >= 1.5)) ? new TimeSpan(1, 0, 0, 0) : new TimeSpan(2, 0, 0, 0)));
		}
		else if (timeSpan.TotalHours >= 1.0)
		{
			double totalHours = timeSpan.TotalHours;
			timeSpan = ((totalHours >= 18.0) ? new TimeSpan(0, 20, 0, 0) : ((totalHours >= 14.0) ? new TimeSpan(0, 16, 0, 0) : ((totalHours >= 10.0) ? new TimeSpan(0, 12, 0, 0) : ((totalHours >= 6.0) ? new TimeSpan(0, 8, 0, 0) : ((totalHours >= 3.0) ? new TimeSpan(0, 4, 0, 0) : ((!(totalHours >= 1.5)) ? new TimeSpan(0, 1, 0, 0) : new TimeSpan(0, 2, 0, 0)))))));
		}
		else if (timeSpan.TotalMinutes > 0.0)
		{
			double totalMinutes = timeSpan.TotalMinutes;
			timeSpan = ((totalMinutes >= 15.0) ? new TimeSpan(0, 0, 20, 0) : ((totalMinutes >= 7.5) ? new TimeSpan(0, 0, 10, 0) : ((totalMinutes >= 2.5) ? new TimeSpan(0, 0, 5, 0) : ((!(totalMinutes >= 1.5)) ? new TimeSpan(0, 0, 1, 0) : new TimeSpan(0, 0, 2, 0)))));
		}
		return timeSpan.TotalMinutes;
	}

	protected virtual void OnPropertyChanged(string propertyName)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
