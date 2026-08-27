using System.Windows;
using System.Windows.Input;

namespace VideoOS.Platform.UI.Extensions;

public class UIElementExtensions
{
	private static Point? _dragInitiatedMouseDownPosition = null;

	public static readonly DependencyProperty IsDragInitiatedAwareProperty = DependencyProperty.RegisterAttached("IsDragInitiatedAware", typeof(bool), typeof(UIElementExtensions), new FrameworkPropertyMetadata(false, IsDragInitiatedAwareChanged));

	public static readonly RoutedEvent DragInitiatedEvent = EventManager.RegisterRoutedEvent("DragInitiated", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DependencyObjectAttachedProperties));

	public static void SetIsDragInitiatedAware(DependencyObject element, bool value)
	{
		element.SetValue(IsDragInitiatedAwareProperty, value);
	}

	public static bool GetIsDragInitiatedAware(DependencyObject element)
	{
		return (bool)element.GetValue(IsDragInitiatedAwareProperty);
	}

	public static void AddDragInitiatedHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
	{
		if (dependencyObject is UIElement uIElement)
		{
			uIElement.AddHandler(DragInitiatedEvent, handler);
		}
	}

	public static void RemoveDragInitiatedHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
	{
		if (dependencyObject is UIElement uIElement)
		{
			uIElement.RemoveHandler(DragInitiatedEvent, handler);
		}
	}

	private static void IsDragInitiatedAwareChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue == null)
		{
			return;
		}
		if (d is UIElement element)
		{
			if ((bool)e.NewValue)
			{
				Register(element);
			}
			else
			{
				UnRegister(element);
			}
		}
		else if (d is ContentElement element2)
		{
			if ((bool)e.NewValue)
			{
				Register(element2);
			}
			else
			{
				UnRegister(element2);
			}
		}
	}

	private static void Register(UIElement element)
	{
		element.MouseLeftButtonDown += Element_MouseLeftButtonDown;
		element.MouseMove += Element_MouseMove;
		element.MouseLeftButtonUp += Element_MouseLeftButtonUp;
	}

	private static void Register(ContentElement element)
	{
		element.MouseLeftButtonDown += Element_MouseLeftButtonDown;
		element.MouseMove += Element_MouseMove;
		element.MouseLeftButtonUp += Element_MouseLeftButtonUp;
	}

	private static void UnRegister(UIElement element)
	{
		element.MouseLeftButtonDown -= Element_MouseLeftButtonDown;
		element.MouseMove -= Element_MouseMove;
		element.MouseLeftButtonUp -= Element_MouseLeftButtonUp;
	}

	private static void UnRegister(ContentElement element)
	{
		element.MouseLeftButtonDown -= Element_MouseLeftButtonDown;
		element.MouseMove -= Element_MouseMove;
		element.MouseLeftButtonUp -= Element_MouseLeftButtonUp;
	}

	private static void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (sender is UIElement uIElement)
		{
			_dragInitiatedMouseDownPosition = uIElement.PointToScreen(e.GetPosition(uIElement));
			uIElement.CaptureMouse();
		}
	}

	private static void Element_MouseMove(object sender, MouseEventArgs e)
	{
		if (sender is UIElement uIElement && _dragInitiatedMouseDownPosition.HasValue)
		{
			Point point = uIElement.PointToScreen(Mouse.GetPosition(uIElement));
			double num = 10.0;
			double num2 = point.X - _dragInitiatedMouseDownPosition.Value.X;
			double num3 = point.Y - _dragInitiatedMouseDownPosition.Value.Y;
			if (num2 * num2 + num3 * num3 >= num * num)
			{
				_dragInitiatedMouseDownPosition = null;
				uIElement.ReleaseMouseCapture();
				uIElement.RaiseEvent(new RoutedEventArgs(DragInitiatedEvent));
			}
		}
	}

	private static void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (sender is UIElement uIElement)
		{
			_dragInitiatedMouseDownPosition = null;
			uIElement.ReleaseMouseCapture();
		}
	}
}
