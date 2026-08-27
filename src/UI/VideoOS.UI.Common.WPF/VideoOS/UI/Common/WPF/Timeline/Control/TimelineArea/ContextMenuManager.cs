using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using VideoOS.Platform.UI.Controls;
using VideoOS.UI.Common.WPF.Timeline.Control.Controllers;
using VideoOS.UI.Common.WPF.Timeline.Control.Data;
using VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.resources;
using VideoOS.UI.Common.WPF.Timeline.Control.UIControls.SelectTimePopup;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea;

public class ContextMenuManager
{
	private TimeAndSpanController _timeAndSpanController;

	private AppearanceAndBehaviourController _appearanceAndBehaviourController;

	private VideoOSContextMenuLarge _contextMenu;

	private VideoOSContextMenuItem _selectTimeMenuItem;

	private VideoOSContextMenuItem _spanMenuItem;

	private VideoOSContextMenuItem _setSelectionFromMenuItem;

	private VideoOSContextMenuItem _setSelectionCenterMenuItem;

	private VideoOSContextMenuItem _setSelectionToMenuItem;

	private VideoOSContextMenuItem _selectSelectionFromTimeMenuItem;

	private VideoOSContextMenuItem _selectSelectionCenterTimeMenuItem;

	private VideoOSContextMenuItem _selectSelectionToTimeMenuItem;

	private VideoOSContextMenuItem _toggleTimeSelectionStateMenuItem;

	private VideoOSContextMenuItem _playSelectionMenuItem;

	private List<KeyValuePair<ContextMenuMode, VideoOSContextMenuItem>> _customMenuItems = new List<KeyValuePair<ContextMenuMode, VideoOSContextMenuItem>>();

	public bool IsContextMenuOpen => _contextMenu.IsOpen;

	public void Init(TimeAndSpanController timeAndSpanController, AppearanceAndBehaviourController appearanceAndBehaviourController, UIElement parent)
	{
		_timeAndSpanController = timeAndSpanController;
		_appearanceAndBehaviourController = appearanceAndBehaviourController;
		_contextMenu?.Items.Clear();
		_contextMenu = new VideoOSContextMenuLarge();
		_contextMenu.PlacementTarget = parent;
		_selectTimeMenuItem = CreateMenuItem(Strings.SelectTimeContextMenuItemText, null, selectTimeMenuItem_Click);
		_spanMenuItem = CreateMenuItem(Strings.SpanContextMenuItemText, null, null);
		foreach (TimelineSpan timelineSpan in _appearanceAndBehaviourController.TimelineSpans)
		{
			_spanMenuItem.Items.Add(CreateMenuItem(timelineSpan.Text, timelineSpan, spanMenuItem_Click));
		}
		_setSelectionFromMenuItem = CreateMenuItem(Strings.SetSelectionFromContextMenuItemText, null, setSelectionFromMenuItem_Click);
		_setSelectionCenterMenuItem = CreateMenuItem(Strings.SetSelectionCenterContextMenuItemText, null, setSelectionCenterMenuItem_Click);
		_setSelectionToMenuItem = CreateMenuItem(Strings.SetSelectionToContextMenuItemText, null, setSelectionToMenuItem_Click);
		_selectSelectionFromTimeMenuItem = CreateMenuItem(Strings.SelectSelectionFromTimeContextMenuItemText, null, selectSelectionTimeMenuItem_Click);
		_selectSelectionCenterTimeMenuItem = CreateMenuItem(Strings.SelectSelectionCenterTimeContextMenuItemText, null, selectSelectionTimeMenuItem_Click);
		_selectSelectionToTimeMenuItem = CreateMenuItem(Strings.SelectSelectionToTimeContextMenuItemText, null, selectSelectionTimeMenuItem_Click);
		_playSelectionMenuItem = CreateMenuItem(Strings.PlaySelectionContextMenuItemText, null, playSelectionStateMenuItem_Click);
		_toggleTimeSelectionStateMenuItem = CreateMenuItem(Strings.ToggleTimeSelectionStateContextMenuItem, null, toggleTimeSelectionStateMenuItem_Click, isCheckable: true);
		_contextMenu.Items.Add(_selectTimeMenuItem);
		_contextMenu.Items.Add(_spanMenuItem);
		_contextMenu.Items.Add(_setSelectionFromMenuItem);
		_contextMenu.Items.Add(_setSelectionCenterMenuItem);
		_contextMenu.Items.Add(_setSelectionToMenuItem);
		_contextMenu.Items.Add(_selectSelectionFromTimeMenuItem);
		_contextMenu.Items.Add(_selectSelectionCenterTimeMenuItem);
		_contextMenu.Items.Add(_selectSelectionToTimeMenuItem);
		_contextMenu.Items.Add(_playSelectionMenuItem);
		_contextMenu.Items.Add(_toggleTimeSelectionStateMenuItem);
	}

