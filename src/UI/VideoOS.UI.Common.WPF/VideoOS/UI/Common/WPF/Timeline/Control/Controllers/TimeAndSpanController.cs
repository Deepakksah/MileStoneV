using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using VideoOS.UI.Common.WPF.Timeline.Control.Data;
using VideoOS.UI.Common.WPF.Timeline.Events;
using VideoOS.UI.Common.WPF.Tween;

namespace VideoOS.UI.Common.WPF.Timeline.Control.Controllers;

public class TimeAndSpanController
{
	private const int TWEEN_FPS = 60;

	private SelectionMode _intervalSelectionMode;

	private SelectionState _intervalSelectionState;

	private SelectionMarker _intervalSelectionActiveMarker;

	private DateTime _intervalSelectionFromTime;

	private DateTime _intervalSelectionCenterTime;

	private DateTime _intervalSelectionToTime;

	private DateTime _browseTime;

	private DateTime _timelineTime;

	private DateTime _targetTimelineTime;

	private TimeSpan _timelineSpan;

	private TimeSpan _targetTimelineSpan;

	private int _pixelCount;

	private long _firstPixelTimeTicks;

	private long _pixelTimeIntervalTicks;

	private VideoOS.UI.Common.WPF.Tween.Tween _timelineTimeTween;

	private VideoOS.UI.Common.WPF.Tween.Tween _timelineSpanTween;

	public DateTime BrowseTime
	{
		get
		{
			return _browseTime;
		}
		private set
		{
			if (_browseTime != value)
			{
				_browseTime = value;
				if (this.BrowseTimeChanged != null)
				{
					this.BrowseTimeChanged(this, new DateTimeEventArgs(_browseTime));
				}
			}
		}
	}

	public DateTime TimelineTime
	{
		get
		{
			return _timelineTime;
		}
		private set
		{
			if (_timelineTime != value)
			{
				_timelineTime = value;
				bool num = UpdateTimelinePixelTimes();
				if (this.TimelineTimeChanged != null)
				{
					this.TimelineTimeChanged(this, new DateTimeEventArgs(_timelineTime));
				}
				if (num && this.TimelinePixelTimesChanged != null)
				{
					this.TimelinePixelTimesChanged(this, new EventArgs());
				}
				if (SelectionState == SelectionState.Disabled || SelectionActiveMarker == SelectionMarker.None)
				{
					BrowseTime = _timelineTime;
				}
			}
		}
	}

	public DateTime TargetTimelineTime
	{
		get
		{
			return _targetTimelineTime;
		}
		private set
		{
			if (_targetTimelineTime != value)
			{
				_targetTimelineTime = value;
				if (this.TargetTimelineTimeChanged != null)
				{
					this.TargetTimelineTimeChanged(this, new DateTimeEventArgs(_targetTimelineTime));
				}
			}
		}
	}

	public TimeSpan TimelineSpan
	{
		get
		{
			return _timelineSpan;
		}
		private set
		{
			if (_timelineSpan != value)
			{
				_timelineSpan = value;
				bool num = UpdateTimelinePixelTimes();
				if (this.TimelineSpanChanged != null)
				{
					this.TimelineSpanChanged(this, new TimeSpanEventArgs(_timelineSpan));
				}
				if (num && this.TimelinePixelTimesChanged != null)
				{
					this.TimelinePixelTimesChanged(this, new EventArgs());
				}
			}
		}
	}

	public TimeSpan TargetTimelineSpan
	{
		get
		{
			return _targetTimelineSpan;
		}
		private set
		{
			if (_targetTimelineSpan != value)
			{
				_targetTimelineSpan = value;
				if (this.TargetTimelineSpanChanged != null)
				{
					this.TargetTimelineSpanChanged(this, new TimeSpanEventArgs(_targetTimelineSpan));
				}
			}
		}
	}

	public bool IsSpanTweening => _timelineSpanTween.IsTweening;

