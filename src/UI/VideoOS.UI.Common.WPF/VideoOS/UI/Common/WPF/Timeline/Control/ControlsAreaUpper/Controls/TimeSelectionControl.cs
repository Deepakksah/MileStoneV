using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaUpper.Controls;

public partial class TimeSelectionControl : UserControl, IComponentConnector
{
	private bool _timeSelectionModeEnabled;

	private bool _userControlEnabled = true;

	public bool UserControlEnabled
	{
		get
		{
			return _userControlEnabled;
		}
		set
		{
			if (_userControlEnabled != value)
			{
				_userControlEnabled = value;
				_timeSelectionModeEnabledToggleButton.IsEnabled = _userControlEnabled;
				_setTimeSelectionTimesButton.IsEnabled = _userControlEnabled;
			}
		}
	}

	public bool TimeSelectionModeEnabled
	{
		get
		{
			return _timeSelectionModeEnabled;
		}
		set
		{
			if (_timeSelectionModeEnabled != value)
			{
				_timeSelectionModeEnabled = value;
				_timeSelectionModeEnabledToggleButton.IsChecked = _timeSelectionModeEnabled;
				if (this.TimeSelectionModeEnabledChanged != null)
				{
					this.TimeSelectionModeEnabledChanged(this, new EventArgs());
				}
			}
		}
	}

	public event EventHandler<EventArgs> TimeSelectionModeEnabledChanged;

	public event EventHandler<EventArgs> SetTimeSelectionTimesButtonClicked;

	public TimeSelectionControl()
	{
		InitializeComponent();
	}

	private void _timeSelectionModeEnabledCheckBox_Click(object sender, RoutedEventArgs e)
	{
		TimeSelectionModeEnabled = _timeSelectionModeEnabledToggleButton.IsChecked == true;
	}

	private void _setTimeSelectionTimesButton_Click(object sender, RoutedEventArgs e)
	{
		if (this.SetTimeSelectionTimesButtonClicked != null)
		{
			this.SetTimeSelectionTimesButtonClicked(this, new EventArgs());
		}
	}
}