	public void ShowContextMenu(ContextMenuMode contextMenuMode)
	{
		_selectTimeMenuItem.Visibility = Visibility.Collapsed;
		_spanMenuItem.Visibility = Visibility.Collapsed;
		_setSelectionFromMenuItem.Visibility = Visibility.Collapsed;
		_setSelectionCenterMenuItem.Visibility = Visibility.Collapsed;
		_setSelectionToMenuItem.Visibility = Visibility.Collapsed;
		_selectSelectionFromTimeMenuItem.Visibility = Visibility.Collapsed;
		_selectSelectionCenterTimeMenuItem.Visibility = Visibility.Collapsed;
		_selectSelectionToTimeMenuItem.Visibility = Visibility.Collapsed;
		_playSelectionMenuItem.Visibility = Visibility.Collapsed;
		_toggleTimeSelectionStateMenuItem.Visibility = Visibility.Collapsed;
		switch (contextMenuMode)
		{
		case ContextMenuMode.TimelineNormal:
			_selectTimeMenuItem.Visibility = Visibility.Visible;
			_spanMenuItem.Visibility = Visibility.Visible;
			_toggleTimeSelectionStateMenuItem.Visibility = Visibility.Visible;
			break;
		case ContextMenuMode.TimelineOutsideSelection:
			_selectTimeMenuItem.Visibility = Visibility.Visible;
			_spanMenuItem.Visibility = Visibility.Visible;
			_toggleTimeSelectionStateMenuItem.Visibility = Visibility.Visible;
			break;
		case ContextMenuMode.TimelineInsideSelection:
			_selectTimeMenuItem.Visibility = Visibility.Visible;
			_spanMenuItem.Visibility = Visibility.Visible;
			_toggleTimeSelectionStateMenuItem.Visibility = Visibility.Visible;
			_playSelectionMenuItem.Visibility = Visibility.Visible;
			break;
		case ContextMenuMode.TimelineCenterMarker:
			_selectTimeMenuItem.Visibility = Visibility.Visible;
			_spanMenuItem.Visibility = Visibility.Visible;
			if (_timeAndSpanController.SelectionState != SelectionState.Disabled)
			{
				_setSelectionFromMenuItem.Visibility = Visibility.Visible;
				if (_timeAndSpanController.SelectionMode == SelectionMode.FromCenterTo)
				{
					_setSelectionCenterMenuItem.Visibility = Visibility.Visible;
				}
				_setSelectionToMenuItem.Visibility = Visibility.Visible;
				_playSelectionMenuItem.Visibility = Visibility.Visible;
			}
			_toggleTimeSelectionStateMenuItem.Visibility = Visibility.Visible;
			break;
		case ContextMenuMode.TimelineSelectionFromMarker:
			_selectSelectionFromTimeMenuItem.Visibility = Visibility.Visible;
			_playSelectionMenuItem.Visibility = Visibility.Visible;
			_toggleTimeSelectionStateMenuItem.Visibility = Visibility.Visible;
			break;
		case ContextMenuMode.TimelineSelectionCenterMarker:
			_selectSelectionCenterTimeMenuItem.Visibility = Visibility.Visible;
			_playSelectionMenuItem.Visibility = Visibility.Visible;
			_toggleTimeSelectionStateMenuItem.Visibility = Visibility.Visible;
			break;
		case ContextMenuMode.TimelineSelectionToMarker:
			_selectSelectionToTimeMenuItem.Visibility = Visibility.Visible;
			_playSelectionMenuItem.Visibility = Visibility.Visible;
			_toggleTimeSelectionStateMenuItem.Visibility = Visibility.Visible;
			break;
		}
		if (!_appearanceAndBehaviourController.UserSelectionControlEnabled)
		{
			_toggleTimeSelectionStateMenuItem.Visibility = Visibility.Collapsed;
		}
		if (!_appearanceAndBehaviourController.ShowSpanContextMenuItem)
		{
			_spanMenuItem.Visibility = Visibility.Collapsed;
		}
		if (_spanMenuItem.Visibility == Visibility.Visible)
		{
			foreach (MenuItem item in (IEnumerable)_spanMenuItem.Items)
			{
				if (((TimelineSpan)item.Tag).TimeSpan == _timeAndSpanController.TargetTimelineSpan)
				{
					item.IsChecked = true;
				}
				else
				{
					item.IsChecked = false;
				}
			}
		}
		if (_setSelectionFromMenuItem.Visibility == Visibility.Visible)
		{
			bool isEnabled = true;
			if (_timeAndSpanController.SelectionMode == SelectionMode.FromCenterTo && _timeAndSpanController.SelectionCenterTime < _timeAndSpanController.TimelineTime)
			{
				isEnabled = false;
			}
			if (_timeAndSpanController.SelectionToTime < _timeAndSpanController.TimelineTime)
			{
				isEnabled = false;
			}
			if (_timeAndSpanController.SelectionFromTime == _timeAndSpanController.TimelineTime)
			{
				isEnabled = false;
			}
			_setSelectionFromMenuItem.IsEnabled = isEnabled;
		}
		if (_setSelectionCenterMenuItem.Visibility == Visibility.Visible)
		{
			bool isEnabled2 = true;
			if (_timeAndSpanController.SelectionFromTime > _timeAndSpanController.TimelineTime)
			{
				isEnabled2 = false;
			}
			else if (_timeAndSpanController.SelectionToTime < _timeAndSpanController.TimelineTime)
			{
				isEnabled2 = false;
			}
			if (_timeAndSpanController.SelectionCenterTime == _timeAndSpanController.TimelineTime)
			{
				isEnabled2 = false;
			}
			_setSelectionCenterMenuItem.IsEnabled = isEnabled2;
		}
		if (_setSelectionToMenuItem.Visibility == Visibility.Visible)
		{
			bool isEnabled3 = true;
			if (_timeAndSpanController.SelectionMode == SelectionMode.FromCenterTo && _timeAndSpanController.SelectionCenterTime > _timeAndSpanController.TimelineTime)
			{
				isEnabled3 = false;
			}
			if (_timeAndSpanController.SelectionFromTime > _timeAndSpanController.TimelineTime)
			{
				isEnabled3 = false;
			}
			if (_timeAndSpanController.SelectionToTime == _timeAndSpanController.TimelineTime)
			{
				isEnabled3 = false;
			}
			_setSelectionToMenuItem.IsEnabled = isEnabled3;
		}
		if (_toggleTimeSelectionStateMenuItem.Visibility == Visibility.Visible)
		{
			if (_timeAndSpanController.SelectionState == SelectionState.Disabled)
			{
				_toggleTimeSelectionStateMenuItem.IsChecked = false;
			}
			else
			{
				_toggleTimeSelectionStateMenuItem.IsChecked = true;
			}
		}
		foreach (KeyValuePair<ContextMenuMode, VideoOSContextMenuItem> customMenuItem in _customMenuItems)
		{
			if (customMenuItem.Key == contextMenuMode || customMenuItem.Key == ContextMenuMode.TimelineAll)
			{
				if (!_contextMenu.Items.Contains(customMenuItem.Value))
				{
					_contextMenu.Items.Add(customMenuItem.Value);
				}
			}
			else if (_contextMenu.Items.Contains(customMenuItem.Value))
			{
				_contextMenu.Items.Remove(customMenuItem.Value);
			}
		}
		_contextMenu.IsOpen = true;
	}