	public bool IsTimeTweening => _timelineTimeTween.IsTweening;

	public SelectionMode SelectionMode
	{
		get
		{
			return _intervalSelectionMode;
		}
		set
		{
			if (_intervalSelectionMode != value)
			{
				_intervalSelectionMode = value;
				if (_intervalSelectionMode == SelectionMode.FromTo && SelectionActiveMarker == SelectionMarker.Center)
				{
					SelectionActiveMarker = SelectionMarker.None;
				}
				if (this.SelectionModeChangedEvent != null)
				{
					this.SelectionModeChangedEvent(this, new EventArgs());
				}
			}
		}
	}

	public SelectionState SelectionState
	{
		get
		{
			return _intervalSelectionState;
		}
		set
		{
			if (_intervalSelectionState != value)
			{
				_intervalSelectionState = value;
				if (_intervalSelectionState == SelectionState.Disabled)
				{
					SelectionActiveMarker = SelectionMarker.None;
				}
				if (this.SelectionStateChangedEvent != null)
				{
					this.SelectionStateChangedEvent(this, new EventArgs());
				}
			}
		}
	}

	public SelectionMarker SelectionActiveMarker
	{
		get
		{
			return _intervalSelectionActiveMarker;
		}
		set
		{
			if (_intervalSelectionActiveMarker != value && (value == SelectionMarker.None || SelectionState != SelectionState.Disabled) && (value != SelectionMarker.Center || SelectionMode == SelectionMode.FromCenterTo))
			{
				SelectionActiveMarkerChangeEventArgs e = new SelectionActiveMarkerChangeEventArgs(value, _intervalSelectionActiveMarker);
				if (this.SelectionActiveMarkerChangingEvent != null)
				{
					this.SelectionActiveMarkerChangingEvent(this, e);
				}
				switch (value)
				{
				case SelectionMarker.None:
					BrowseTime = TimelineTime;
					break;
				case SelectionMarker.From:
					BrowseTime = SelectionFromTime;
					break;
				case SelectionMarker.Center:
					BrowseTime = SelectionCenterTime;
					break;
				case SelectionMarker.To:
					BrowseTime = SelectionToTime;
					break;
				}
				_intervalSelectionActiveMarker = value;
				if (this.SelectionActiveMarkerChangedEvent != null)
				{
					this.SelectionActiveMarkerChangedEvent(this, e);
				}
			}
		}
	}

	public DateTime SelectionFromTime => _intervalSelectionFromTime;

	public DateTime SelectionCenterTime => _intervalSelectionCenterTime;

	public DateTime SelectionToTime => _intervalSelectionToTime;

	public DateTime DefaultSelectionFromTime => TimelineTime;

	public DateTime DefaultSelectionCenterTime => TimelineTime.AddTicks(TimelineSpan.Ticks / 8);

	public DateTime DefaultSelectionToTime => TimelineTime.AddTicks(TimelineSpan.Ticks / 4);

	public TimeSpan AnimationDuration { get; set; } = TimeSpan.FromMilliseconds(200.0);

	public event EventHandler SelectionModeChangedEvent;

	public event EventHandler SelectionStateChangedEvent;

	public event EventHandler<SelectionActiveMarkerChangeEventArgs> SelectionActiveMarkerChangingEvent;

	public event EventHandler<SelectionActiveMarkerChangeEventArgs> SelectionActiveMarkerChangedEvent;

	public event EventHandler<DateTimeEventArgs> SelectionFromTimeChanged;

	public event EventHandler<DateTimeEventArgs> SelectionCenterTimeChanged;

	public event EventHandler<DateTimeEventArgs> SelectionToTimeChanged;

	public event EventHandler<DateTimeEventArgs> BrowseTimeChanged;

	public event EventHandler<DateTimeEventArgs> TimelineTimeChanged;

	public event EventHandler<DateTimeEventArgs> TargetTimelineTimeChanged;

