using System.ComponentModel;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.Selection;

public class SelectionInfoModel : INotifyPropertyChanged
{
	private string _startTime;

	private string _endTime;

	private string _duration;

	public string StartTime
	{
		get
		{
			return _startTime;
		}
		set
		{
			if (_startTime != value)
			{
				_startTime = value;
				FirePropChanged("StartTime");
			}
		}
	}

	public string EndTime
	{
		get
		{
			return _endTime;
		}
		set
		{
			if (_endTime != value)
			{
				_endTime = value;
				FirePropChanged("EndTime");
			}
		}
	}

	public string Duration
	{
		get
		{
			return _duration;
		}
		set
		{
			if (_duration != value)
			{
				_duration = value;
				FirePropChanged("Duration");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	private void FirePropChanged(string propName)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}
	}
}
