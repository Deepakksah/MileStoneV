using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using VideoOS.UI.Common.WPF.Behaviors.Input;
using VideoOS.UI.Common.WPF.Utils;

namespace VideoOS.UI.Common.WPF.Behaviors;

public class MouseInputBehavior : BehaviorBase<FrameworkElement>, IMouseInput
{
	public static readonly DependencyProperty ParentContainerProperty = DependencyProperty.Register("ParentContainer", typeof(FrameworkElement), typeof(MouseInputBehavior), new PropertyMetadata((object)null));

	public static readonly DependencyProperty LimitBoundsToParentContainerProperty = DependencyProperty.Register("LimitBoundsToParentContainer", typeof(bool), typeof(MouseInputBehavior), new PropertyMetadata(true));

	public static readonly DependencyProperty IsDoubleClickEnabledProperty = DependencyProperty.Register("IsDoubleClickEnabled", typeof(bool), typeof(MouseInputBehavior), new PropertyMetadata(true));

	public static readonly DependencyProperty IsDoubleClickEnabledProviderProperty = DependencyProperty.Register("IsDoubleClickEnabledProvider", typeof(Func<bool>), typeof(MouseInputBehavior), new PropertyMetadata((object)null));

	public static readonly DependencyProperty IsLongPressEnabledProperty = DependencyProperty.Register("IsLongPressEnabled", typeof(bool), typeof(MouseInputBehavior), new PropertyMetadata(false));

	public static readonly DependencyProperty IsLongPressEnabledProviderProperty = DependencyProperty.Register("IsLongPressEnabledProvider", typeof(Func<bool>), typeof(MouseInputBehavior), new PropertyMetadata((object)null));

	public static readonly DependencyProperty IsDraggingEnabledProperty = DependencyProperty.Register("IsDraggingEnabled", typeof(bool), typeof(MouseInputBehavior), new PropertyMetadata(true));

	public static readonly DependencyProperty IsDraggingEnabledProviderProperty = DependencyProperty.Register("IsDraggingEnabledProvider", typeof(Func<bool>), typeof(MouseInputBehavior), new PropertyMetadata((object)null));

	public static readonly DependencyProperty IsMouseWheelEnabledProperty = DependencyProperty.Register("IsMouseWheelEnabled", typeof(bool), typeof(MouseInputBehavior), new PropertyMetadata(true));

	public static readonly DependencyProperty IsMouseWheelEnabledProviderProperty = DependencyProperty.Register("IsMouseWheelEnabledProvider", typeof(Func<bool>), typeof(MouseInputBehavior), new PropertyMetadata((object)null));

	public static readonly DependencyProperty LongPressStartDelayMillisProperty = DependencyProperty.Register("LongPressStartDelayMillis", typeof(double), typeof(MouseInputBehavior), new PropertyMetadata(500.0));

	public static readonly DependencyProperty DragThresholdDistanceProperty = DependencyProperty.Register("DragThresholdDistance", typeof(double), typeof(MouseInputBehavior), new PropertyMetadata(10.0));

	public static readonly DependencyProperty MarkAllEnabledEventsAsHandledProperty = DependencyProperty.Register("MarkAllEnabledEventsAsHandled", typeof(bool), typeof(MouseInputBehavior), new PropertyMetadata(false));

	private Point? _mouseLeftButtonDownPoint;

	private volatile bool _mouseLeftButtonClickInProgress;

	private bool _mouseLeftButtonDoubleClickInProgress;

	private bool _mouseRightButtonClickInProgress;

	private bool _isDragging;

	private bool _isLongPressing;

	private readonly DispatcherTimer _longPressTimer = new DispatcherTimer();

	private readonly DispatcherTimer _doubleClickEdgeCaseTimer = new DispatcherTimer();