	public event EventHandler<TimeSpanEventArgs> TimelineSpanChanged;

	public event EventHandler<TimeSpanEventArgs> TargetTimelineSpanChanged;

	public event EventHandler TimelineSpanTweenComplete;

	public event EventHandler TimelineTimeTweenComplete;

	public event EventHandler TimelinePixelTimesChanged;

	public TimeAndSpanController()
	{
		_browseTime = DateTime.UtcNow;
		_timelineTime = _browseTime;
		_targetTimelineTime = _browseTime;
		_timelineSpan = TimeSpan.FromMinutes(60.0);
		_targetTimelineSpan = TimeSpan.FromMinutes(60.0);
		_intervalSelectionCenterTime = _timelineTime;
		_intervalSelectionFromTime = _timelineTime.AddMinutes(-5.0);
		_intervalSelectionToTime = _timelineTime.AddMinutes(5.0);
	}

	public void Init(Dispatcher dispatcher)
	{
		_timelineTimeTween = new VideoOS.UI.Common.WPF.Tween.Tween(dispatcher);
		_timelineTimeTween.FPS = 60;
		_timelineTimeTween.TweenUpdateEvent += _timeTween_TweenUpdateEvent;
		_timelineTimeTween.TweenCompleteEvent += _timeTween_TweenCompleteEvent;
		_timelineSpanTween = new VideoOS.UI.Common.WPF.Tween.Tween(dispatcher);
		_timelineSpanTween.FPS = 60;
		_timelineSpanTween.TweenUpdateEvent += _spanTween_TweenUpdateEvent;
		_timelineSpanTween.TweenCompleteEvent += _spanTween_TweenCompleteEvent;
	}

	public void SetPixelCount(int pixelCount)
	{
		if (_pixelCount != pixelCount)
		{
			_pixelCount = pixelCount;
			if (UpdateTimelinePixelTimes() && this.TimelinePixelTimesChanged != null)
			{
				this.TimelinePixelTimesChanged(this, new EventArgs());
			}
		}
	}

	public DateTime GetTimelineMinTime()
	{
		return new DateTime(TimelineTime.Ticks - TimelineSpan.Ticks / 2, DateTimeKind.Utc);
	}

	public DateTime GetTimelineMaxTime()
	{
		return new DateTime(TimelineTime.Ticks + TimelineSpan.Ticks / 2, DateTimeKind.Utc);
	}

	public void StopTween()
	{
		if (_timelineTimeTween.IsTweening)
		{
			_timelineTimeTween.Stop();
			TimelineTime = TargetTimelineTime;
		}
		if (_timelineSpanTween.IsTweening)
		{
			_timelineSpanTween.Stop();
			TimelineSpan = TargetTimelineSpan;
		}
	}

	public void SetTimelineTime(DateTime time, bool animate)
	{
		SetTimelineTime(time, animate ? AnimationDuration : TimeSpan.Zero);
	}

	public void SetTimelineTime(DateTime time, TimeSpan animationDuration)
	{
		_timelineTimeTween.Stop();
		TargetTimelineTime = time;
		if (animationDuration <= TimeSpan.Zero)
		{
			TimelineTime = time;
			return;
		}
		_timelineTimeTween.Duration = animationDuration;
		_timelineTimeTween.StartValues = new double[1] { TimelineTime.Ticks };
		_timelineTimeTween.EndValues = new double[1] { time.Ticks };
		_timelineTimeTween.Start();
	}

	public void SetSpan(TimeSpan span, bool animate)
	{
		SetSpan(span, animate ? AnimationDuration : TimeSpan.Zero);
	}

