using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using VideoOS.UI.Common.WPF.Timeline.Control.Controllers;
using VideoOS.UI.Common.WPF.Timeline.Control.UIControls.SelectTimePopup.resources;

namespace VideoOS.UI.Common.WPF.Timeline.Control.UIControls.SelectTimePopup;

public class TimelineSelectTimePopup
{
	private TimelineSelectTimeUserControl _timelineSelectTimeUserControl = new TimelineSelectTimeUserControl();

	private Popup _popup = new Popup();

	private AppearanceAndBehaviourController _appearanceAndBehaviourController;

	private TimeAndSpanController _timeAndSpanController;

	private static DateTime _timePopupLastCloseTime = DateTime.MinValue;

	public TimeAndSpanController TimeAndSpanController => _timeAndSpanController;

	public AppearanceAndBehaviourController AppearanceAndBehaviourController => _appearanceAndBehaviourController;

	public TimelineSelectTimeUserControl TimelineSelectTimeUserControl => _timelineSelectTimeUserControl;

	public DateTime Time => _timelineSelectTimeUserControl.SelectTime1Time;

	public DateTime FromTime => _timelineSelectTimeUserControl.SelectTime1Time;

	public DateTime CenterTime => _timelineSelectTimeUserControl.SelectTime2Time;

	public DateTime ToTime => _timelineSelectTimeUserControl.SelectTime3Time;

	public bool IsOpen => _popup.IsOpen;

	public object Tag { get; set; }

	public event EventHandler TimesChangedEvent;

	public TimelineSelectTimePopup(TimeAndSpanController timeAndSpanController, AppearanceAndBehaviourController appearanceAndBehaviourController, UIElement parent)
	{
		_popup.Closed += _popup_Closed;
		_timeAndSpanController = timeAndSpanController;
		_appearanceAndBehaviourController = appearanceAndBehaviourController;
		_popup.AllowsTransparency = true;
		_popup.StaysOpen = false;
		_popup.PopupAnimation = PopupAnimation.None;
		_popup.Placement = PlacementMode.MousePoint;
		_popup.PlacementTarget = parent;
		_popup.Child = _timelineSelectTimeUserControl;
		_timelineSelectTimeUserControl.OKButtonClickEvent += _timelineSelectTimeUserControl_OKButtonClickEvent;
		_timelineSelectTimeUserControl.CloseButtonClickEvent += _timelineSelectTimeUserControl_CloseButtonClickEvent;
		_timelineSelectTimeUserControl.TimeZone = _appearanceAndBehaviourController.TimeZone;
	}

	private void _popup_Closed(object sender, EventArgs e)
	{
		_timePopupLastCloseTime = DateTime.Now;
	}

	public void OpenSelectTimeMode(DateTime time)
	{
		if (!(DateTime.Now - _timePopupLastCloseTime < TimeSpan.FromMilliseconds(200.0)))
		{
			_timelineSelectTimeUserControl.OKButtonLabel = Strings.GoToButtonLabel;
			_timelineSelectTimeUserControl.SelectTime1Visibility = Visibility.Visible;
			_timelineSelectTimeUserControl.SelectTime1Header = Strings.SelectTimeHeaderSelectTime;
			_timelineSelectTimeUserControl.SelectTime1Time = time;
			_timelineSelectTimeUserControl.SelectTime2Visibility = Visibility.Collapsed;
			_timelineSelectTimeUserControl.SelectTime3Visibility = Visibility.Collapsed;
			_popup.IsOpen = true;
		}
	}

	public void OpenSelectFromToTimeMode(DateTime fromTime, DateTime toTime)
	{
		if (!(DateTime.Now - _timePopupLastCloseTime < TimeSpan.FromMilliseconds(200.0)))
		{
			_timelineSelectTimeUserControl.OKButtonLabel = Strings.OKButtonLabel;
			_timelineSelectTimeUserControl.SelectTime1Visibility = Visibility.Visible;
			_timelineSelectTimeUserControl.SelectTime1Header = Strings.SelectTimeHeaderFromTime;
			_timelineSelectTimeUserControl.SelectTime1Time = fromTime;
			_timelineSelectTimeUserControl.SelectTime2Visibility = Visibility.Collapsed;
			_timelineSelectTimeUserControl.SelectTime3Visibility = Visibility.Visible;
			_timelineSelectTimeUserControl.SelectTime3Header = Strings.SelectTimeHeaderToTime;
			_timelineSelectTimeUserControl.SelectTime3Time = toTime;
			_popup.IsOpen = true;
		}
	}

	public void OpenSelectFromCenterToTimeMode(DateTime fromTime, DateTime centerTime, DateTime toTime)
	{
		_timelineSelectTimeUserControl.OKButtonLabel = Strings.OKButtonLabel;
		_timelineSelectTimeUserControl.SelectTime1Visibility = Visibility.Visible;
		_timelineSelectTimeUserControl.SelectTime1Header = Strings.SelectTimeHeaderFromTime;
		_timelineSelectTimeUserControl.SelectTime1Time = fromTime;
		_timelineSelectTimeUserControl.SelectTime2Visibility = Visibility.Visible;
		_timelineSelectTimeUserControl.SelectTime2Header = Strings.SelectTimeHeaderCenterTime;
		_timelineSelectTimeUserControl.SelectTime2Time = centerTime;
		_timelineSelectTimeUserControl.SelectTime3Visibility = Visibility.Visible;
		_timelineSelectTimeUserControl.SelectTime3Header = Strings.SelectTimeHeaderToTime;
		_timelineSelectTimeUserControl.SelectTime3Time = toTime;
		_popup.IsOpen = true;
	}

	private void _timelineSelectTimeUserControl_OKButtonClickEvent(object sender, EventArgs e)
	{
		if (this.TimesChangedEvent != null)
		{
			this.TimesChangedEvent(this, new EventArgs());
		}
		_popup.IsOpen = false;
	}

	private void _timelineSelectTimeUserControl_CloseButtonClickEvent(object sender, EventArgs e)
	{
		_popup.IsOpen = false;
	}
}
