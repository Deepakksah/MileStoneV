using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using VideoOS.UI.Common.WPF.Timeline.Cache;
using VideoOS.UI.Common.WPF.Timeline.Control.Controllers;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.TimeMarkers;

public partial class TimeMarkersLayer : LayerBase, IComponentConnector
{
	private class MarkerInfo
	{
		public DateTime Time { get; set; }

		public MarkerState MarkerState { get; set; }

		public bool Narrow { get; set; }

		public string LegendFormatString { get; set; }
	}

	private Dictionary<DateTime, TimeMarker> _usedTimeMarkers = new Dictionary<DateTime, TimeMarker>();

	private Dictionary<DateTime, TimeMarker> _freeTimeMarkers = new Dictionary<DateTime, TimeMarker>();

	public TimeMarkersLayer()
	{
		InitializeComponent();
	}

	public override void Init(TimeAndSpanController timeAndSpanController, AppearanceAndBehaviourController appearanceAndBehaviourController, ITimelineSequenceCache timelineCache)
	{
		base.Init(timeAndSpanController, appearanceAndBehaviourController, timelineCache);
		base.TimeAndSpanController.TimelinePixelTimesChanged += timeAndSpanController_TimelinePixelTimesChangedEvent;
		base.AppearanceAndBehaviourController.PropertyChanged += AppearanceAndBehaviourController_PropertyChanged;
	}

	public override void Close()
	{
		base.Close();
		base.TimeAndSpanController.TimelinePixelTimesChanged -= timeAndSpanController_TimelinePixelTimesChangedEvent;
		base.AppearanceAndBehaviourController.PropertyChanged -= AppearanceAndBehaviourController_PropertyChanged;
	}