	public void SetSpan(TimeSpan span, TimeSpan animationDuration)
	{
		_timelineSpanTween.Stop();
		TargetTimelineSpan = span;
		if (animationDuration <= TimeSpan.Zero)
		{
			TimelineSpan = span;
			return;
		}
		_timelineSpanTween.Duration = animationDuration;
		_timelineSpanTween.StartValues = new double[1] { TimelineSpan.Ticks };
		_timelineSpanTween.EndValues = new double[1] { span.Ticks };
		_timelineSpanTween.Start();
	}

	public void SetSelectionTimes(DateTime fromTime, DateTime centerTime, DateTime toTime, bool adjustToFromTimeIfNessesary, bool adjustToCenterTimeIfNessesary, bool adjustToToTimeIfNessesary)
	{
		if (adjustToFromTimeIfNessesary)
		{
			if (fromTime > centerTime)
			{
				centerTime = fromTime;
			}
			if (fromTime > toTime)
			{
				toTime = fromTime;
			}
			if (centerTime > toTime)
			{
				centerTime = toTime;
			}
		}
		else if (adjustToCenterTimeIfNessesary)
		{
			if (fromTime > centerTime)
			{
				fromTime = centerTime;
			}
			if (toTime < centerTime)
			{
				toTime = centerTime;
			}
		}
		else if (adjustToToTimeIfNessesary)
		{
			if (toTime < centerTime)
			{
				centerTime = toTime;
			}
			if (toTime < fromTime)
			{
				fromTime = toTime;
			}
			if (centerTime < fromTime)
			{
				centerTime = fromTime;
			}
		}
		SetSelectionTimes(fromTime, centerTime, toTime);
	}

	public int GetPixelOffset(DateTime time)
	{
		if (_firstPixelTimeTicks == 0L || _pixelTimeIntervalTicks == 0L)
		{
			return 0;
		}
		return (int)((time.Ticks - _firstPixelTimeTicks) / _pixelTimeIntervalTicks);
	}

	public DateTime GetPixelTime(int pixelOffset)
	{
		if (_firstPixelTimeTicks == 0L || _pixelTimeIntervalTicks == 0L)
		{
			return DateTime.MinValue;
		}
		return new DateTime(_firstPixelTimeTicks + _pixelTimeIntervalTicks * pixelOffset, DateTimeKind.Utc);
	}

	public long GetTicksPerPixel()
	{
		return _pixelTimeIntervalTicks;
	}

	public void FitTimeAndSpan(DateTime minTime, DateTime timelineTime, DateTime maxTime, bool allowSpanDecrease, bool allowSpanIncrease, IEnumerable<TimelineSpan> timelineSpans, bool animate)
	{
		FitTimeAndSpan(minTime, timelineTime, maxTime, allowSpanDecrease, allowSpanIncrease, timelineSpans, animate ? AnimationDuration : TimeSpan.Zero);
	}

	public void FitTimeAndSpan(DateTime minTime, DateTime timelineTime, DateTime maxTime, bool allowSpanDecrease, bool allowSpanIncrease, IEnumerable<TimelineSpan> timelineSpans, TimeSpan animationDuration)
	{
		TimeSpan timeSpan = new TimeSpan(Math.Max((timelineTime - minTime).Ticks, (maxTime - timelineTime).Ticks) * 2);
		timeSpan = TimeSpan.FromTicks((long)((double)timeSpan.Ticks * 1.2));
		TimeSpan timeSpan2 = timelineSpans.Last().TimeSpan;
		foreach (TimelineSpan timelineSpan in timelineSpans)
		{
			if (timeSpan <= timelineSpan.TimeSpan)
			{
				timeSpan2 = timelineSpan.TimeSpan;
				break;
			}
		}
		TimeSpan span = TimelineSpan;
		if (timeSpan2 < TimelineSpan && allowSpanDecrease)
		{
			span = timeSpan2;
		}
		else if (timeSpan > TimelineSpan && allowSpanIncrease)
		{
			span = timeSpan2;
		}
		SetTimelineTime(timelineTime, animationDuration);
		SetSpan(span, animationDuration);
	}