	public void AddContextMenuItem(VideoOSContextMenuItem menuItem, ContextMenuMode contextMenuMode)
	{
		_customMenuItems.Add(new KeyValuePair<ContextMenuMode, VideoOSContextMenuItem>(contextMenuMode, menuItem));
	}

	public void RemoveContextMenuItem(VideoOSContextMenuItem menuItem, ContextMenuMode contextMenuMode)
	{
		foreach (KeyValuePair<ContextMenuMode, VideoOSContextMenuItem> customMenuItem in _customMenuItems)
		{
			if (customMenuItem.Key == contextMenuMode && customMenuItem.Value == menuItem)
			{
				_customMenuItems.Remove(customMenuItem);
				_contextMenu.Items.Remove(customMenuItem.Value);
				break;
			}
		}
	}

	private void selectTimeMenuItem_Click(object sender, RoutedEventArgs e)
	{
		TimelineSelectTimePopupHelper.OpenAndHandleSelectTimelineTime(_timeAndSpanController, _appearanceAndBehaviourController, _contextMenu.PlacementTarget);
	}

	private void spanMenuItem_Click(object sender, RoutedEventArgs e)
	{
		TimelineSpan timelineSpan = (TimelineSpan)((MenuItem)sender).Tag;
		_timeAndSpanController.SetSpan(timelineSpan.TimeSpan, animate: true);
	}

