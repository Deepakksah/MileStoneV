#define TRACE
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI;
using VideoOS.UI.Common.WPF.Timeline.Cache;
using VideoOS.UI.Common.WPF.Timeline.Control.Controllers;
using VideoOS.UI.Common.WPF.Timeline.Events;
using VideoOS.Utilities.DateTimeHelper;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.Selection;

public partial class SelectionLayer : LayerBase, IComponentConnector
{
	private static readonly int MARKER_SCROLL_AREA_WIDTH = 60;

	private static readonly int MARKER_MIN_MARGIN = 10;

	private static readonly int MARKER_SNAP_MARGIN = 10;

	private double _selectionMarkerMouseLeftButtonDownPosition = -1.0;

	private SelectionMarker _draggedSelectionMarker;

	private DispatcherTimer _timelineMoveTimer;

	public event EventHandler<EventArgs> SelectionFromMarkerRightClicked;

	public event EventHandler<EventArgs> SelectionCenterMarkerRightClicked;

	public event EventHandler<EventArgs> SelectionToMarkerRightClicked;

	public event EventHandler<EventArgs> SelectionOutOfScopeMarkerClicked;

	public SelectionLayer()
	{
		InitializeComponent();
		_timelineMoveTimer = new DispatcherTimer();
		_timelineMoveTimer.Interval = TimeSpan.FromMilliseconds(20.0);
		_timelineMoveTimer.Tick += _timelineMoveTimer_Tick;
	}

	public override void Init(TimeAndSpanController timeAndSpanController, AppearanceAndBehaviourController appearanceAndBehaviourController, ITimelineSequenceCache timelineCache)
	{
		base.Init(timeAndSpanController, appearanceAndBehaviourController, timelineCache);
		base.TimeAndSpanController.TimelinePixelTimesChanged += timeAndSpanController_TimelinePixelTimesChangedEvent;
		base.TimeAndSpanController.SelectionModeChangedEvent += timeAndSpanController_SelectionModeChangedEvent;
		base.TimeAndSpanController.SelectionStateChangedEvent += timeAndSpanController_SelectionStateChangedEvent;
		base.TimeAndSpanController.SelectionActiveMarkerChangedEvent += timeAndSpanController_SelectionActiveMarkerChangedEvent;
		base.TimeAndSpanController.SelectionToTimeChanged += timeAndSpanController_SelectionToTimeChangedEvent;
		base.TimeAndSpanController.SelectionCenterTimeChanged += timeAndSpanController_SelectionCenterTimeChangedEvent;
		base.TimeAndSpanController.SelectionFromTimeChanged += timeAndSpanController_SelectionFromTimeChangedEvent;
		base.AppearanceAndBehaviourController.PropertyChanged += appearanceAndBehaviourController_PropertyChanged;
		ThemeManager.Instance.SelectedThemeChanged += ThemeManager_SelectedThemeChanged;
		UpdateTheme();
	}

	public override void Close()
	{
		base.Close();
		base.TimeAndSpanController.TimelinePixelTimesChanged -= timeAndSpanController_TimelinePixelTimesChangedEvent;
		base.TimeAndSpanController.SelectionModeChangedEvent -= timeAndSpanController_SelectionModeChangedEvent;
		base.TimeAndSpanController.SelectionStateChangedEvent -= timeAndSpanController_SelectionStateChangedEvent;
		base.TimeAndSpanController.SelectionActiveMarkerChangedEvent -= timeAndSpanController_SelectionActiveMarkerChangedEvent;
		base.TimeAndSpanController.SelectionToTimeChanged -= timeAndSpanController_SelectionToTimeChangedEvent;
		base.TimeAndSpanController.SelectionCenterTimeChanged -= timeAndSpanController_SelectionCenterTimeChangedEvent;
		base.TimeAndSpanController.SelectionFromTimeChanged -= timeAndSpanController_SelectionFromTimeChangedEvent;
		base.AppearanceAndBehaviourController.PropertyChanged -= appearanceAndBehaviourController_PropertyChanged;
		ThemeManager.Instance.SelectedThemeChanged -= ThemeManager_SelectedThemeChanged;
	}

