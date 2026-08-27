using System;
using System.Windows.Input;
using System.Windows.Markup;
using VideoOS.UI.Common.WPF.Timeline.Cache;
using VideoOS.UI.Common.WPF.Timeline.Control.Controllers;
using VideoOS.UI.Common.WPF.Tween;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.Drag;

public partial class DragLayer : LayerBase, IComponentConnector
{
	private class DragMouseMoveInfo
	{
		public DateTime Time { get; set; }

		public double Position { get; set; }
	}

	private const int _scrollFPS = 60;

	private const double _throwingScrollDeaccellerationFactor = 0.99;

	private double _dragStartMousePosition = double.NaN;

	private DateTime _dragStartTimelineTime = DateTime.MinValue;

	private bool _precisionDrag;

	private DragMouseMoveInfo _dragMouseMoveInfoNew;

	private DragMouseMoveInfo _dragMouseMoveInfoOld;

	private DateTime _dragLastMouseMoveTime;

	private double _scrollSpeed;

	private VideoOS.UI.Common.WPF.Tween.Tween _scrollTween;

	private DateTime _lastTweenTime;

	private bool __isUserDragging;

	public bool IsUserDragging
	{
		get
		{
			return __isUserDragging;
		}
		private set
		{
			if (value != __isUserDragging)
			{
				__isUserDragging = value;
				FireIsUserDraggingChangedEvent();
			}
		}
	}

	public event EventHandler IsUserDraggingChangedEvent;

	private void FireIsUserDraggingChangedEvent()
	{
		if (this.IsUserDraggingChangedEvent != null)
		{
			this.IsUserDraggingChangedEvent(this, new EventArgs());
		}
	}

	public DragLayer()
	{
		InitializeComponent();
		_scrollTween = new VideoOS.UI.Common.WPF.Tween.Tween(base.Dispatcher);
		_scrollTween.TweenUpdateEvent += _scrollTween_TweenUpdateEvent;
		_scrollTween.FPS = 60;
	}

	public override void Init(TimeAndSpanController timeAndSpanController, AppearanceAndBehaviourController appearanceAndBehaviourController, ITimelineSequenceCache timelineCache)
	{
		base.Init(timeAndSpanController, appearanceAndBehaviourController, timelineCache);
	}

	public void StopThrow()
	{
		StopScroll();
	}

	public void SimulateLeftMouseDown(MouseButtonEventArgs e)
	{
		_dragRectangle_MouseLeftButtonDown(null, e);
	}

