using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Threading;
using VideoOS.Platform.UI.Controls;
using VideoOS.UI.Common.WPF.Timeline.Cache;
using VideoOS.UI.Common.WPF.Timeline.Control.Controllers;
using VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.Timelines.SlicedTimeline;
using VideoOS.UI.Common.WPF.Timeline.Events;
using VideoOS.UI.Common.WPF.Timeline.InterfaceData;
using VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea;

public partial class TimelineArea : UserControl, IComponentConnector
{
	private TimeAndSpanController _timeAndSpanController = new TimeAndSpanController();

	private AppearanceAndBehaviourController _appearanceAndBehaviourController = new AppearanceAndBehaviourController();

	private ITimelineSequenceCache _timelineSequenceCache;

	private ContextMenuManager _contextMenuManager = new ContextMenuManager();

	private SequenceSourcesInfo _upperSequenceSourcesInfo = new SequenceSourcesInfo();

	private SequenceSourcesInfo _lowerSequenceSourcesInfo = new SequenceSourcesInfo();

	private DispatcherTimer _dispatcherTimer = new DispatcherTimer();

	private HwndSource _hwndSource;

	public event EventHandler<MarkerEventArgs> UpperTimelineMarkerHovered;

	public event EventHandler<MarkerEventArgs> LowerTimelineMarkerHovered;

	public event EventHandler<PlaybackNavigationEventArgs> PlaybackNavigationOccurred;

	public TimelineArea()
	{
		InitializeComponent();
		_dispatcherTimer.Interval = TimeSpan.FromMilliseconds(300.0);
		_dispatcherTimer.Tick += _dispatcherTimer_Tick;
	}

	public void Init(TimeAndSpanController timeAndSpanController, AppearanceAndBehaviourController appearanceAndBehaviourController, ITimelineSequenceCache timelineCache)
	{
		_timeAndSpanController = timeAndSpanController;
		_appearanceAndBehaviourController = appearanceAndBehaviourController;
		_timelineSequenceCache = timelineCache;
		_timeAndSpanController.SetPixelCount((int)_layersGrid.ActualWidth);
		_timelinesLayer.Init(_timeAndSpanController, _appearanceAndBehaviourController, _timelineSequenceCache);
		_timeMarkersLayer.Init(_timeAndSpanController, _appearanceAndBehaviourController, _timelineSequenceCache);
		_dragLayer.Init(_timeAndSpanController, _appearanceAndBehaviourController, _timelineSequenceCache);
		_selectionLayer.Init(_timeAndSpanController, _appearanceAndBehaviourController, _timelineSequenceCache);
		_controlsLayer.Init(_timeAndSpanController, _appearanceAndBehaviourController, _timelineSequenceCache);
		_timelineLabelsLayer.Init(_timeAndSpanController, _appearanceAndBehaviourController, _timelineSequenceCache);
		_contextMenuManager.Init(_timeAndSpanController, _appearanceAndBehaviourController, this);
		_upperSequenceSourcesInfo = new SequenceSourcesInfo(_appearanceAndBehaviourController.UpperSequenceSources);
		_lowerSequenceSourcesInfo = new SequenceSourcesInfo(_appearanceAndBehaviourController.LowerSequenceSources);
		_appearanceAndBehaviourController.PropertyChanged += _appearanceAndBehaviourController_PropertyChanged;
		_hwndSource = (HwndSource)PresentationSource.FromDependencyObject(this);
		if (_hwndSource != null)
		{
			_hwndSource.AddHook(WindowProc);
		}
		UpdateHeight();
	}

	public void Close()
	{
		_timelinesLayer.Close();
		_timeMarkersLayer.Close();
		_dragLayer.Close();
		_selectionLayer.Close();
		_controlsLayer.Close();
		_timelineLabelsLayer.Close();
	}

	private void TimelineArea_Unloaded(object sender, RoutedEventArgs e)
	{
		if (_hwndSource != null)
		{
			_hwndSource.RemoveHook(WindowProc);
			_hwndSource = null;
		}
	}

	public void StopThrow()
	{
		_dragLayer.StopThrow();
	}

	public void AddContextMenuItem(VideoOSContextMenuItem menuItem, ContextMenuMode contextMenuMode)
	{
		_contextMenuManager.AddContextMenuItem(menuItem, contextMenuMode);
	}

