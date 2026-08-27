using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Timeline.Control.UIControls.SelectTimePopup;

public partial class SelectTimeUserControl : UserControl, IComponentConnector
{
	private TimeZoneInfo _timeZone = TimeZoneInfo.Local;

	public string Header
	{
		get
		{
			return _headerTextBlock.Text;
		}
		set
		{
			_headerTextBlock.Text = value;
		}
	}

	public TimeZoneInfo TimeZone
	{
		get
		{
			return _timeZone;
		}
		set
		{
			_timeZone = value;
		}
	}

	public DateTime Time
	{
		get
		{
			DateTime dateTime = new DateTime(_calendar.SelectedDate.Value.Year, _calendar.SelectedDate.Value.Month, _calendar.SelectedDate.Value.Day);
			dateTime = dateTime.Add(_timePickerTime.Time);
			try
			{
				dateTime = TimeZoneInfo.ConvertTime(dateTime, _timeZone, TimeZoneInfo.Utc);
			}
			catch (ArgumentException)
			{
				dateTime = TimeZoneInfo.ConvertTime(dateTime.AddHours(1.0), _timeZone, TimeZoneInfo.Utc);
			}
			return dateTime;
		}
		set
		{
			DateTime dateTime = TimeZoneInfo.ConvertTime(value, TimeZoneInfo.Utc, _timeZone);
			_calendar.SelectedDate = dateTime;
			_calendar.DisplayDate = dateTime;
			_timePickerTime.Time = new TimeSpan(0, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
		}
	}

	public SelectTimeUserControl()
	{
		InitializeComponent();
	}

	public void FocusOnHours()
	{
		_timePickerTime.FocusOnHours();
	}

	private void _calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
	{
		if (!_calendar.SelectedDate.HasValue && e.RemovedItems.Count > 0)
		{
			_calendar.SelectedDate = (DateTime)e.RemovedItems[0];
		}
		CaptureMouse();
		ReleaseMouseCapture();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
