using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using VideoOS.UI.Common.WPF.Resources;
using VideoOS.UI.Common.WPF.Utils.RightToLeft;

namespace VideoOS.UI.Common.WPF.DisableAlarm;

public partial class DisableSetupWindow : Window, IComponentConnector
{
	private DisableAlarmListItem _disableAlarmListItem;

	private bool _initializing;

	private bool _isModifying;

	private object _serverId;

	private DisableWindowCallback _callBack;

	private DisableWindowConvertTimeCallback _convertTimeCallBack;

	public DisableSetupWindow(DisableAlarmListItem disableAlarmListItem, object serverId, bool isModifying, DisableWindowCallback callBack, DisableWindowConvertTimeCallback convertTimeCallBack)
	{
		_initializing = true;
		InitializeComponent();
		base.FlowDirection = RtlUtility.SelectedFlowDirection;
		base.Title = string.Format(CultureInfo.CurrentUICulture, Strings.DisableAlarmsTitle);
		_disableAlarmListItem = disableAlarmListItem;
		_isModifying = isModifying;
		_serverId = serverId;
		_callBack = callBack;
		_convertTimeCallBack = convertTimeCallBack;
		List<string> source = ((disableAlarmListItem.PreselectedEventTypes == null) ? new List<string>() : new List<string>(disableAlarmListItem.PreselectedEventTypes));
		foreach (string availableEventType in disableAlarmListItem.AvailableEventTypes)
		{
			CheckBox checkBox = new CheckBox();
			checkBox.Checked += checkBox_Checked;
			checkBox.Unchecked += checkBox_Checked;
			checkBox.Content = availableEventType;
			if (source.Contains(availableEventType, StringComparer.OrdinalIgnoreCase))
			{
				checkBox.IsChecked = true;
			}
			alarmTypeListBox.Items.Add(checkBox);
		}
		DateTime dateTime = _convertTimeCallBack(_disableAlarmListItem.Time);
		DateTime dateTime2 = _convertTimeCallBack(DateTime.UtcNow);
		TimeSpan timeSpan = dateTime - dateTime2;
		TimeSpan timeSpan2 = new TimeSpan(0, 0, 1, 0, 0);
		if (timeSpan < timeSpan2)
		{
			timeSpan = timeSpan2;
		}
		double totalMinutes = timeSpan.TotalMinutes;
		if (totalMinutes < 120.0)
		{
			disableForTextBox.Text = Convert.ToInt32(totalMinutes).ToString();
		}
		else
		{
			double totalHours = timeSpan.TotalHours;
			if (totalHours < 48.0)
			{
				disableForTextBox.Text = Convert.ToInt32(totalHours).ToString();
				disableForComboBox.SelectedIndex = 1;
			}
			else
			{
				double totalDays = timeSpan.TotalDays;
				disableForTextBox.Text = Convert.ToInt32(totalDays).ToString();
				disableForComboBox.SelectedIndex = 2;
			}
		}
		disableForRadioButton.IsChecked = true;
		disableUntilDatePicker.SelectedDate = dateTime;
		disableUntilDatePicker.SelectedDateChanged += disableUntilDatePicker_SelectedDateChanged;
		disableUntilTimePickerTime.Time = dateTime2 + timeSpan - DateTime.MinValue;
		disableCommentTextBox.Text = _disableAlarmListItem.Comment;
		_initializing = false;
		Validate();
	}

	private void disableUntilDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
	{
		Validate();
	}

	private void disableUntilTimePickerTime_TimeChanged(object sender, EventArgs e)
	{
		Validate();
	}

	private void checkBox_Checked(object sender, RoutedEventArgs e)
	{
		Validate();
	}

	private void okButton_Click(object sender, RoutedEventArgs e)
	{
		if (disableForRadioButton.IsChecked == true && disableForTextBox.Text != string.Empty)
		{
			DateTime utcNow = DateTime.UtcNow;
			switch (disableForComboBox.SelectedIndex)
			{
			case 0:
				_disableAlarmListItem.Time = utcNow.AddMinutes(Convert.ToInt32(disableForTextBox.Text));
				break;
			case 1:
				_disableAlarmListItem.Time = utcNow.AddHours(Convert.ToInt32(disableForTextBox.Text));
				break;
			case 2:
				_disableAlarmListItem.Time = utcNow.AddDays(Convert.ToInt32(disableForTextBox.Text));
				break;
			}
		}
		else
		{
			_disableAlarmListItem.Time = disableUntilDatePicker.SelectedDate.GetValueOrDefault();
			_disableAlarmListItem.Time = TimeZoneInfo.ConvertTimeToUtc(_disableAlarmListItem.Time.Add(disableUntilTimePickerTime.Time));
		}
		_disableAlarmListItem.Comment = disableCommentTextBox.Text;
		Collection<string> collection = new Collection<string>();
		foreach (CheckBox item in (IEnumerable)alarmTypeListBox.Items)
		{
			if (item.IsChecked == true)
			{
				collection.Add(item.Content as string);
			}
		}
		_disableAlarmListItem.SetEventTypes(collection, _disableAlarmListItem.AvailableEventTypes);
		if (_callBack(_disableAlarmListItem, _serverId, _isModifying))
		{
			Close();
		}
	}

	private void disableForTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		if (!int.TryParse(e.Text, out var _))
		{
			e.Handled = true;
		}
	}

	private void disableForTextBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		Validate();
	}

	private void disableForRadioButton_Checked(object sender, RoutedEventArgs e)
	{
		disableUntilDatePicker.IsEnabled = false;
		disableUntilTimePickerTime.IsEnabled = false;
		disableForComboBox.IsEnabled = true;
		disableForTextBox.IsEnabled = true;
	}

	private void disableForRadioButton_Unchecked(object sender, RoutedEventArgs e)
	{
		disableUntilDatePicker.IsEnabled = true;
		disableUntilTimePickerTime.IsEnabled = true;
		disableForComboBox.IsEnabled = false;
		disableForTextBox.IsEnabled = false;
	}

	private void Validate()
	{
		if (_initializing)
		{
			return;
		}
		bool flag = true;
		bool flag2 = true;
		bool flag3 = false;
		if (disableForRadioButton.IsChecked == true)
		{
			if (disableForTextBox.Text.Equals(string.Empty))
			{
				flag = false;
			}
			if (!int.TryParse(disableForTextBox.Text, out var result))
			{
				flag = false;
			}
			if (result < 1)
			{
				flag = false;
			}
		}
		else
		{
			DateTime dateTime = TimeZoneInfo.ConvertTimeToUtc(disableUntilDatePicker.SelectedDate.GetValueOrDefault().Add(disableUntilTimePickerTime.Time));
			if (dateTime <= DateTime.UtcNow)
			{
				flag2 = false;
			}
		}
		if (alarmTypeListBox.Items.Count == 0)
		{
			flag3 = false;
		}
		else
		{
			foreach (CheckBox item in (IEnumerable)alarmTypeListBox.Items)
			{
				if (item.IsChecked == true)
				{
					flag3 = true;
					break;
				}
			}
		}
		if (flag && flag2 && flag3)
		{
			okButton.IsEnabled = true;
		}
		else
		{
			okButton.IsEnabled = false;
		}
	}

	private void Window_Closing(object sender, CancelEventArgs e)
	{
		foreach (CheckBox item in (IEnumerable)alarmTypeListBox.Items)
		{
			item.Checked -= checkBox_Checked;
			item.Unchecked -= checkBox_Checked;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