	public FrameworkElement ParentContainer
	{
		get
		{
			return (FrameworkElement)((DependencyObject)(object)this).GetValue(ParentContainerProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(ParentContainerProperty, (object)value);
		}
	}

	public bool LimitBoundsToParentContainer
	{
		get
		{
			return (bool)((DependencyObject)(object)this).GetValue(LimitBoundsToParentContainerProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(LimitBoundsToParentContainerProperty, (object)value);
		}
	}

	public bool IsDoubleClickEnabled
	{
		get
		{
			if (IsDoubleClickEnabledProvider != null)
			{
				((DependencyObject)(object)this).SetCurrentValue(IsDoubleClickEnabledProperty, (object)IsDoubleClickEnabledProvider());
			}
			return (bool)((DependencyObject)(object)this).GetValue(IsDoubleClickEnabledProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(IsDoubleClickEnabledProperty, (object)value);
		}
	}

	public Func<bool> IsDoubleClickEnabledProvider
	{
		get
		{
			return (Func<bool>)((DependencyObject)(object)this).GetValue(IsDoubleClickEnabledProviderProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(IsDoubleClickEnabledProviderProperty, (object)value);
		}
	}

	public bool IsLongPressEnabled
	{
		get
		{
			if (IsLongPressEnabledProvider != null)
			{
				((DependencyObject)(object)this).SetCurrentValue(IsLongPressEnabledProperty, (object)IsLongPressEnabledProvider());
			}
			return (bool)((DependencyObject)(object)this).GetValue(IsLongPressEnabledProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(IsLongPressEnabledProperty, (object)value);
		}
	}

	public Func<bool> IsLongPressEnabledProvider
	{
		get
		{
			return (Func<bool>)((DependencyObject)(object)this).GetValue(IsLongPressEnabledProviderProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(IsLongPressEnabledProviderProperty, (object)value);
		}
	}

	public bool IsDraggingEnabled
	{
		get
		{
			if (IsDraggingEnabledProvider != null)
			{
				((DependencyObject)(object)this).SetCurrentValue(IsDraggingEnabledProperty, (object)IsDraggingEnabledProvider());
			}
			return (bool)((DependencyObject)(object)this).GetValue(IsDraggingEnabledProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(IsDraggingEnabledProperty, (object)value);
		}
	}

	public Func<bool> IsDraggingEnabledProvider
	{
		get
		{
			return (Func<bool>)((DependencyObject)(object)this).GetValue(IsDraggingEnabledProviderProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(IsDraggingEnabledProviderProperty, (object)value);
		}
	}

	public bool IsMouseWheelEnabled
	{
		get
		{
			if (IsMouseWheelEnabledProvider != null)
			{
				((DependencyObject)(object)this).SetCurrentValue(IsMouseWheelEnabledProperty, (object)IsMouseWheelEnabledProvider());
			}
			return (bool)((DependencyObject)(object)this).GetValue(IsMouseWheelEnabledProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(IsMouseWheelEnabledProperty, (object)value);
		}
	}

	public Func<bool> IsMouseWheelEnabledProvider
	{
		get
		{
			return (Func<bool>)((DependencyObject)(object)this).GetValue(IsMouseWheelEnabledProviderProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(IsMouseWheelEnabledProviderProperty, (object)value);
		}
	}

	public double LongPressStartDelayMillis
	{
		get
		{
			return (double)((DependencyObject)(object)this).GetValue(LongPressStartDelayMillisProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(LongPressStartDelayMillisProperty, (object)value);
		}
	}

	public double DragThresholdDistance
	{
		get
		{
			return (double)((DependencyObject)(object)this).GetValue(DragThresholdDistanceProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(DragThresholdDistanceProperty, (object)value);
		}
	}

	public bool MarkAllEnabledEventsAsHandled
	{
		get
		{
			return (bool)((DependencyObject)(object)this).GetValue(MarkAllEnabledEventsAsHandledProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(MarkAllEnabledEventsAsHandledProperty, (object)value);
		}
	}

	private FrameworkElement AssociatedObject => GetAssociatedObject();

	private static bool IsControlPressed
	{
		get
		{
			if (!Keyboard.IsKeyDown(Key.LeftCtrl))
			{
				return Keyboard.IsKeyDown(Key.RightCtrl);
			}
			return true;
		}
	}

	private bool IsDoubleClickEdgeCaseTimerActive => _doubleClickEdgeCaseTimer.IsEnabled;

	private bool CanDrag
	{
		get
		{
			if (IsDraggingEnabled)
			{
				if (Mouse.Captured != null)
				{
					return object.Equals(Mouse.Captured, AssociatedObject);
				}
				return true;
			}
			return false;
		}
	}

	public event EventHandler<MouseInputMouseDownUpEventArgs> MouseDown;

	public event EventHandler<MouseInputMouseDownUpEventArgs> MouseUp;

	public event EventHandler<VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs> BeforeContextMenuShown;

	public event EventHandler<VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs> AfterContextMenuClosed;

	public event EventHandler<VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs> LongPressStarted;

	public event EventHandler<VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs> LongPressStopped;

	public event EventHandler<VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs> MouseLeftButtonClicked;

	public event EventHandler<VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs> MouseLeftButtonDoubleClicked;

	public event EventHandler<VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs> MouseRightButtonClicked;

	public event EventHandler<VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs> DraggingStarted;

	public event EventHandler<MouseInputDraggingEventArgs> Dragging;

	public event EventHandler<VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs> DraggingStopped;

	public event EventHandler<MouseInputMouseWheelEventArgs> MouseWheel;

	public MouseInputBehavior()
	{
		_doubleClickEdgeCaseTimer.Tick += DoubleClickEdgeCaseTimerOnTick;
	}

	private void DoubleClickEdgeCaseTimerOnTick(object sender, EventArgs e)
	{
		_doubleClickEdgeCaseTimer.Stop();
	}

	protected override void OnSetup()
	{
		FrameworkElement associatedObject = AssociatedObject;
		if (associatedObject != null)
		{
			associatedObject.MouseRightButtonDown += Element_OnMouseRightButtonDown;
			associatedObject.MouseRightButtonUp += Element_OnMouseRightButtonUp;
			associatedObject.MouseLeftButtonDown += Element_OnMouseLeftButtonDown;
			associatedObject.MouseLeftButtonUp += Element_OnMouseLeftButtonUp;
			associatedObject.MouseMove += Element_OnMouseMove;
			associatedObject.MouseWheel += Element_OnMouseWheel;
		}
	}

	protected override void Attached()
	{
		(AssociatedObject as IMouseInputAttachable)?.Attached(this);
		_longPressTimer.Tick += LongPressTimerOnTick;
	}

	protected override void OnCleanup()
	{
		FrameworkElement associatedObject = AssociatedObject;
		if (associatedObject != null)
		{
			associatedObject.MouseRightButtonDown -= Element_OnMouseRightButtonDown;
			associatedObject.MouseRightButtonUp -= Element_OnMouseRightButtonUp;
			associatedObject.MouseLeftButtonDown -= Element_OnMouseLeftButtonDown;
			associatedObject.MouseLeftButtonUp -= Element_OnMouseLeftButtonUp;
			associatedObject.MouseMove -= Element_OnMouseMove;
			associatedObject.MouseWheel -= Element_OnMouseWheel;
		}
	}

	protected override void Detached()
	{
		(AssociatedObject as IMouseInputAttachable)?.Detached(this);
		_longPressTimer.Tick -= LongPressTimerOnTick;
		ReleaseMouse();
	}

	private void CaptureMouse()
	{
		if (AssociatedObject != null)
		{
			Mouse.Capture(AssociatedObject);
			Mouse.AddLostMouseCaptureHandler(AssociatedObject, LostMouseCapture);
		}
	}

	private void ReleaseMouse()
	{
		if (AssociatedObject != null && object.Equals(AssociatedObject, Mouse.Captured))
		{
			Mouse.RemoveLostMouseCaptureHandler(AssociatedObject, LostMouseCapture);
			Mouse.Capture(null);
		}
	}

	private void LostMouseCapture(object sender, MouseEventArgs e)
	{
		if (_isDragging)
		{
			this.DraggingStopped?.Invoke(this, VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs.Empty);
			_isDragging = false;
		}
		if (_isLongPressing)
		{
			this.LongPressStopped?.Invoke(this, VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs.Empty);
			_isLongPressing = false;
		}
		_mouseLeftButtonClickInProgress = false;
		_mouseLeftButtonDoubleClickInProgress = false;
		_mouseRightButtonClickInProgress = false;
		e.Handled = true;
	}

	private bool IsMouseOverAssociatedObject()
	{
		if (AssociatedObject == null)
		{
			return false;
		}
		return VisualTreeHelper.HitTest(AssociatedObject, Mouse.GetPosition(AssociatedObject)) != null;
	}

	private void Element_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (!e.Handled)
		{
			_mouseRightButtonClickInProgress = true;
			MouseInputMouseDownUpEventArgs e2 = new MouseInputMouseDownUpEventArgs(e.ChangedButton);
			this.MouseDown?.Invoke(this, e2);
			e.Handled = MarkAllEnabledEventsAsHandled || e2.Handled;
		}
	}

	private void Element_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (!e.Handled)
		{
			MouseInputMouseDownUpEventArgs e2 = new MouseInputMouseDownUpEventArgs(e.ChangedButton);
			this.MouseUp?.Invoke(this, e2);
			bool flag = e2.Handled;
			if (_mouseRightButtonClickInProgress)
			{
				_mouseRightButtonClickInProgress = false;
				e2 = new MouseInputMouseDownUpEventArgs(e.ChangedButton);
				this.MouseRightButtonClicked?.Invoke(this, e2);
				flag = e2.Handled;
			}
			if (!IsControlPressed && AssociatedObject.ContextMenu != null && AssociatedObject.ContextMenu.HasItems)
			{
				this.BeforeContextMenuShown?.Invoke(this, VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs.Empty);
				AssociatedObject.ContextMenu.Closed += ContextMenu_OnClosed;
				AssociatedObject.ContextMenu.IsOpen = true;
				flag = true;
			}
			e.Handled = MarkAllEnabledEventsAsHandled || flag;
		}
	}

	private void ContextMenu_OnClosed(object sender, RoutedEventArgs e)
	{
		if (sender is ContextMenu contextMenu)
		{
			contextMenu.Closed -= ContextMenu_OnClosed;
		}
		this.AfterContextMenuClosed?.Invoke(this, VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs.Empty);
	}

	private void Element_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (e.Handled)
		{
			return;
		}
		MouseInputMouseDownUpEventArgs e2 = new MouseInputMouseDownUpEventArgs(e.ChangedButton);
		this.MouseDown?.Invoke(this, e2);
		bool handled = e2.Handled;
		_mouseLeftButtonDownPoint = e.GetPosition(AssociatedObject);
		int num = e.ClickCount;
		if (num == 1 && IsDoubleClickEdgeCaseTimerActive)
		{
			num = 2;
			_doubleClickEdgeCaseTimer.Stop();
		}
		if (num == 1)
		{
			_doubleClickEdgeCaseTimer.Interval = TimeSpan.FromMilliseconds(NativeMethods.GetDoubleClickTime());
			_doubleClickEdgeCaseTimer.Start();
			_mouseLeftButtonClickInProgress = true;
			_mouseLeftButtonDoubleClickInProgress = false;
			_mouseRightButtonClickInProgress = false;
			if (IsLongPressEnabled)
			{
				_longPressTimer.Interval = TimeSpan.FromMilliseconds(LongPressStartDelayMillis);
				_longPressTimer.Start();
			}
			if (CanDrag || IsLongPressEnabled)
			{
				CaptureMouse();
			}
		}
		else
		{
			_mouseLeftButtonClickInProgress = false;
			_mouseRightButtonClickInProgress = false;
			if (IsDoubleClickEnabled && num == 2 && !IsControlPressed)
			{
				_mouseLeftButtonDoubleClickInProgress = true;
			}
		}
		e.Handled = MarkAllEnabledEventsAsHandled || handled;
	}

	private void Element_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (e.Handled)
		{
			return;
		}
		MouseInputMouseDownUpEventArgs e2 = new MouseInputMouseDownUpEventArgs(e.ChangedButton);
		this.MouseUp?.Invoke(this, e2);
		bool handled = e2.Handled;
		handled |= StopLongPress();
		if (_mouseLeftButtonClickInProgress)
		{
			if (IsDoubleClickEnabled)
			{
				int delay = (int)NativeMethods.GetDoubleClickTime();
				TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
				if (IsMouseOverAssociatedObject() && !_isDragging && !IsControlPressed)
				{
					Task.Run(async delegate
					{
						await Task.Delay(delay).ContinueWith(delegate
						{
							if ((Mouse.Captured == null || object.Equals(AssociatedObject, Mouse.Captured)) && _mouseLeftButtonClickInProgress)
							{
								ContextMenu contextMenu = AssociatedObject.ContextMenu;
								if (contextMenu == null || !contextMenu.IsOpen)
								{
									_mouseLeftButtonClickInProgress = false;
									ReleaseMouse();
									this.MouseLeftButtonClicked?.Invoke(this, VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs.Empty);
								}
							}
						}, scheduler);
					});
				}
			}
			else
			{
				_mouseLeftButtonClickInProgress = false;
				ReleaseMouse();
				VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs e3 = new VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs();
				this.MouseLeftButtonClicked?.Invoke(this, e3);
				handled |= e3.Handled;
			}
		}
		else if (_mouseLeftButtonDoubleClickInProgress)
		{
			_mouseLeftButtonDoubleClickInProgress = false;
			ReleaseMouse();
			VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs e4 = new VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs();
			this.MouseLeftButtonDoubleClicked?.Invoke(this, e4);
			handled |= e4.Handled;
		}
		if (_isDragging)
		{
			_isDragging = false;
			Point pointElementWasDraggedTo = GetPointElementWasDraggedTo(e);
			ref Point? mouseLeftButtonDownPoint = ref _mouseLeftButtonDownPoint;
			Point point = default(Point);
			MouseInputDraggingEventArgs e5 = new MouseInputDraggingEventArgs(mouseLeftButtonDownPoint ?? point, pointElementWasDraggedTo, ParentContainer);
			this.Dragging?.Invoke(this, e5);
			handled |= e5.Handled;
			VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs e6 = new VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs();
			this.DraggingStopped?.Invoke(this, e6);
			handled |= e6.Handled;
		}
		_mouseLeftButtonDownPoint = null;
		ReleaseMouse();
		e.Handled = MarkAllEnabledEventsAsHandled || handled;
	}

	private void Element_OnMouseMove(object sender, MouseEventArgs e)
	{
		if (e.Handled || !CanDrag)
		{
			return;
		}
		bool flag = false;
		Point position = e.GetPosition(AssociatedObject);
		if (e.LeftButton == MouseButtonState.Pressed && !_isDragging && _mouseLeftButtonDownPoint.HasValue && _mouseLeftButtonClickInProgress)
		{
			if (Math.Abs(Point.Subtract(_mouseLeftButtonDownPoint.Value, position).Length) > DragThresholdDistance)
			{
				_isDragging = true;
				VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs e2 = new VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs();
				this.DraggingStarted?.Invoke(this, e2);
				flag |= e2.Handled;
			}
		}
		else if (e.LeftButton == MouseButtonState.Released && _isDragging)
		{
			_isDragging = false;
			VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs e3 = new VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs();
			this.DraggingStopped?.Invoke(this, e3);
			flag |= e3.Handled;
		}
		if (_isDragging)
		{
			Point pointElementWasDraggedTo = GetPointElementWasDraggedTo(e);
			ref Point? mouseLeftButtonDownPoint = ref _mouseLeftButtonDownPoint;
			Point point = default(Point);
			MouseInputDraggingEventArgs e4 = new MouseInputDraggingEventArgs(mouseLeftButtonDownPoint ?? point, pointElementWasDraggedTo, ParentContainer);
			this.Dragging?.Invoke(this, e4);
			flag |= e4.Handled;
			_mouseLeftButtonClickInProgress = false;
		}
		e.Handled = MarkAllEnabledEventsAsHandled || flag;
	}

	private Point GetPointElementWasDraggedTo(MouseEventArgs e)
	{
		FrameworkElement parentContainer = ParentContainer;
		if (parentContainer != null)
		{
			Point position = e.GetPosition(parentContainer);
			if (LimitBoundsToParentContainer)
			{
				ref Point? mouseLeftButtonDownPoint = ref _mouseLeftButtonDownPoint;
				Point point = default(Point);
				double x = (mouseLeftButtonDownPoint ?? point).X;
				ref Point? mouseLeftButtonDownPoint2 = ref _mouseLeftButtonDownPoint;
				point = default(Point);
				double y = (mouseLeftButtonDownPoint2 ?? point).Y;
				double x2 = Math.Min(Math.Max(x, position.X), parentContainer.ActualWidth - AssociatedObject.ActualWidth + x);
				double y2 = Math.Min(Math.Max(y, position.Y), parentContainer.ActualHeight - AssociatedObject.ActualHeight + y);
				return new Point(x2, y2);
			}
			return position;
		}
		return e.GetPosition(null);
	}

	private void Element_OnMouseWheel(object sender, MouseWheelEventArgs e)
	{
		bool flag = false;
		if (IsMouseWheelEnabled)
		{
			MouseInputMouseWheelEventArgs e2 = new MouseInputMouseWheelEventArgs(e.Delta);
			this.MouseWheel?.Invoke(this, e2);
			flag = e2.Handled;
		}
		e.Handled = MarkAllEnabledEventsAsHandled || flag;
	}

	private bool StopLongPress()
	{
		bool result = false;
		if (_isLongPressing)
		{
			VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs e = new VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs();
			this.LongPressStopped?.Invoke(this, e);
			result = e.Handled;
		}
		_longPressTimer.Stop();
		_isLongPressing = false;
		return result;
	}

	private void LongPressTimerOnTick(object sender, EventArgs e)
	{
		_isLongPressing = true;
		this.LongPressStarted?.Invoke(this, VideoOS.UI.Common.WPF.Behaviors.Input.InputEventArgs.Empty);
		_longPressTimer.Stop();
	}
}
