using System;
using System.Windows;
using VideoOS.UI.Common.WPF.Timeline.Control.Controllers;

namespace VideoOS.UI.Common.WPF.Timeline.Control.UIControls.SelectTimePopup;

public class TimelineSelectTimePopupHelper
{
	private TimelineSelectTimePopupHelper()
	{
	}

	public static void OpenAndHandleSelectTimelineTime(TimeAndSpanController timeAndSpanController, AppearanceAndBehaviourController appearanceAndBehaviourController, UIElement parent)
	{
		TimelineSelectTimePopup timelineSelectTimePopup = new TimelineSelectTimePopup(timeAndSpanController, appearanceAndBehaviourController, parent);
		ApplyStyles(timelineSelectTimePopup, appearanceAndBehaviourController.TimelineStyles);
		timelineSelectTimePopup.TimesChangedEvent += selectTimelineTimePopup_TimesChangedEvent;
		timelineSelectTimePopup.OpenSelectTimeMode(timeAndSpanController.TimelineTime);
	}

	public static void OpenAndHandleSelectSelectionTimes(TimeAndSpanController timeAndSpanController, AppearanceAndBehaviourController appearanceAndBehaviourController, UIElement parent)
	{
		DateTime fromTime;
		DateTime centerTime;
		DateTime toTime;
		if (timeAndSpanController.SelectionState == SelectionState.Disabled)
		{
			fromTime = timeAndSpanController.DefaultSelectionFromTime;
			centerTime = timeAndSpanController.DefaultSelectionCenterTime;
			toTime = timeAndSpanController.DefaultSelectionToTime;
		}
		else
		{
			fromTime = timeAndSpanController.SelectionFromTime;
			centerTime = timeAndSpanController.SelectionCenterTime;
			toTime = timeAndSpanController.SelectionToTime;
		}
		TimelineSelectTimePopup timelineSelectTimePopup = new TimelineSelectTimePopup(timeAndSpanController, appearanceAndBehaviourController, parent);
		ApplyStyles(timelineSelectTimePopup, appearanceAndBehaviourController.TimelineStyles);
		timelineSelectTimePopup.TimesChangedEvent += selectSelectionTimesPopup_TimesChangedEvent;
		if (timeAndSpanController.SelectionMode == SelectionMode.FromCenterTo)
		{
			timelineSelectTimePopup.OpenSelectFromCenterToTimeMode(fromTime, centerTime, toTime);
		}
		else
		{
			timelineSelectTimePopup.OpenSelectFromToTimeMode(fromTime, toTime);
		}
	}

	private static void selectTimelineTimePopup_TimesChangedEvent(object sender, EventArgs e)
	{
		TimelineSelectTimePopup timelineSelectTimePopup = (TimelineSelectTimePopup)sender;
		timelineSelectTimePopup.AppearanceAndBehaviourController.PlaybackState = PlaybackState.Stopped;
		timelineSelectTimePopup.TimeAndSpanController.SetTimelineTime(timelineSelectTimePopup.Time, animate: true);
	}

	private static void selectSelectionTimesPopup_TimesChangedEvent(object sender, EventArgs e)
	{
		TimelineSelectTimePopup timelineSelectTimePopup = (TimelineSelectTimePopup)sender;
		TimeAndSpanController timeAndSpanController = timelineSelectTimePopup.TimeAndSpanController;
		AppearanceAndBehaviourController appearanceAndBehaviourController = timelineSelectTimePopup.AppearanceAndBehaviourController;
		DateTime selectionFromTime = timeAndSpanController.SelectionFromTime;
		DateTime centerTime = timeAndSpanController.SelectionCenterTime;
		DateTime selectionToTime = timeAndSpanController.SelectionToTime;
		if (timeAndSpanController.SelectionMode == SelectionMode.FromCenterTo)
		{
			selectionFromTime = timelineSelectTimePopup.FromTime;
			centerTime = timelineSelectTimePopup.CenterTime;
			selectionToTime = timelineSelectTimePopup.ToTime;
		}
		else
		{
			selectionFromTime = timelineSelectTimePopup.FromTime;
			selectionToTime = timelineSelectTimePopup.ToTime;
		}
		timeAndSpanController.SetSelectionTimes(selectionFromTime, centerTime, selectionToTime, adjustToFromTimeIfNessesary: true, adjustToCenterTimeIfNessesary: false, adjustToToTimeIfNessesary: false);
		timeAndSpanController.SelectionState = SelectionState.Enabled;
		timeAndSpanController.SelectionFitToTimeline(appearanceAndBehaviourController.TimelineSpans, decreaseTimelineSpan: false, animate: false);
	}

	private static void ApplyStyles(TimelineSelectTimePopup timelineSelectTimePopup, ITimelineStyles timelineStyles)
	{
		object obj = timelineStyles.TryFindResource("TimelineCalendarStyle");
		if (obj != null)
		{
			timelineSelectTimePopup.TimelineSelectTimeUserControl.Resources.Add("TimelineCalendarStyle", obj);
		}
		obj = timelineStyles.TryFindResource("TimelineTimePickerStyle");
		if (obj != null)
		{
			timelineSelectTimePopup.TimelineSelectTimeUserControl.Resources.Add("TimelineTimePickerStyle", obj);
		}
	}
}
