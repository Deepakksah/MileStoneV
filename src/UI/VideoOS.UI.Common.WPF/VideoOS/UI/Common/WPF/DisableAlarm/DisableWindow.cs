using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using VideoOS.UI.Common.WPF.Resources;
using VideoOS.UI.Common.WPF.Utils.RightToLeft;

namespace VideoOS.UI.Common.WPF.DisableAlarm;

public partial class DisableWindow : Window, IComponentConnector
{
	private DisableWindowCallback _callBack;

	private DisableWindowDeleteCallback _deleteCallBack;

	private DisableWindowConvertTimeCallback _convertTimeCallBack;

	private MenuItem _disableModifyMenuItem;

	private MenuItem _disableDeleteMenuItem;

	private ObservableCollection<ObservableDisableItem> _observableDisableCollection = new ObservableCollection<ObservableDisableItem>();

	private Dictionary<Guid, ObservableDisableItem> _lookupObservableItem = new Dictionary<Guid, ObservableDisableItem>();

	private ContextMenu _disableContextmenu;

	public ObservableCollection<ObservableDisableItem> ObservableDisableCollection => _observableDisableCollection;

	public string StrResDisableTimeOut => Strings.DisableAlarmsTimeOut;

	public string StrResDisableEvents => Strings.DisableAlarmsEvents;

	public string StrResDisableSource => Strings.DisableAlarmsSource;

	public DisableWindow(DisableWindowCallback callBack, DisableWindowDeleteCallback removeCallBack, DisableWindowConvertTimeCallback convertTimeCallBack)
	{
		InitializeComponent();
		base.FlowDirection = RtlUtility.SelectedFlowDirection;
		_callBack = callBack;
		_deleteCallBack = removeCallBack;
		_convertTimeCallBack = convertTimeCallBack;
		_disableContextmenu = new ContextMenu();
		_disableModifyMenuItem = new MenuItem();
		_disableModifyMenuItem.Header = Strings.DisableAlarmsContextMenuModify;
		_disableContextmenu.Items.Insert(0, _disableModifyMenuItem);
		_disableDeleteMenuItem = new MenuItem();
		_disableDeleteMenuItem.Header = Strings.DisableAlarmsContextMenuDelete;
		_disableContextmenu.Items.Insert(1, _disableDeleteMenuItem);
		_disableModifyMenuItem.Click += _disableModifyMenuItem_Click;
		_disableDeleteMenuItem.Click += _disableDeleteMenuItem_Click;
	}

	private void Window_Closing(object sender, CancelEventArgs e)
	{
		if (_disableModifyMenuItem != null)
		{
			_disableModifyMenuItem.Click -= _disableModifyMenuItem_Click;
		}
		if (_disableDeleteMenuItem != null)
		{
			_disableDeleteMenuItem.Click -= _disableDeleteMenuItem_Click;
		}
	}

	public void Update(DisableAlarmListItem[] disabledAlarms)
	{
		Guid guid = Guid.NewGuid();
		foreach (DisableAlarmListItem disableAlarmListItem in disabledAlarms)
		{
			if (_lookupObservableItem.ContainsKey(disableAlarmListItem.DisableAlarmItemId))
			{
				_lookupObservableItem[disableAlarmListItem.DisableAlarmItemId].DisableAlarmItem = disableAlarmListItem;
				_lookupObservableItem[disableAlarmListItem.DisableAlarmItemId].UpdateId = guid;
				continue;
			}
			ObservableDisableItem observableDisableItem = new ObservableDisableItem(disableAlarmListItem);
			_lookupObservableItem.Add(disableAlarmListItem.DisableAlarmItemId, observableDisableItem);
			observableDisableItem.UpdateId = guid;
			_observableDisableCollection.Add(observableDisableItem);
		}
		for (int num = _observableDisableCollection.Count - 1; num >= 0; num--)
		{
			if (_observableDisableCollection[num].UpdateId != guid)
			{
				_lookupObservableItem.Remove(_observableDisableCollection[num].DisableAlarmItem.DisableAlarmItemId);
				_observableDisableCollection.RemoveAt(num);
			}
		}
	}

	private void _disableModifyMenuItem_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (!(_disableListView.SelectedItem is ObservableDisableItem { DisableAlarmItem: { } disableAlarmItem }))
			{
				return;
			}
			DisableSetupWindow disableSetupWindow = new DisableSetupWindow(disableAlarmItem, disableAlarmItem.ServerId, isModifying: true, _callBack, _convertTimeCallBack);
			foreach (ResourceDictionary mergedDictionary in base.Resources.MergedDictionaries)
			{
				disableSetupWindow.Resources.MergedDictionaries.Add(mergedDictionary);
			}
			new WindowInteropHelper(disableSetupWindow).Owner = new WindowInteropHelper(this).Handle;
			disableSetupWindow.ShowDialog();
		}
		catch
		{
		}
	}

	private void _disableDeleteMenuItem_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			foreach (ObservableDisableItem selectedItem in _disableListView.SelectedItems)
			{
				DisableAlarmListItem disableAlarmItem = selectedItem.DisableAlarmItem;
				if (disableAlarmItem != null)
				{
					_deleteCallBack(disableAlarmItem.DisableAlarmItemId, disableAlarmItem.SourceId, disableAlarmItem.ServerId);
				}
			}
		}
		catch
		{
		}
	}

	private void _disableListView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
	{
		if (_disableListView.SelectedItems.Count > 1)
		{
			_disableModifyMenuItem.IsEnabled = false;
		}
		else
		{
			_disableModifyMenuItem.IsEnabled = true;
		}
	}

	private void _disableListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		try
		{
			if (!(_disableListView.SelectedItem is ObservableDisableItem { DisableAlarmItem: { } disableAlarmItem }))
			{
				return;
			}
			DisableSetupWindow disableSetupWindow = new DisableSetupWindow(disableAlarmItem, disableAlarmItem.ServerId, isModifying: true, _callBack, _convertTimeCallBack);
			foreach (ResourceDictionary mergedDictionary in base.Resources.MergedDictionaries)
			{
				disableSetupWindow.Resources.MergedDictionaries.Add(mergedDictionary);
			}
			new WindowInteropHelper(disableSetupWindow).Owner = new WindowInteropHelper(this).Handle;
			disableSetupWindow.ShowDialog();
		}
		catch
		{
		}
	}

	private void _disableListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (_disableListView.SelectedItem != null)
		{
			_disableListView.ContextMenu = _disableContextmenu;
		}
		else
		{
			_disableListView.ContextMenu = null;
		}
	}
}