	private void _dragRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		StopScroll();
		if (e.ClickCount == 2)
		{
			DateTime time = base.TimeAndSpanController.GetTimelineMinTime().AddTicks((long)(e.GetPosition(_dragRectangle).X * (double)base.TimeAndSpanController.GetTicksPerPixel()));
			base.AppearanceAndBehaviourController.PlaybackState = PlaybackState.Stopped;
			base.TimeAndSpanController.SetTimelineTime(time, animate: true);
			return;
		}
		_dragStartMousePosition = e.GetPosition(_dragRectangle).X;
		_dragStartTimelineTime = base.TimeAndSpanController.TimelineTime;
		_dragMouseMoveInfoNew = new DragMouseMoveInfo
		{
			Position = _dragStartMousePosition,
			Time = DateTime.UtcNow
		};
		_dragMouseMoveInfoOld = null;
		IsUserDragging = true;
		_precisionDrag = (Keyboard.Modifiers & ModifierKeys.Control) > ModifierKeys.None;
		_dragRectangle.CaptureMouse();
	}

	private void _dragRectangle_MouseMove(object sender, MouseEventArgs e)
	{
		if (double.IsNaN(_dragStartMousePosition))
		{
			return;
		}
		double x = e.GetPosition(_dragRectangle).X;
		if (_precisionDrag != (Keyboard.Modifiers & ModifierKeys.Control) > ModifierKeys.None)
		{
			_precisionDrag = (Keyboard.Modifiers & ModifierKeys.Control) > ModifierKeys.None;
			_dragStartMousePosition = x;
			_dragStartTimelineTime = base.TimeAndSpanController.TimelineTime;
			_dragMouseMoveInfoNew = new DragMouseMoveInfo
			{
				Position = _dragStartMousePosition,
				Time = DateTime.UtcNow
			};
			_dragMouseMoveInfoOld = null;
		}
		double num = x - _dragStartMousePosition;
		_dragLastMouseMoveTime = DateTime.UtcNow;
		if (Math.Abs(_dragMouseMoveInfoNew.Position - x) >= 50.0 && new TimeSpan(DateTime.UtcNow.Ticks - _dragMouseMoveInfoNew.Time.Ticks).TotalMilliseconds >= 100.0)
		{
			_dragMouseMoveInfoOld = _dragMouseMoveInfoNew;
			_dragMouseMoveInfoNew = new DragMouseMoveInfo
			{
				Position = x,
				Time = DateTime.UtcNow
			};
		}
		long num2 = (long)((double)(-base.TimeAndSpanController.GetTicksPerPixel()) * num);
		if (_precisionDrag)
		{
			num2 /= 10;
		}
		DateTime time = _dragStartTimelineTime.AddTicks(num2);
		if (base.TimeAndSpanController.SelectionState != SelectionState.Disabled)
		{
			if (Math.Abs(time.Ticks - base.TimeAndSpanController.SelectionFromTime.Ticks) < base.TimeAndSpanController.GetTicksPerPixel() * 10)
			{
				time = base.TimeAndSpanController.SelectionFromTime;
			}
			else if (base.TimeAndSpanController.SelectionMode == SelectionMode.FromCenterTo && Math.Abs(time.Ticks - base.TimeAndSpanController.SelectionCenterTime.Ticks) < base.TimeAndSpanController.GetTicksPerPixel() * 10)
			{
				time = base.TimeAndSpanController.SelectionCenterTime;
			}
			else if (Math.Abs(time.Ticks - base.TimeAndSpanController.SelectionToTime.Ticks) < base.TimeAndSpanController.GetTicksPerPixel() * 10)
			{
				time = base.TimeAndSpanController.SelectionToTime;
			}
		}
		base.TimeAndSpanController.SetTimelineTime(time, animate: false);
	}

	private void _dragRectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		_dragStartMousePosition = double.NaN;
		_dragStartTimelineTime = DateTime.MinValue;
		if (_dragMouseMoveInfoNew != null && _dragMouseMoveInfoOld != null && !_precisionDrag && new TimeSpan(DateTime.UtcNow.Ticks - _dragLastMouseMoveTime.Ticks).TotalMilliseconds < 50.0)
		{
			double num = _dragMouseMoveInfoNew.Position - _dragMouseMoveInfoOld.Position;
			double totalSeconds = new TimeSpan(_dragMouseMoveInfoNew.Time.Ticks - _dragMouseMoveInfoOld.Time.Ticks).TotalSeconds;
			double num2 = num / totalSeconds;
			StartScroll(0.0 - num2 / 3200.0, 0.99);
		}
		IsUserDragging = false;
		_dragRectangle.ReleaseMouseCapture();
	}

	private void StartScroll(double speed, double deaccellerationFactor)
	{
		if (speed == 0.0)
		{
			StopScroll();
			return;
		}
		_scrollSpeed = speed * 100.0;
		_scrollTween.StartValues = new double[1] { Math.Log10(Math.Abs(_scrollSpeed)) };
		_scrollTween.EndValues = new double[1] { Math.Log10(5.0) };
		_scrollTween.Duration = TimeSpan.FromMilliseconds(1000.0 * (Math.Log10(5.0) - Math.Log10(Math.Abs(_scrollSpeed))) / (60.0 * Math.Log10(deaccellerationFactor)));
		_lastTweenTime = DateTime.UtcNow;
		_scrollTween.Start();
	}

	private void _scrollTween_TweenUpdateEvent(object sender, TweenEventArgs e)
	{
		double totalMilliseconds = (DateTime.UtcNow - _lastTweenTime).TotalMilliseconds;
		_lastTweenTime = DateTime.UtcNow;
		if (!double.IsInfinity(e.NewValues[0]) && totalMilliseconds * 60.0 > 800.0)
		{
			double num = Math.Pow(10.0, e.NewValues[0]);
			double num2 = (double)(-Math.Sign(_scrollSpeed)) * totalMilliseconds * num / 50.0;
			long value = (long)((double)(-base.TimeAndSpanController.GetTicksPerPixel()) * num2);
			base.TimeAndSpanController.SetTimelineTime(base.TimeAndSpanController.TimelineTime.AddTicks(value), animate: false);
		}
	}

	private void StopScroll()
	{
		_scrollTween.Stop();
		_scrollSpeed = 0.0;
	}
}