	public void RemoveContextMenuItem(VideoOSContextMenuItem menuItem, ContextMenuMode contextMenuMode)
	{
		_contextMenuManager.RemoveContextMenuItem(menuItem, contextMenuMode);
	}

	private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
	{
		if (msg == 526)
		{
			long num;
			try
			{
				num = wParam.ToInt64();
			}
			catch (OverflowException)
			{
				return IntPtr.Zero;
			}
			long num2 = (num >> 16) & 0xFFFF;
			if (num2 > 0 && num2 < 32768)
			{
				ScrollTimelineSpan(increase: true);
			}
			else
			{
				ScrollTimelineSpan(increase: false);
			}
		}
		return IntPtr.Zero;
	}

	private void _appearanceAndBehaviourController_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == AppearanceAndBehaviourController.PropertyNames.TimelineAreaDisplayMode.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineTopLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineBottomLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineCustomDataLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineTopLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineBottomLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineCustomDataLine.ToString())
		{
			UpdateHeight();
		}
		else if (e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSources.ToString())
		{
			_upperSequenceSourcesInfo = new SequenceSourcesInfo(_appearanceAndBehaviourController.UpperSequenceSources);
			_lowerSequenceSourcesInfo = new SequenceSourcesInfo(_appearanceAndBehaviourController.LowerSequenceSources);
		}
	}

	private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		Focus();
		e.Handled = true;
	}

	private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
	{
		if ((Keyboard.Modifiers & ModifierKeys.Control) > ModifierKeys.None)
		{
			ScrollTimelineSpan(e.Delta > 0);
		}
		else
		{
			ScrollTimelineTime(e.Delta > 0);
		}
		e.Handled = true;
	}

	private void _layersGrid_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		if (_timeAndSpanController != null)
		{
			_timeAndSpanController.SetPixelCount((int)e.NewSize.Width);
		}
	}

	private void _dragLayer_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
	{
		DateTime pixelTime = _timeAndSpanController.GetPixelTime((int)e.GetPosition(_dragLayer).X);
		if (_timeAndSpanController.SelectionState == SelectionState.Disabled)
		{
			_contextMenuManager.ShowContextMenu(ContextMenuMode.TimelineNormal);
		}
		else if (pixelTime >= _timeAndSpanController.SelectionFromTime && pixelTime <= _timeAndSpanController.SelectionToTime)
		{
			_contextMenuManager.ShowContextMenu(ContextMenuMode.TimelineInsideSelection);
		}
		else
		{
			_contextMenuManager.ShowContextMenu(ContextMenuMode.TimelineOutsideSelection);
		}
	}

	private void _timelineLabelsLayer_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		_dragLayer.SimulateLeftMouseDown(e);
	}

	private void _controlsLayer_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		_dragLayer.SimulateLeftMouseDown(e);
	}

	private void _controlsLayer_CenterTimeMarkerRightClicked(object sender, EventArgs e)
	{
		_contextMenuManager.ShowContextMenu(ContextMenuMode.TimelineCenterMarker);
	}

	private void _controlsLayer_PlaybackNavigationOccurred(object sender, PlaybackNavigationEventArgs e)
	{
		if (this.PlaybackNavigationOccurred != null)
		{
			this.PlaybackNavigationOccurred(this, e);
		}
	}

	private void _selectionLayer_SelectionFromMarkerRightClicked(object sender, EventArgs e)
	{
		_contextMenuManager.ShowContextMenu(ContextMenuMode.TimelineSelectionFromMarker);
	}

	private void _selectionLayer_SelectionCenterMarkerRightClicked(object sender, EventArgs e)
	{
		_contextMenuManager.ShowContextMenu(ContextMenuMode.TimelineSelectionCenterMarker);
	}

	private void _selectionLayer_SelectionToMarkerRightClicked(object sender, EventArgs e)
	{
		_contextMenuManager.ShowContextMenu(ContextMenuMode.TimelineSelectionToMarker);
	}

	private void _selectionLayer_SelectionOutOfScopeMarkerClicked(object sender, EventArgs e)
	{
		StopThrow();
	}

	private void _layersGrid_MouseMove(object sender, MouseEventArgs e)
	{
		_dispatcherTimer.Stop();
		_dispatcherTimer.Start();
	}

	private void _dispatcherTimer_Tick(object sender, EventArgs e)
	{
		_dispatcherTimer.Stop();
		if (!_timelinesLayer.IsLoaded || _contextMenuManager.IsContextMenuOpen)
		{
			return;
		}
		Point position = Mouse.GetPosition(_timelinesLayer);
		if (_timelinesLayer.IsPointOverUpperTimeline(position))
		{
			if (this.UpperTimelineMarkerHovered != null)
			{
				object hoveredMarker = GetHoveredMarker(_upperSequenceSourcesInfo.MarkerSequenceSources, position.X);
				if (hoveredMarker != null)
				{
					this.UpperTimelineMarkerHovered(this, new MarkerEventArgs(hoveredMarker));
				}
			}
		}
		else if (_timelinesLayer.IsPointOverLowerTimeline(position) && this.LowerTimelineMarkerHovered != null)
		{
			object hoveredMarker2 = GetHoveredMarker(_lowerSequenceSourcesInfo.MarkerSequenceSources, position.X);
			if (hoveredMarker2 != null)
			{
				this.LowerTimelineMarkerHovered(this, new MarkerEventArgs(hoveredMarker2));
			}
		}
	}

	private object GetHoveredMarker(IEnumerable<ISequenceSource> sequenceSources, double hoverXPosition)
	{
		if (sequenceSources.Any())
		{
			int num = (int)_appearanceAndBehaviourController.SequenceSourceVisualAppearanceMarkerImage.Width;
			DateTime pixelTime = _timeAndSpanController.GetPixelTime((int)hoverXPosition);
			DateTime dateTime = new DateTime(pixelTime.Ticks - _timeAndSpanController.GetTicksPerPixel() * num);
			ISequenceSource[] timelineData = _timelineSequenceCache.GetTimelineData(sequenceSources, dateTime, new TimeSpan(_timeAndSpanController.GetTicksPerPixel()), num * 2);
			int num2 = -1;
			int num3 = timelineData.Length / 2;
			int num4 = num3 + 1;
			while (num3 >= 0)
			{
				if (timelineData[num3] != null)
				{
					num2 = num3;
					break;
				}
				if (num4 < timelineData.Length && timelineData[num4] != null)
				{
					num2 = num4;
					break;
				}
				num3--;
				num4++;
			}
			if (num2 != -1)
			{
				IEnumerable<Sequence> sequences = _timelineSequenceCache.GetSequences(new List<ISequenceSource> { timelineData[num2] }, dateTime, pixelTime + (pixelTime - dateTime));
				Sequence sequence = null;
				foreach (Sequence item in sequences)
				{
					if (sequence == null || (pixelTime - item.MiddleTime).Duration() < (pixelTime - sequence.MiddleTime).Duration())
					{
						sequence = item;
						continue;
					}
					break;
				}
				if (sequence != null)
				{
					return sequence.Tag;
				}
			}
		}
		return null;
	}

	private void UpdateHeight()
	{
		_layersGrid.Height = _appearanceAndBehaviourController.TimelineAreaRulerSectionHeight + _appearanceAndBehaviourController.TimelineAreaTracksSectionHeight;
	}

	private void ScrollTimelineSpan(bool increase)
	{
		int num = -1;
		for (int i = 0; i < _appearanceAndBehaviourController.TimelineSpans.Count; i++)
		{
			if (_appearanceAndBehaviourController.TimelineSpans[i].TimeSpan == _timeAndSpanController.TargetTimelineSpan)
			{
				num = i;
				break;
			}
		}
		if (increase && num > 0)
		{
			_timeAndSpanController.SetSpan(_appearanceAndBehaviourController.TimelineSpans[num - 1].TimeSpan, animate: true);
		}
		else if (!increase && num < _appearanceAndBehaviourController.TimelineSpans.Count - 1)
		{
			_timeAndSpanController.SetSpan(_appearanceAndBehaviourController.TimelineSpans[num + 1].TimeSpan, animate: true);
		}
	}

	private void ScrollTimelineTime(bool increase)
	{
		TimeSpan timeSpan = new TimeSpan(_timeAndSpanController.TimelineSpan.Ticks / 100);
		DateTime time = ((!increase) ? (_timeAndSpanController.TargetTimelineTime - timeSpan) : (_timeAndSpanController.TargetTimelineTime + timeSpan));
		_timeAndSpanController.SetTimelineTime(time, animate: true);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