	public void SelectionFitToTimeline(List<TimelineSpan> timelineSpans, bool decreaseTimelineSpan, bool animate)
	{
		if (SelectionState == SelectionState.Disabled)
		{
			return;
		}
		SetTimelineTime(SelectionFromTime, animate);
		TimeSpan timeSpan = SelectionToTime - SelectionFromTime;
		if (!(timeSpan.Ticks > TimelineSpan.Ticks / 2 || decreaseTimelineSpan))
		{
			return;
		}
		TimelineSpan timelineSpan = timelineSpans.Last();
		foreach (TimelineSpan timelineSpan2 in timelineSpans)
		{
			if (timeSpan.Ticks <= timelineSpan2.TimeSpan.Ticks / 2)
			{
				timelineSpan = timelineSpan2;
				break;
			}
		}
		SetSpan(timelineSpan.TimeSpan, animate);
	}

	private void _spanTween_TweenUpdateEvent(object sender, TweenEventArgs e)
	{
		TimelineSpan = new TimeSpan((long)e.NewValues[0]);
	}

	private void _spanTween_TweenCompleteEvent(object sender, EventArgs e)
	{
		if (this.TimelineSpanTweenComplete != null)
		{
			this.TimelineSpanTweenComplete(this, new EventArgs());
		}
	}

	private void _timeTween_TweenUpdateEvent(object sender, TweenEventArgs e)
	{
		TimelineTime = new DateTime((long)e.NewValues[0], DateTimeKind.Utc);
	}

	private void _timeTween_TweenCompleteEvent(object sender, EventArgs e)
	{
		if (this.TimelineTimeTweenComplete != null)
		{
			this.TimelineTimeTweenComplete(this, new EventArgs());
		}
	}

	private void SetSelectionTimes(DateTime fromTime, DateTime centerTime, DateTime toTime)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		if (_intervalSelectionFromTime != fromTime)
		{
			_intervalSelectionFromTime = fromTime;
			flag = true;
		}
		if (_intervalSelectionCenterTime != centerTime)
		{
			_intervalSelectionCenterTime = centerTime;
			flag2 = true;
		}
		if (_intervalSelectionToTime != toTime)
		{
			_intervalSelectionToTime = toTime;
			flag3 = true;
		}
		if (flag)
		{
			if (this.SelectionFromTimeChanged != null)
			{
				this.SelectionFromTimeChanged(this, new DateTimeEventArgs(_intervalSelectionFromTime));
			}
			if (_intervalSelectionActiveMarker == SelectionMarker.From)
			{
				BrowseTime = _intervalSelectionFromTime;
			}
		}
		if (flag2)
		{
			if (this.SelectionCenterTimeChanged != null)
			{
				this.SelectionCenterTimeChanged(this, new DateTimeEventArgs(_intervalSelectionCenterTime));
			}
			if (_intervalSelectionActiveMarker == SelectionMarker.Center)
			{
				BrowseTime = _intervalSelectionCenterTime;
			}
		}
		if (flag3)
		{
			if (this.SelectionToTimeChanged != null)
			{
				this.SelectionToTimeChanged(this, new DateTimeEventArgs(_intervalSelectionToTime));
			}
			if (_intervalSelectionActiveMarker == SelectionMarker.To)
			{
				BrowseTime = _intervalSelectionToTime;
			}
		}
	}

	private bool UpdateTimelinePixelTimes()
	{
		long num;
		long num2;
		if (_pixelCount <= 0)
		{
			num = 0L;
			num2 = 0L;
		}
		else
		{
			num2 = TimelineSpan.Ticks / _pixelCount;
			num = GetTimelineMinTime().Ticks / num2 * num2;
		}
		if (num != _firstPixelTimeTicks || num2 != _pixelTimeIntervalTicks)
		{
			_firstPixelTimeTicks = num;
			_pixelTimeIntervalTicks = num2;
			return true;
		}
		return false;
	}
}
