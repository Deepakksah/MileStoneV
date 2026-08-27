using System;
using System.ComponentModel;
using System.Globalization;

namespace VideoOS.UI.Common.WPF.DisableAlarm;

public class ObservableDisableItem : INotifyPropertyChanged
{
	private DisableAlarmListItem _disableAlarmItem;

	private Guid _updateId;

	public string Source => _disableAlarmItem.SourceName;

	public string Events
	{
		get
		{
			string text = "";
			bool flag = true;
			foreach (string preselectedEventType in _disableAlarmItem.PreselectedEventTypes)
			{
				if (flag)
				{
					text = preselectedEventType;
					flag = false;
				}
				else
				{
					text = text + ", " + preselectedEventType;
				}
			}
			return text;
		}
	}

	public string Time
	{
		get
		{
			TimeSpan timeSpan = _disableAlarmItem.Time - DateTime.UtcNow;
			if (timeSpan.TotalDays >= 1.0)
			{
				return timeSpan.ToString("d\\.hh\\:mm\\:ss", CultureInfo.CurrentCulture);
			}
			return timeSpan.ToString("hh\\:mm\\:ss", CultureInfo.CurrentCulture);
		}
	}

	public DisableAlarmListItem DisableAlarmItem
	{
		get
		{
			return _disableAlarmItem;
		}
		set
		{
			_disableAlarmItem = value;
			NotifyPropertyChanged("Source");
			NotifyPropertyChanged("Time");
			NotifyPropertyChanged("Events");
		}
	}

	public Guid UpdateId
	{
		get
		{
			return _updateId;
		}
		set
		{
			_updateId = value;
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public ObservableDisableItem(DisableAlarmListItem disableAlarmItem)
	{
		_disableAlarmItem = disableAlarmItem;
	}

	protected void NotifyPropertyChanged(string propertyName)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