	private void setSelectionFromMenuItem_Click(object sender, RoutedEventArgs e)
	{
		_timeAndSpanController.SetSelectionTimes(_timeAndSpanController.TimelineTime, _timeAndSpanController.SelectionCenterTime, _timeAndSpanController.SelectionToTime, adjustToFromTimeIfNessesary: true, adjustToCenterTimeIfNessesary: false, adjustToToTimeIfNessesary: false);
	}

	private void setSelectionCenterMenuItem_Click(object sender, RoutedEventArgs e)
	{
		_timeAndSpanController.SetSelectionTimes(_timeAndSpanController.SelectionFromTime, _timeAndSpanController.TimelineTime, _timeAndSpanController.SelectionToTime, adjustToFromTimeIfNessesary: false, adjustToCenterTimeIfNessesary: true, adjustToToTimeIfNessesary: false);
	}

	private void setSelectionToMenuItem_Click(object sender, RoutedEventArgs e)
	{
		_timeAndSpanController.SetSelectionTimes(_timeAndSpanController.SelectionFromTime, _timeAndSpanController.SelectionCenterTime, _timeAndSpanController.TimelineTime, adjustToFromTimeIfNessesary: false, adjustToCenterTimeIfNessesary: false, adjustToToTimeIfNessesary: true);
	}

	private void toggleTimeSelectionStateMenuItem_Click(object sender, RoutedEventArgs e)
	{
		if (_timeAndSpanController.SelectionState == SelectionState.Disabled)
		{
			DateTime defaultSelectionFromTime = _timeAndSpanController.DefaultSelectionFromTime;
			DateTime defaultSelectionCenterTime = _timeAndSpanController.DefaultSelectionCenterTime;
			DateTime defaultSelectionToTime = _timeAndSpanController.DefaultSelectionToTime;
			_timeAndSpanController.SetSelectionTimes(defaultSelectionFromTime, defaultSelectionCenterTime, defaultSelectionToTime, adjustToFromTimeIfNessesary: false, adjustToCenterTimeIfNessesary: false, adjustToToTimeIfNessesary: false);
			_timeAndSpanController.SelectionState = SelectionState.Enabled;
		}
		else
		{
			_timeAndSpanController.SelectionState = SelectionState.Disabled;
		}
	}

	private void playSelectionStateMenuItem_Click(object sender, RoutedEventArgs e)
	{
		_appearanceAndBehaviourController.PlaybackState = PlaybackState.Stopped;
		_timeAndSpanController.SetTimelineTime(_timeAndSpanController.SelectionFromTime, animate: false);
		_appearanceAndBehaviourController.PlaybackState = PlaybackState.Forward;
	}

	private void selectSelectionTimeMenuItem_Click(object sender, RoutedEventArgs e)
	{
		TimelineSelectTimePopupHelper.OpenAndHandleSelectSelectionTimes(_timeAndSpanController, _appearanceAndBehaviourController, _contextMenu.PlacementTarget);
	}

	private VideoOSContextMenuItem CreateMenuItem(string header, object tag, RoutedEventHandler clickHandler, bool isCheckable = false, bool isChecked = false, bool staysOpenOnClick = false)
	{
		VideoOSContextMenuItem videoOSContextMenuItem = new VideoOSContextMenuItem();
		videoOSContextMenuItem.Data = header;
		videoOSContextMenuItem.Tag = tag;
		if (clickHandler != null)
		{
			videoOSContextMenuItem.Click += clickHandler;
		}
		videoOSContextMenuItem.IsCheckable = isCheckable;
		videoOSContextMenuItem.IsChecked = isChecked;
		videoOSContextMenuItem.StaysOpenOnClick = staysOpenOnClick;
		return videoOSContextMenuItem;
	}
}
