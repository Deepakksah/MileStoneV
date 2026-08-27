using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Timeline.Control.UIControls.SelectTimePopup;

public partial class TimelineSelectTimeUserControl : UserControl, IComponentConnector
{
	public TimeZoneInfo TimeZone
	{
		get
		{
			return _selectTimeUserControl1.TimeZone;
		}
		set
		{
			_selectTimeUserControl1.TimeZone = value;
			_selectTimeUserControl2.TimeZone = value;
			_selectTimeUserControl3.TimeZone = value;
		}
	}

	public Visibility SelectTime1Visibility
	{
		get
		{
			return _selectTimeUserControl1.Visibility;
		}
		set
		{
			_selectTimeUserControl1.Visibility = value;
		}
	}

	public Visibility SelectTime2Visibility
	{
		get
		{
			return _selectTimeUserControl2.Visibility;
		}
		set
		{
			_selectTimeUserControl2.Visibility = value;
		}
	}

	public Visibility SelectTime3Visibility
	{
		get
		{
			return _selectTimeUserControl3.Visibility;
		}
		set
		{
			_selectTimeUserControl3.Visibility = value;
		}
	}

	public string OKButtonLabel
	{
		get
		{
			return _okButton.Content as string;
		}
		set
		{
			_okButton.Content = value;
		}
	}

	public string SelectTime1Header
	{
		get
		{
			return _selectTimeUserControl1.Header;
		}
		set
		{
			_selectTimeUserControl1.Header = value;
		}
	}

	public string SelectTime2Header
	{
		get
		{
			return _selectTimeUserControl2.Header;
		}
		set
		{
			_selectTimeUserControl2.Header = value;
		}
	}

	public string SelectTime3Header
	{
		get
		{
			return _selectTimeUserControl3.Header;
		}
		set
		{
			_selectTimeUserControl3.Header = value;
		}
	}

	public DateTime SelectTime1Time
	{
		get
		{
			return _selectTimeUserControl1.Time;
		}
		set
		{
			_selectTimeUserControl1.Time = value;
		}
	}

	public DateTime SelectTime2Time
	{
		get
		{
			return _selectTimeUserControl2.Time;
		}
		set
		{
			_selectTimeUserControl2.Time = value;
		}
	}

	public DateTime SelectTime3Time
	{
		get
		{
			return _selectTimeUserControl3.Time;
		}
		set
		{
			_selectTimeUserControl3.Time = value;
		}
	}

	public event EventHandler CloseButtonClickEvent;

	public event EventHandler OKButtonClickEvent;

	public TimelineSelectTimeUserControl()
	{
		InitializeComponent();
		_selectTimeUserControl1.FocusOnHours();
	}

	private void _closeButton_Click(object sender, EventArgs e)
	{
		if (this.CloseButtonClickEvent != null)
		{
			this.CloseButtonClickEvent(this, new EventArgs());
		}
	}

	private void OKButton_Click(object sender, RoutedEventArgs e)
	{
		if (this.OKButtonClickEvent != null)
		{
			this.OKButtonClickEvent(this, new EventArgs());
		}
	}
}
