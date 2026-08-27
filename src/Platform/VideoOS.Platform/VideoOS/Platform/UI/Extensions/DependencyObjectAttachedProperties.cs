using System;
using System.Windows;
using System.Windows.Input;

namespace VideoOS.Platform.UI.Extensions;

public class DependencyObjectAttachedProperties
{
	private class MouseClickMonitor : IDisposable
	{
		private readonly DependencyObject _element;

		private bool _clickInProgress;

		private bool _handlersRegistered;

		public MouseClickMonitor(DependencyObject element)
		{
			_element = element;
			AddMouseHandlers();
			if (_element is FrameworkElement frameworkElement)
			{
				frameworkElement.Loaded += delegate
				{
					AddMouseHandlers();
				};
				frameworkElement.Unloaded += delegate
				{
					RemoveMouseHandlers();
				};
			}
			if (_element is FrameworkContentElement frameworkContentElement)
			{
				frameworkContentElement.Loaded += delegate
				{
					AddMouseHandlers();
				};
				frameworkContentElement.Unloaded += delegate
				{
					RemoveMouseHandlers();
				};
			}
		}

		private void AddMouseHandlers()
		{
			if (!_handlersRegistered)
			{
				Mouse.AddMouseDownHandler(_element, OnMouseDown);
				Mouse.AddMouseUpHandler(_element, OnMouseUp);
				Mouse.AddMouseLeaveHandler(_element, OnMouseLeave);
				_handlersRegistered = true;
			}
		}

		private void RemoveMouseHandlers()
		{
			if (_handlersRegistered)
			{
				Mouse.RemoveMouseDownHandler(_element, OnMouseDown);
				Mouse.RemoveMouseUpHandler(_element, OnMouseUp);
				Mouse.RemoveMouseLeaveHandler(_element, OnMouseLeave);
				_handlersRegistered = false;
			}
		}

		private void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left && e.ClickCount == 1)
			{
				_clickInProgress = true;
			}
		}

		private void OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			if (_clickInProgress && e.ChangedButton == MouseButton.Left)
			{
				if (_element is FrameworkElement frameworkElement)
				{
					frameworkElement.RaiseEvent(new RoutedEventArgs(MouseLeftButtonClickEvent));
				}
				else if (_element is FrameworkContentElement frameworkContentElement)
				{
					frameworkContentElement.RaiseEvent(new RoutedEventArgs(MouseLeftButtonClickEvent));
				}
			}
		}

		private void OnMouseLeave(object sender, MouseEventArgs e)
		{
			_clickInProgress = false;
		}

		public void Dispose()
		{
			Mouse.RemoveMouseDownHandler(_element, OnMouseDown);
			Mouse.RemoveMouseUpHandler(_element, OnMouseUp);
			Mouse.RemoveMouseLeaveHandler(_element, OnMouseLeave);
		}
	}

	public static readonly DependencyProperty IsMouseLeftButtonDownAwareProperty = DependencyProperty.RegisterAttached("IsMouseLeftButtonDownAware", typeof(bool), typeof(DependencyObjectAttachedProperties), new FrameworkPropertyMetadata(false, IsMouseDownAwareChanged));

	public static readonly DependencyProperty IsMouseLeftButtonDownProperty = DependencyProperty.RegisterAttached("IsMouseLeftButtonDown", typeof(bool), typeof(DependencyObjectAttachedProperties), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

	private static readonly DependencyProperty MouseClickMonitorProperty = DependencyProperty.RegisterAttached("MouseClickMonitor", typeof(MouseClickMonitor), typeof(DependencyObjectAttachedProperties), new PropertyMetadata((object)null));

	public static readonly RoutedEvent MouseLeftButtonClickEvent = EventManager.RegisterRoutedEvent("MouseLeftButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DependencyObjectAttachedProperties));

	public static void SetIsMouseLeftButtonDownAware(DependencyObject element, bool value)
	{
		element.SetValue(IsMouseLeftButtonDownAwareProperty, value);
	}

	public static bool GetIsMouseLeftButtonDownAware(DependencyObject element)
	{
		return (bool)element.GetValue(IsMouseLeftButtonDownAwareProperty);
	}

	private static void IsMouseDownAwareChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
		element.PreviewMouseLeftButtonDown += Element_MouseLeftButtonDown;
		element.MouseLeave += Element_MouseLeave;
		element.PreviewMouseUp += Element_MouseUp;
	}

	private static void Register(ContentElement element)
	{
		element.PreviewMouseLeftButtonDown += Element_MouseLeftButtonDown;
		element.MouseLeave += Element_MouseLeave;
		element.PreviewMouseUp += Element_MouseUp;
	}

	private static void UnRegister(UIElement element)
	{
		element.PreviewMouseLeftButtonDown -= Element_MouseLeftButtonDown;
		element.MouseLeave -= Element_MouseLeave;
		element.PreviewMouseUp -= Element_MouseUp;
	}

	private static void UnRegister(ContentElement element)
	{
		element.PreviewMouseLeftButtonDown -= Element_MouseLeftButtonDown;
		element.MouseLeave -= Element_MouseLeave;
		element.PreviewMouseUp -= Element_MouseUp;
	}

	private static void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (sender is DependencyObject element)
		{
			SetIsMouseLeftButtonDown(element, value: true);
		}
	}

	private static void Element_MouseLeave(object sender, MouseEventArgs e)
	{
		if (sender is DependencyObject element)
		{
			SetIsMouseLeftButtonDown(element, value: false);
		}
	}

	private static void Element_MouseUp(object sender, MouseButtonEventArgs e)
	{
		if (sender is DependencyObject element)
		{
			SetIsMouseLeftButtonDown(element, value: false);
		}
	}

	public static void SetIsMouseLeftButtonDown(DependencyObject element, bool value)
	{
		element.SetValue(IsMouseLeftButtonDownProperty, value);
	}

	public static bool GetIsMouseLeftButtonDown(DependencyObject element)
	{
		return (bool)element.GetValue(IsMouseLeftButtonDownProperty);
	}

	private static void SetMouseClickMonitor(DependencyObject element, MouseClickMonitor value)
	{
		element.SetValue(MouseClickMonitorProperty, value);
	}

	private static MouseClickMonitor GetMouseClickMonitor(DependencyObject element)
	{
		return (MouseClickMonitor)element.GetValue(MouseClickMonitorProperty);
	}

	public static void AddMouseLeftButtonClickHandler(DependencyObject d, RoutedEventHandler handler)
	{
		bool flag = false;
		if (d is FrameworkElement frameworkElement)
		{
			frameworkElement.AddHandler(MouseLeftButtonClickEvent, handler);
			flag = true;
		}
		if (d is FrameworkContentElement frameworkContentElement)
		{
			frameworkContentElement.AddHandler(MouseLeftButtonClickEvent, handler);
			flag = true;
		}
		if (flag)
		{
			SetMouseClickMonitor(d, new MouseClickMonitor(d));
		}
	}

	public static void RemoveMouseLeftButtonClickHandler(DependencyObject d, RoutedEventHandler handler)
	{
		bool flag = false;
		if (d is FrameworkElement frameworkElement)
		{
			frameworkElement.RemoveHandler(MouseLeftButtonClickEvent, handler);
			flag = true;
		}
		if (d is FrameworkContentElement frameworkContentElement)
		{
			frameworkContentElement.RemoveHandler(MouseLeftButtonClickEvent, handler);
			flag = true;
		}
		if (flag)
		{
			GetMouseClickMonitor(d)?.Dispose();
			SetMouseClickMonitor(d, null);
		}
	}
}
