using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using VideoOS.UI.Common.WPF.Timeline.Cache;
using VideoOS.UI.Common.WPF.Timeline.Control.Controllers;
using VideoOS.UI.Common.WPF.Timeline.Events;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.Timelines;

public partial class TimelinesLayer : LayerBase, IComponentConnector
{
	public TimelinesLayer()
	{
		InitializeComponent();
	}

	public override void Init(TimeAndSpanController timeAndSpanController, AppearanceAndBehaviourController appearanceAndBehaviourController, ITimelineSequenceCache timelineCache)
	{
		base.Init(timeAndSpanController, appearanceAndBehaviourController, timelineCache);
		_slicedTimeline.Init(timeAndSpanController, appearanceAndBehaviourController, timelineCache);
		base.TimeAndSpanController.SelectionStateChangedEvent += TimeAndSpanController_SelectionStateChangedEvent;
		base.TimeAndSpanController.SelectionFromTimeChanged += TimeAndSpanController_SelectionFromTimeChangedEvent;
		base.TimeAndSpanController.SelectionToTimeChanged += TimeAndSpanController_SelectionToTimeChangedEvent;
		base.TimeAndSpanController.TimelinePixelTimesChanged += TimeAndSpanController_TimelinePixelTimesChangedEvent;
		base.AppearanceAndBehaviourController.PropertyChanged += AppearanceAndBehaviourController_PropertyChanged;
	}

	public override void Close()
	{
		base.Close();
		_slicedTimeline.Close();
		base.TimeAndSpanController.SelectionStateChangedEvent -= TimeAndSpanController_SelectionStateChangedEvent;
		base.TimeAndSpanController.SelectionFromTimeChanged -= TimeAndSpanController_SelectionFromTimeChangedEvent;
		base.TimeAndSpanController.SelectionToTimeChanged -= TimeAndSpanController_SelectionToTimeChangedEvent;
		base.TimeAndSpanController.TimelinePixelTimesChanged -= TimeAndSpanController_TimelinePixelTimesChangedEvent;
		base.AppearanceAndBehaviourController.PropertyChanged -= AppearanceAndBehaviourController_PropertyChanged;
	}

	public bool IsPointOverUpperTimeline(Point point)
	{
		return _slicedTimeline.IsPointOverUpperTimeline(_slicedTimeline.PointFromScreen(PointToScreen(point)));
	}

	public bool IsPointOverLowerTimeline(Point point)
	{
		return _slicedTimeline.IsPointOverLowerTimeline(_slicedTimeline.PointFromScreen(PointToScreen(point)));
	}

	private void LayerBase_Loaded(object sender, RoutedEventArgs e)
	{
		UpdateIntervalSelectionRectangle();
		UpdateControlLocationsAndSizes();
		UpdateControlColors();
	}

	private void TimeAndSpanController_TimelinePixelTimesChangedEvent(object sender, EventArgs e)
	{
		UpdateIntervalSelectionRectangle();
	}

	private void TimeAndSpanController_SelectionToTimeChangedEvent(object sender, DateTimeEventArgs e)
	{
		UpdateIntervalSelectionRectangle();
	}

	private void TimeAndSpanController_SelectionFromTimeChangedEvent(object sender, DateTimeEventArgs e)
	{
		UpdateIntervalSelectionRectangle();
	}

	private void TimeAndSpanController_SelectionStateChangedEvent(object sender, EventArgs e)
	{
		UpdateIntervalSelectionRectangle();
	}

	private void AppearanceAndBehaviourController_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == AppearanceAndBehaviourController.PropertyNames.TimelineAreaDisplayMode.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.UpperTimelineLabel.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.LowerTimelineLabel.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineTopLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineBottomLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineCustomDataLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineTopLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineBottomLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineCustomDataLine.ToString())
		{
			UpdateControlLocationsAndSizes();
		}
		else if (e.PropertyName == AppearanceAndBehaviourController.PropertyNames.TimelineLineBackgroundBrush.ToString())
		{
			UpdateControlColors();
		}
	}

	private void UpdateIntervalSelectionRectangle()
	{
		if (base.TimeAndSpanController.SelectionState != SelectionState.Disabled)
		{
			_intervalSelectionLeftGrid.Visibility = Visibility.Visible;
			_intervalSelectionRightGrid.Visibility = Visibility.Visible;
			int pixelOffset = base.TimeAndSpanController.GetPixelOffset(base.TimeAndSpanController.SelectionFromTime);
			int pixelOffset2 = base.TimeAndSpanController.GetPixelOffset(base.TimeAndSpanController.SelectionToTime);
			_intervalSelectionLeftGrid.Width = Math.Max(pixelOffset, 0);
			Canvas.SetLeft(_intervalSelectionRightGrid, pixelOffset2);
			_intervalSelectionRightGrid.Width = Math.Max(base.ActualWidth - (double)pixelOffset2, 0.0);
		}
		else
		{
			_intervalSelectionLeftGrid.Visibility = Visibility.Hidden;
			_intervalSelectionRightGrid.Visibility = Visibility.Hidden;
		}
	}

	private void UpdateControlLocationsAndSizes()
	{
		bool flag = false;
		bool flag2 = false;
		if (base.AppearanceAndBehaviourController.ShowUpperTimelineTopLine || base.AppearanceAndBehaviourController.ShowUpperTimelineBottomLine || base.AppearanceAndBehaviourController.ShowUpperTimelineCustomDataLine)
		{
			flag = true;
		}
		if (base.AppearanceAndBehaviourController.ShowLowerTimelineTopLine || base.AppearanceAndBehaviourController.ShowLowerTimelineBottomLine || base.AppearanceAndBehaviourController.ShowLowerTimelineCustomDataLine)
		{
			flag2 = true;
		}
		int num = ((base.AppearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Full) ? 14 : 10);
		int num2 = ((base.AppearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Full) ? 6 : 4);
		_upperTimelineBackground.Visibility = ((!flag) ? Visibility.Collapsed : Visibility.Visible);
		_upperTimelineBackground.Height = num;
		Canvas.SetTop(_upperTimelineBackground, num2);
		_lowerTimelineBackground.Visibility = ((!flag2) ? Visibility.Collapsed : Visibility.Visible);
		_lowerTimelineBackground.Height = num;
		Canvas.SetTop(_lowerTimelineBackground, flag ? (num2 * 2 + num) : num2);
		_contentCanvas.Height = base.AppearanceAndBehaviourController.TimelineAreaTracksSectionHeight;
	}

	private void UpdateControlColors()
	{
		_upperTimelineBackground.Fill = base.AppearanceAndBehaviourController.TimelineLineBackgroundBrush;
		_lowerTimelineBackground.Fill = base.AppearanceAndBehaviourController.TimelineLineBackgroundBrush;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