	private void appearanceAndBehaviourController_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == AppearanceAndBehaviourController.PropertyNames.UserSelectionControlEnabled.ToString())
		{
			Update();
		}
		else if (e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineTopLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineBottomLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineCustomDataLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineTopLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineBottomLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineCustomDataLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.TimelineAreaDisplayMode.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSourceVisualAppearanceMarkerImage.ToString())
		{
			UpdateSelectionMarkerOffsets();
		}
	}

	private void ThemeManager_SelectedThemeChanged(object sender, EventArgs e)
	{
		UpdateTheme();
	}

	private void LayerBase_Loaded(object sender, RoutedEventArgs e)
	{
		Update();
		UpdateSelectionMarkerOffsets();
	}

	private void _selectionMarker_MarkerMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		_draggedSelectionMarker = null;
		if (!(sender is SelectionMarker selectionMarker))
		{
			return;
		}
		if (e.ClickCount == 2)
		{
			if (selectionMarker == _fromSelectionMarker)
			{
				base.TimeAndSpanController.SetTimelineTime(base.TimeAndSpanController.SelectionFromTime, animate: true);
			}
			else if (selectionMarker == _centerSelectionMarker)
			{
				base.TimeAndSpanController.SetTimelineTime(base.TimeAndSpanController.SelectionCenterTime, animate: true);
			}
			else
			{
				base.TimeAndSpanController.SetTimelineTime(base.TimeAndSpanController.SelectionToTime, animate: true);
			}
			return;
		}
		_draggedSelectionMarker = selectionMarker;
		_selectionMarkerMouseLeftButtonDownPosition = e.GetPosition(this).X;
		_draggedSelectionMarker.MarkerCaptureMouse();
		if (_draggedSelectionMarker == _fromSelectionMarker)
		{
			base.TimeAndSpanController.SelectionActiveMarker = VideoOS.UI.Common.WPF.Timeline.SelectionMarker.From;
		}
		else if (_draggedSelectionMarker == _centerSelectionMarker)
		{
			base.TimeAndSpanController.SelectionActiveMarker = VideoOS.UI.Common.WPF.Timeline.SelectionMarker.Center;
		}
		else
		{
			base.TimeAndSpanController.SelectionActiveMarker = VideoOS.UI.Common.WPF.Timeline.SelectionMarker.To;
		}
	}

	private void _selectionMarker_MarkerMouseMove(object sender, MouseEventArgs e)
	{
		double x = e.GetPosition(this).X;
		if (_draggedSelectionMarker == null || _selectionMarkerMouseLeftButtonDownPosition == x)
		{
			return;
		}
		_selectionMarkerMouseLeftButtonDownPosition = -1.0;
		bool flag = false;
		DateTime dateTime = base.TimeAndSpanController.SelectionFromTime;
		DateTime dateTime2 = base.TimeAndSpanController.SelectionCenterTime;
		DateTime dateTime3 = base.TimeAndSpanController.SelectionToTime;
		int pixelOffset = (int)Math.Min(Math.Max(x, MARKER_MIN_MARGIN), base.ActualWidth - 1.0 - (double)MARKER_MIN_MARGIN);
		if (_draggedSelectionMarker == _fromSelectionMarker)
		{
			dateTime = base.TimeAndSpanController.GetPixelTime(pixelOffset);
			if (dateTime < base.TimeAndSpanController.GetPixelTime(MARKER_SCROLL_AREA_WIDTH) || dateTime > base.TimeAndSpanController.GetPixelTime((int)base.ActualWidth - 1 - MARKER_SCROLL_AREA_WIDTH))
			{
				flag = true;
			}
			if (Math.Abs(dateTime.Ticks - base.TimeAndSpanController.TimelineTime.Ticks) < base.TimeAndSpanController.GetTicksPerPixel() * MARKER_SNAP_MARGIN)
			{
				dateTime = base.TimeAndSpanController.TimelineTime;
			}
			if (base.TimeAndSpanController.SelectionMode == SelectionMode.FromCenterTo)
			{
				if (dateTime > dateTime2)
				{
					dateTime = dateTime2;
					flag = false;
				}
			}
			else
			{
				if (dateTime > dateTime3)
				{
					dateTime = dateTime3;
					flag = false;
				}
				if (dateTime > dateTime2)
				{
					dateTime2 = dateTime;
				}
			}
		}
		else if (_draggedSelectionMarker == _centerSelectionMarker)
		{
			dateTime2 = base.TimeAndSpanController.GetPixelTime(pixelOffset);
			if (dateTime2 < base.TimeAndSpanController.GetPixelTime(MARKER_SCROLL_AREA_WIDTH) || dateTime2 > base.TimeAndSpanController.GetPixelTime((int)base.ActualWidth - 1 - MARKER_SCROLL_AREA_WIDTH))
			{
				flag = true;
			}
			if (Math.Abs(dateTime2.Ticks - base.TimeAndSpanController.TimelineTime.Ticks) < base.TimeAndSpanController.GetTicksPerPixel() * MARKER_SNAP_MARGIN)
			{
				dateTime2 = base.TimeAndSpanController.TimelineTime;
			}
			if (dateTime2 < dateTime)
			{
				dateTime2 = dateTime;
				flag = false;
			}
			if (dateTime2 > dateTime3)
			{
				dateTime2 = dateTime3;
				flag = false;
			}
		}
		else
		{
			dateTime3 = base.TimeAndSpanController.GetPixelTime(pixelOffset);
			if (dateTime3 < base.TimeAndSpanController.GetPixelTime(MARKER_SCROLL_AREA_WIDTH) || dateTime3 > base.TimeAndSpanController.GetPixelTime((int)base.ActualWidth - 1 - MARKER_SCROLL_AREA_WIDTH))
			{
				flag = true;
			}
			if (Math.Abs(dateTime3.Ticks - base.TimeAndSpanController.TimelineTime.Ticks) < base.TimeAndSpanController.GetTicksPerPixel() * MARKER_SNAP_MARGIN)
			{
				dateTime3 = base.TimeAndSpanController.TimelineTime;
			}
			if (base.TimeAndSpanController.SelectionMode == SelectionMode.FromCenterTo)
			{
				if (dateTime3 < dateTime2)
				{
					dateTime3 = dateTime2;
					flag = false;
				}
			}
			else
			{
				if (dateTime3 < dateTime)
				{
					dateTime3 = dateTime;
					flag = false;
				}
				if (dateTime3 < dateTime2)
				{
					dateTime2 = dateTime3;
				}
			}
		}
		base.TimeAndSpanController.SetSelectionTimes(dateTime, dateTime2, dateTime3, adjustToFromTimeIfNessesary: false, adjustToCenterTimeIfNessesary: false, adjustToToTimeIfNessesary: false);
		if (flag)
		{
			double num = ((!(x < (double)MARKER_SCROLL_AREA_WIDTH)) ? ((double)MARKER_SCROLL_AREA_WIDTH - (base.ActualWidth - 1.0 - x)) : (x - (double)MARKER_SCROLL_AREA_WIDTH));
			num /= 3.0;
			num = Math.Min(num, 20.0);
			num = Math.Max(num, -20.0);
			long moveTicks = (long)((double)base.TimeAndSpanController.GetTicksPerPixel() * num);
			StartTimelineMoveTimer(moveTicks);
		}
		else
		{
			StopTimelineMoveTimer();
		}
	}

	private void _selectionMarker_MarkerMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (_draggedSelectionMarker != null)
		{
			_draggedSelectionMarker.MarkerReleaseMouseCapture();
		}
		_draggedSelectionMarker = null;
		base.TimeAndSpanController.SelectionActiveMarker = VideoOS.UI.Common.WPF.Timeline.SelectionMarker.None;
		_selectionMarkerMouseLeftButtonDownPosition = -1.0;
		StopTimelineMoveTimer();
	}

	private void _timelineMoveTimer_Tick(object sender, EventArgs e)
	{
		Trace.WriteLine("_timelineMoveTimer_Tick");
		long value = (long)_timelineMoveTimer.Tag;
		DateTime time = base.TimeAndSpanController.TimelineTime.AddTicks(value);
		int pixelOffset = base.TimeAndSpanController.GetPixelOffset(base.TimeAndSpanController.SelectionFromTime);
		int pixelOffset2 = base.TimeAndSpanController.GetPixelOffset(base.TimeAndSpanController.SelectionCenterTime);
		int pixelOffset3 = base.TimeAndSpanController.GetPixelOffset(base.TimeAndSpanController.SelectionToTime);
		base.TimeAndSpanController.SetTimelineTime(time, animate: false);
		DateTime dateTime = base.TimeAndSpanController.SelectionFromTime;
		DateTime dateTime2 = base.TimeAndSpanController.SelectionCenterTime;
		DateTime dateTime3 = base.TimeAndSpanController.SelectionToTime;
		if (_draggedSelectionMarker == _fromSelectionMarker)
		{
			dateTime = base.TimeAndSpanController.GetPixelTime(pixelOffset);
			if (base.TimeAndSpanController.SelectionMode == SelectionMode.FromCenterTo)
			{
				if (dateTime > dateTime2)
				{
					dateTime = dateTime2;
					StopTimelineMoveTimer();
				}
			}
			else
			{
				if (dateTime > dateTime3)
				{
					dateTime = dateTime3;
					StopTimelineMoveTimer();
				}
				if (dateTime > dateTime2)
				{
					dateTime2 = dateTime;
				}
			}
		}
		else if (_draggedSelectionMarker == _centerSelectionMarker)
		{
			dateTime2 = base.TimeAndSpanController.GetPixelTime(pixelOffset2);
			if (dateTime2 < dateTime)
			{
				dateTime2 = dateTime;
				StopTimelineMoveTimer();
			}
			if (dateTime2 > dateTime3)
			{
				dateTime2 = dateTime3;
				StopTimelineMoveTimer();
			}
		}
		else
		{
			dateTime3 = base.TimeAndSpanController.GetPixelTime(pixelOffset3);
			if (base.TimeAndSpanController.SelectionMode == SelectionMode.FromCenterTo)
			{
				if (dateTime3 < dateTime2)
				{
					dateTime3 = dateTime2;
					StopTimelineMoveTimer();
				}
			}
			else
			{
				if (dateTime3 < dateTime)
				{
					dateTime3 = dateTime;
					StopTimelineMoveTimer();
				}
				if (dateTime3 < dateTime2)
				{
					dateTime2 = dateTime3;
				}
			}
		}
		base.TimeAndSpanController.SetSelectionTimes(dateTime, dateTime2, dateTime3, adjustToFromTimeIfNessesary: false, adjustToCenterTimeIfNessesary: false, adjustToToTimeIfNessesary: false);
	}

	private void StartTimelineMoveTimer(long moveTicks)
	{
		_timelineMoveTimer.Tag = moveTicks;
		if (!_timelineMoveTimer.IsEnabled)
		{
			_timelineMoveTimer.Start();
		}
	}

	private void StopTimelineMoveTimer()
	{
		_timelineMoveTimer.Stop();
	}

	private void timeAndSpanController_TimelinePixelTimesChangedEvent(object sender, EventArgs e)
	{
		if (base.TimeAndSpanController.SelectionState == SelectionState.Enabled && base.TimeAndSpanController.SelectionActiveMarker != VideoOS.UI.Common.WPF.Timeline.SelectionMarker.None && _draggedSelectionMarker == null)
		{
			base.TimeAndSpanController.SelectionActiveMarker = VideoOS.UI.Common.WPF.Timeline.SelectionMarker.None;
		}
		Update();
	}

	private void timeAndSpanController_SelectionModeChangedEvent(object sender, EventArgs e)
	{
		Update();
	}

	private void timeAndSpanController_SelectionStateChangedEvent(object sender, EventArgs e)
	{
		Update();
	}

	private void timeAndSpanController_SelectionActiveMarkerChangedEvent(object sender, SelectionActiveMarkerChangeEventArgs e)
	{
		Update();
	}

	private void timeAndSpanController_SelectionToTimeChangedEvent(object sender, DateTimeEventArgs e)
	{
		Update();
	}

	private void timeAndSpanController_SelectionCenterTimeChangedEvent(object sender, DateTimeEventArgs e)
	{
		Update();
	}

	private void timeAndSpanController_SelectionFromTimeChangedEvent(object sender, DateTimeEventArgs e)
	{
		Update();
	}

	private void _fromSelectionMarkerLeftOutOfScopeButton_Click(object sender, EventArgs e)
	{
		base.TimeAndSpanController.SetTimelineTime(base.TimeAndSpanController.SelectionFromTime, animate: true);
		FireSelectionOutOfScopeMarkerClicked();
	}

	private void _centerSelectionMarkerLeftOutOfScopeButton_Click(object sender, EventArgs e)
	{
		base.TimeAndSpanController.SetTimelineTime(base.TimeAndSpanController.SelectionCenterTime, animate: true);
		FireSelectionOutOfScopeMarkerClicked();
	}

	private void _toSelectionMarkerLeftOutOfScopeButton_Click(object sender, EventArgs e)
	{
		base.TimeAndSpanController.SetTimelineTime(base.TimeAndSpanController.SelectionToTime, animate: true);
		FireSelectionOutOfScopeMarkerClicked();
	}

	private void _fromSelectionMarkerRightOutOfScopeButton_Click(object sender, EventArgs e)
	{
		base.TimeAndSpanController.SetTimelineTime(base.TimeAndSpanController.SelectionFromTime, animate: true);
		FireSelectionOutOfScopeMarkerClicked();
	}

	private void _centerSelectionMarkerRightOutOfScopeButton_Click(object sender, EventArgs e)
	{
		base.TimeAndSpanController.SetTimelineTime(base.TimeAndSpanController.SelectionCenterTime, animate: true);
		FireSelectionOutOfScopeMarkerClicked();
	}

	private void _toSelectionMarkerRightOutOfScopeButton_Click(object sender, EventArgs e)
	{
		base.TimeAndSpanController.SetTimelineTime(base.TimeAndSpanController.SelectionToTime, animate: true);
		FireSelectionOutOfScopeMarkerClicked();
	}

	private void _fromSelectionMarker_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (this.SelectionFromMarkerRightClicked != null)
		{
			this.SelectionFromMarkerRightClicked(this, new EventArgs());
		}
	}

	private void _centerSelectionMarker_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (this.SelectionCenterMarkerRightClicked != null)
		{
			this.SelectionCenterMarkerRightClicked(this, new EventArgs());
		}
	}

	private void _toSelectionMarker_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (this.SelectionToMarkerRightClicked != null)
		{
			this.SelectionToMarkerRightClicked(this, new EventArgs());
		}
	}

	private void FireSelectionOutOfScopeMarkerClicked()
	{
		if (this.SelectionOutOfScopeMarkerClicked != null)
		{
			this.SelectionOutOfScopeMarkerClicked(this, new EventArgs());
		}
	}

	private void UpdateSelectionMarkerOffsets()
	{
		int textAreaHeight;
		if (base.AppearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Narrow)
		{
			textAreaHeight = 20;
			_outOfScopeMarkersCanvas.Margin = new Thickness(3.0, 3.0, 3.0, 3.0);
		}
		else
		{
			textAreaHeight = 24;
			_outOfScopeMarkersCanvas.Margin = new Thickness(3.0, 9.0, 3.0, 3.0);
		}
		_fromSelectionMarker.UpdateOffsets(textAreaHeight, base.AppearanceAndBehaviourController.TimelineAreaTracksSectionHeight, base.AppearanceAndBehaviourController.TimelineAreaTracksMargin, base.AppearanceAndBehaviourController.SequenceSourceVisualAppearanceMarkerImage);
		_centerSelectionMarker.UpdateOffsets(textAreaHeight, base.AppearanceAndBehaviourController.TimelineAreaTracksSectionHeight, base.AppearanceAndBehaviourController.TimelineAreaTracksMargin, base.AppearanceAndBehaviourController.SequenceSourceVisualAppearanceMarkerImage);
		_toSelectionMarker.UpdateOffsets(textAreaHeight, base.AppearanceAndBehaviourController.TimelineAreaTracksSectionHeight, base.AppearanceAndBehaviourController.TimelineAreaTracksMargin, base.AppearanceAndBehaviourController.SequenceSourceVisualAppearanceMarkerImage);
		Canvas.SetBottom(_selectionAreaTopLine, base.AppearanceAndBehaviourController.TimelineAreaTracksSectionHeight - 1);
		Canvas.SetTop(_selectionInfo, base.AppearanceAndBehaviourController.TimelineAreaRulerSectionHeight);
	}

	private void Update()
	{
		if (base.TimeAndSpanController.SelectionState == SelectionState.Enabled)
		{
			_fromSelectionMarker.Selected = false;
			_centerSelectionMarker.Selected = false;
			_toSelectionMarker.Selected = false;
			_selectionAreaTopLine.Visibility = Visibility.Visible;
			_selectionAreaBottomLine.Visibility = Visibility.Visible;
			int pixelOffset = base.TimeAndSpanController.GetPixelOffset(base.TimeAndSpanController.SelectionFromTime);
			int pixelOffset2 = base.TimeAndSpanController.GetPixelOffset(base.TimeAndSpanController.SelectionCenterTime);
			int pixelOffset3 = base.TimeAndSpanController.GetPixelOffset(base.TimeAndSpanController.SelectionToTime);
			if (base.AppearanceAndBehaviourController.SelectionInfoAvailable && (base.TimeAndSpanController.SelectionActiveMarker == VideoOS.UI.Common.WPF.Timeline.SelectionMarker.From || base.TimeAndSpanController.SelectionActiveMarker == VideoOS.UI.Common.WPF.Timeline.SelectionMarker.To))
			{
				_selectionInfo.Visibility = Visibility.Visible;
				_selectionInfo.Model.StartTime = DateTimeFormatter.ConvertAndFormat("T", base.AppearanceAndBehaviourController.TimeZone, base.TimeAndSpanController.SelectionFromTime, injectMilliseconds: false, injectOffset: false);
				_selectionInfo.Model.EndTime = DateTimeFormatter.ConvertAndFormat("T", base.AppearanceAndBehaviourController.TimeZone, base.TimeAndSpanController.SelectionToTime, injectMilliseconds: false, injectOffset: false);
				TimeSpan timeSpan = base.TimeAndSpanController.SelectionToTime - base.TimeAndSpanController.SelectionFromTime;
				string text = ((int)timeSpan.TotalHours).ToString();
				text += CultureInfo.CurrentCulture.DateTimeFormat.TimeSeparator;
				text += timeSpan.Minutes.ToString("00");
				text += CultureInfo.CurrentCulture.DateTimeFormat.TimeSeparator;
				text += timeSpan.Seconds.ToString("00");
				_selectionInfo.Model.Duration = text;
			}
			else
			{
				_selectionInfo.Visibility = Visibility.Hidden;
			}
			if (base.TimeAndSpanController.SelectionActiveMarker == VideoOS.UI.Common.WPF.Timeline.SelectionMarker.From)
			{
				_fromSelectionMarker.Selected = true;
				_fromSelectionMarker.Time = ((base.AppearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Full) ? (DateTimeFormatter.ConvertAndFormat("G", base.AppearanceAndBehaviourController.TimeZone, base.TimeAndSpanController.SelectionFromTime, injectMilliseconds: true, injectOffset: false) ?? "") : (DateTimeFormatter.ConvertAndFormat("T", base.AppearanceAndBehaviourController.TimeZone, base.TimeAndSpanController.SelectionFromTime, injectMilliseconds: true, injectOffset: false) ?? ""));
				_selectionInfo.ClearValue(Canvas.LeftProperty);
				Canvas.SetRight(_selectionInfo, base.ActualWidth - (double)pixelOffset + 15.0);
			}
			else if (base.TimeAndSpanController.SelectionActiveMarker == VideoOS.UI.Common.WPF.Timeline.SelectionMarker.Center)
			{
				_centerSelectionMarker.Selected = true;
				_centerSelectionMarker.Time = ((base.AppearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Full) ? (DateTimeFormatter.ConvertAndFormat("G", base.AppearanceAndBehaviourController.TimeZone, base.TimeAndSpanController.SelectionCenterTime, injectMilliseconds: true, injectOffset: false) ?? "") : (DateTimeFormatter.ConvertAndFormat("T", base.AppearanceAndBehaviourController.TimeZone, base.TimeAndSpanController.SelectionCenterTime, injectMilliseconds: true, injectOffset: false) ?? ""));
			}
			else if (base.TimeAndSpanController.SelectionActiveMarker == VideoOS.UI.Common.WPF.Timeline.SelectionMarker.To)
			{
				_toSelectionMarker.Selected = true;
				_toSelectionMarker.Time = ((base.AppearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Full) ? (DateTimeFormatter.ConvertAndFormat("G", base.AppearanceAndBehaviourController.TimeZone, base.TimeAndSpanController.SelectionToTime, injectMilliseconds: true, injectOffset: false) ?? "") : (DateTimeFormatter.ConvertAndFormat("T", base.AppearanceAndBehaviourController.TimeZone, base.TimeAndSpanController.SelectionToTime, injectMilliseconds: true, injectOffset: false) ?? ""));
				_selectionInfo.ClearValue(Canvas.RightProperty);
				Canvas.SetLeft(_selectionInfo, pixelOffset3 + 15);
			}
			if (pixelOffset < 0)
			{
				_fromSelectionMarker.Visibility = Visibility.Hidden;
				_fromSelectionMarkerLeftOutOfScopeButton.Visibility = Visibility.Visible;
			}
			else
			{
				_fromSelectionMarker.Visibility = Visibility.Visible;
				_fromSelectionMarkerLeftOutOfScopeButton.Visibility = Visibility.Collapsed;
			}
			if (base.TimeAndSpanController.SelectionMode == SelectionMode.FromCenterTo)
			{
				if (pixelOffset2 < 0)
				{
					_centerSelectionMarker.Visibility = Visibility.Hidden;
					_centerSelectionMarkerLeftOutOfScopeButton.Visibility = Visibility.Visible;
					_centerSelectionMarkerRightOutOfScopeButton.Visibility = Visibility.Collapsed;
				}
				else if ((double)pixelOffset2 > base.ActualWidth - 1.0)
				{
					_centerSelectionMarker.Visibility = Visibility.Hidden;
					_centerSelectionMarkerLeftOutOfScopeButton.Visibility = Visibility.Collapsed;
					_centerSelectionMarkerRightOutOfScopeButton.Visibility = Visibility.Visible;
				}
				else
				{
					_centerSelectionMarker.Visibility = Visibility.Visible;
					_centerSelectionMarkerLeftOutOfScopeButton.Visibility = Visibility.Collapsed;
					_centerSelectionMarkerRightOutOfScopeButton.Visibility = Visibility.Collapsed;
				}
			}
			else
			{
				_centerSelectionMarker.Visibility = Visibility.Hidden;
				_centerSelectionMarkerLeftOutOfScopeButton.Visibility = Visibility.Collapsed;
				_centerSelectionMarkerRightOutOfScopeButton.Visibility = Visibility.Collapsed;
			}
			if (pixelOffset3 < 0)
			{
				_toSelectionMarker.Visibility = Visibility.Hidden;
				_toSelectionMarkerLeftOutOfScopeButton.Visibility = Visibility.Visible;
			}
			else
			{
				_toSelectionMarker.Visibility = Visibility.Visible;
				_toSelectionMarkerLeftOutOfScopeButton.Visibility = Visibility.Collapsed;
			}
			if ((double)pixelOffset > base.ActualWidth - 1.0)
			{
				_fromSelectionMarker.Visibility = Visibility.Hidden;
				_fromSelectionMarkerRightOutOfScopeButton.Visibility = Visibility.Visible;
			}
			else
			{
				_fromSelectionMarker.Visibility = Visibility.Visible;
				_fromSelectionMarkerRightOutOfScopeButton.Visibility = Visibility.Collapsed;
			}
			if ((double)pixelOffset3 > base.ActualWidth - 1.0)
			{
				_toSelectionMarker.Visibility = Visibility.Hidden;
				_toSelectionMarkerRightOutOfScopeButton.Visibility = Visibility.Visible;
			}
			else
			{
				_toSelectionMarker.Visibility = Visibility.Visible;
				_toSelectionMarkerRightOutOfScopeButton.Visibility = Visibility.Collapsed;
			}
			_fromSelectionMarker.LeftBounds = -pixelOffset;
			_centerSelectionMarker.LeftBounds = -pixelOffset2;
			_toSelectionMarker.LeftBounds = -pixelOffset3;
			_fromSelectionMarker.RightBounds = base.ActualWidth - (double)pixelOffset;
			_centerSelectionMarker.RightBounds = base.ActualWidth - (double)pixelOffset2;
			_toSelectionMarker.RightBounds = base.ActualWidth - (double)pixelOffset3;
			Canvas.SetLeft(_fromSelectionMarker, pixelOffset);
			Canvas.SetLeft(_centerSelectionMarker, pixelOffset2);
			Canvas.SetLeft(_toSelectionMarker, pixelOffset3);
			Canvas.SetLeft(_selectionAreaTopLine, pixelOffset);
			_selectionAreaTopLine.Width = pixelOffset3 - pixelOffset + 1;
			Canvas.SetLeft(_selectionAreaBottomLine, pixelOffset);
			_selectionAreaBottomLine.Width = pixelOffset3 - pixelOffset + 1;
			_toSelectionMarker.IsEnabled = base.AppearanceAndBehaviourController.UserSelectionMarkerTimeChangeEnabled;
			_centerSelectionMarker.IsEnabled = base.AppearanceAndBehaviourController.UserSelectionMarkerTimeChangeEnabled;
			_fromSelectionMarker.IsEnabled = base.AppearanceAndBehaviourController.UserSelectionMarkerTimeChangeEnabled;
		}
		else
		{
			_selectionAreaTopLine.Visibility = Visibility.Hidden;
			_selectionAreaBottomLine.Visibility = Visibility.Hidden;
			_fromSelectionMarker.Visibility = Visibility.Hidden;
			_centerSelectionMarker.Visibility = Visibility.Hidden;
			_toSelectionMarker.Visibility = Visibility.Hidden;
			_fromSelectionMarkerLeftOutOfScopeButton.Visibility = Visibility.Collapsed;
			_fromSelectionMarkerRightOutOfScopeButton.Visibility = Visibility.Collapsed;
			_centerSelectionMarkerLeftOutOfScopeButton.Visibility = Visibility.Collapsed;
			_centerSelectionMarkerRightOutOfScopeButton.Visibility = Visibility.Collapsed;
			_toSelectionMarkerLeftOutOfScopeButton.Visibility = Visibility.Collapsed;
			_toSelectionMarkerRightOutOfScopeButton.Visibility = Visibility.Collapsed;
			_selectionInfo.Visibility = Visibility.Hidden;
		}
	}

	private void UpdateTheme()
	{
		_fromSelectionMarker.ApplyTheme((ThemeManager.Instance.SelectedTheme == ThemeType.Dark) ? ThemeType.Light : ThemeType.Dark);
		_centerSelectionMarker.ApplyTheme((ThemeManager.Instance.SelectedTheme == ThemeType.Dark) ? ThemeType.Light : ThemeType.Dark);
		_toSelectionMarker.ApplyTheme((ThemeManager.Instance.SelectedTheme == ThemeType.Dark) ? ThemeType.Light : ThemeType.Dark);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