	private void AppearanceAndBehaviourController_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == AppearanceAndBehaviourController.PropertyNames.TimeZone.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.TimelineAreaDisplayMode.ToString())
		{
			Update();
		}
	}

	private void timeAndSpanController_TimelinePixelTimesChangedEvent(object sender, EventArgs e)
	{
		Update();
	}

	private void _markerCanvas_Loaded(object sender, RoutedEventArgs e)
	{
		Update();
	}

	private void Update()
	{
		if (base.IsLoaded && base.TimeAndSpanController != null && !(base.ActualWidth <= 0.0))
		{
			_rulerBaselineRectangle.Margin = ((base.AppearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Full) ? new Thickness(0.0, 29.0, 0.0, 0.0) : new Thickness(0.0, 25.0, 0.0, 0.0));
			List<MarkerInfo> markerInfos = CalculateMarkerInfos();
			UpdateMarkers(markerInfos);
		}
	}

	private void UpdateMarkers(List<MarkerInfo> markerInfos)
	{
		foreach (KeyValuePair<DateTime, TimeMarker> usedTimeMarker in _usedTimeMarkers)
		{
			_freeTimeMarkers.Add(usedTimeMarker.Key, usedTimeMarker.Value);
		}
		_usedTimeMarkers.Clear();
		List<MarkerInfo> list = new List<MarkerInfo>();
		foreach (MarkerInfo markerInfo in markerInfos)
		{
			if (_freeTimeMarkers.ContainsKey(markerInfo.Time))
			{
				TimeMarker timeMarker = _freeTimeMarkers[markerInfo.Time];
				timeMarker.Update(markerInfo.MarkerState, base.TimeAndSpanController.GetPixelOffset(markerInfo.Time), markerInfo.Narrow, markerInfo.Time, markerInfo.LegendFormatString, base.AppearanceAndBehaviourController.TimeZone);
				_usedTimeMarkers.Add(markerInfo.Time, timeMarker);
				_freeTimeMarkers.Remove(markerInfo.Time);
			}
			else
			{
				list.Add(markerInfo);
			}
		}
		foreach (MarkerInfo item in list)
		{
			TimeMarker timeMarker2;
			if (_freeTimeMarkers.Count > 0)
			{
				timeMarker2 = _freeTimeMarkers.First().Value;
				_freeTimeMarkers.Remove(_freeTimeMarkers.First().Key);
			}
			else
			{
				timeMarker2 = new TimeMarker();
				timeMarker2.Height = base.ActualHeight;
				_markerCanvas.Children.Add(timeMarker2);
			}
			_usedTimeMarkers.Add(item.Time, timeMarker2);
			timeMarker2.Update(item.MarkerState, base.TimeAndSpanController.GetPixelOffset(item.Time), item.Narrow, item.Time, item.LegendFormatString, base.AppearanceAndBehaviourController.TimeZone);
		}
		foreach (KeyValuePair<DateTime, TimeMarker> freeTimeMarker in _freeTimeMarkers)
		{
			freeTimeMarker.Value.Hide();
		}
	}

	private List<MarkerInfo> CalculateMarkerInfos()
	{
		List<MarkerInfo> list = new List<MarkerInfo>();
		int num = (int)base.ActualWidth / 20;
		if (num > 0)
		{
			TimeSpan timeSpan = new TimeSpan(base.TimeAndSpanController.TimelineSpan.Ticks / num);
			DateTime dateTime = base.TimeAndSpanController.TimelineTime.Subtract(new TimeSpan(base.TimeAndSpanController.TimelineSpan.Ticks / 2));
			DateTime dateTime2;
			TimeSpan timeSpan2;
			string legendFormatString;
			if (timeSpan <= TimeSpan.FromSeconds(10.0))
			{
				timeSpan = TimeSpan.FromSeconds(10.0);
				dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 10 * (dateTime.Second / 10), DateTimeKind.Utc);
				timeSpan2 = TimeSpan.FromMinutes(1.0);
				legendFormatString = "t";
			}
			else if (timeSpan <= TimeSpan.FromSeconds(15.0))
			{
				timeSpan = TimeSpan.FromSeconds(15.0);
				dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 15 * (dateTime.Second / 15), DateTimeKind.Utc);
				timeSpan2 = TimeSpan.FromMinutes(1.0);
				legendFormatString = "t";
			}
			else if (timeSpan <= TimeSpan.FromSeconds(30.0))
			{
				timeSpan = TimeSpan.FromSeconds(30.0);
				dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 30 * (dateTime.Second / 30), DateTimeKind.Utc);
				timeSpan2 = TimeSpan.FromMinutes(2.0);
				legendFormatString = "t";
			}
			else if (timeSpan <= TimeSpan.FromMinutes(1.0))
			{
				timeSpan = TimeSpan.FromMinutes(1.0);
				dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute / 1, 0, DateTimeKind.Utc);
				timeSpan2 = TimeSpan.FromMinutes(5.0);
				legendFormatString = "t";
			}
			else if (timeSpan <= TimeSpan.FromMinutes(2.0))
			{
				timeSpan = TimeSpan.FromMinutes(2.0);
				dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 2 * (dateTime.Minute / 2), 0, DateTimeKind.Utc);
				timeSpan2 = TimeSpan.FromMinutes(10.0);
				legendFormatString = "t";
			}
			else if (timeSpan <= TimeSpan.FromMinutes(5.0))
			{
				timeSpan = TimeSpan.FromMinutes(5.0);
				dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 5 * (dateTime.Minute / 5), 0, DateTimeKind.Utc);
				timeSpan2 = TimeSpan.FromMinutes(30.0);
				legendFormatString = "t";
			}
			else if (timeSpan <= TimeSpan.FromMinutes(10.0))
			{
				timeSpan = TimeSpan.FromMinutes(10.0);
				dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 10 * (dateTime.Minute / 10), 0, DateTimeKind.Utc);
				timeSpan2 = TimeSpan.FromHours(1.0);
				legendFormatString = "t";
			}
			else if (timeSpan <= TimeSpan.FromMinutes(15.0))
			{
				timeSpan = TimeSpan.FromMinutes(15.0);
				dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 15 * (dateTime.Minute / 15), 0, DateTimeKind.Utc);
				timeSpan2 = TimeSpan.FromHours(1.0);
				legendFormatString = "t";
			}
			else if (timeSpan <= TimeSpan.FromMinutes(30.0))
			{
				timeSpan = TimeSpan.FromMinutes(30.0);
				dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 30 * (dateTime.Minute / 30), 0, DateTimeKind.Utc);
				timeSpan2 = TimeSpan.FromHours(2.0);
				legendFormatString = "t";
			}
			else if (timeSpan <= TimeSpan.FromHours(1.0))
			{
				timeSpan = TimeSpan.FromHours(1.0);
				dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0, DateTimeKind.Utc);
				timeSpan2 = TimeSpan.FromHours(4.0);
				legendFormatString = "t";
			}
			else if (timeSpan <= TimeSpan.FromHours(2.0))
			{
				timeSpan = TimeSpan.FromHours(2.0);
				dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 2 * (dateTime.Hour / 2), 0, 0, DateTimeKind.Utc);
				timeSpan2 = TimeSpan.FromHours(8.0);
				legendFormatString = "t";
			}
			else if (timeSpan <= TimeSpan.FromHours(4.0))
			{
				timeSpan = TimeSpan.FromHours(4.0);
				dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 4 * (dateTime.Hour / 4), 0, 0, DateTimeKind.Utc);
				timeSpan2 = TimeSpan.FromHours(12.0);
				legendFormatString = "t";
			}
			else if (timeSpan <= TimeSpan.FromHours(6.0))
			{
				timeSpan = TimeSpan.FromHours(6.0);
				dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 6 * (dateTime.Hour / 6), 0, 0, DateTimeKind.Utc);
				timeSpan2 = TimeSpan.FromDays(1.0);
				legendFormatString = "m";
			}
			else
			{
				timeSpan = TimeSpan.FromHours(12.0);
				dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 12 * (dateTime.Hour / 12), 0, 0, DateTimeKind.Utc);
				timeSpan2 = TimeSpan.FromDays(2.0);
				legendFormatString = "m";
			}
			long num2 = (DateTime.MinValue.Ticks + base.AppearanceAndBehaviourController.TimeZone.GetUtcOffset(base.TimeAndSpanController.TimelineTime).Ticks) % timeSpan2.Ticks;
			DateTime dateTime3 = dateTime2.AddTicks(-(dateTime2.Ticks % timeSpan2.Ticks + num2)).Subtract(TimeSpan.FromTicks(timeSpan2.Ticks));
			DateTime dateTime4 = base.TimeAndSpanController.TimelineTime.Add(new TimeSpan(base.TimeAndSpanController.TimelineSpan.Ticks / 2 + timeSpan2.Ticks));
			while (dateTime3 <= dateTime4)
			{
				MarkerInfo markerInfo = new MarkerInfo();
				markerInfo.Time = dateTime3;
				if ((dateTime3.Ticks + num2) % timeSpan2.Ticks == 0L)
				{
					markerInfo.MarkerState = MarkerState.Primary;
				}
				else if ((dateTime3.Ticks + num2) % (timeSpan2.Ticks / 2) == 0L)
				{
					markerInfo.MarkerState = MarkerState.Secondary;
				}
				else
				{
					markerInfo.MarkerState = MarkerState.Tertiary;
				}
				markerInfo.LegendFormatString = legendFormatString;
				markerInfo.Narrow = base.AppearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Narrow;
				list.Add(markerInfo);
				dateTime3 = dateTime3.Add(timeSpan);
			}
		}
		return list;
	}
}
