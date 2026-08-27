using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace VideoOS.Mobile.UI.Behaviors;

public static class VirtualToggleButton
{
	public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.RegisterAttached("IsChecked", typeof(bool?), typeof(VirtualToggleButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, OnIsCheckedChanged));

	public static readonly DependencyProperty IsThreeStateProperty = DependencyProperty.RegisterAttached("IsThreeState", typeof(bool), typeof(VirtualToggleButton), new FrameworkPropertyMetadata(false));

	public static readonly DependencyProperty IsVirtualToggleButtonProperty = DependencyProperty.RegisterAttached("IsVirtualToggleButton", typeof(bool), typeof(VirtualToggleButton), new FrameworkPropertyMetadata(false, OnIsVirtualToggleButtonChanged));

	public static bool? GetIsChecked(DependencyObject d)
	{
		return (bool?)d.GetValue(IsCheckedProperty);
	}

	public static void SetIsChecked(DependencyObject d, bool? value)
	{
		d.SetValue(IsCheckedProperty, value);
	}

	private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is UIElement target)
		{
			bool? flag = (bool?)e.NewValue;
			if (flag == true)
			{
				RaiseCheckedEvent(target);
			}
			else if (flag == false)
			{
				RaiseUncheckedEvent(target);
			}
			else
			{
				RaiseIndeterminateEvent(target);
			}
		}
	}

	public static bool GetIsThreeState(DependencyObject d)
	{
		return (bool)d.GetValue(IsThreeStateProperty);
	}

	public static void SetIsThreeState(DependencyObject d, bool value)
	{
		d.SetValue(IsThreeStateProperty, value);
	}

	public static bool GetIsVirtualToggleButton(DependencyObject d)
	{
		return (bool)d.GetValue(IsVirtualToggleButtonProperty);
	}

	public static void SetIsVirtualToggleButton(DependencyObject d, bool value)
	{
		d.SetValue(IsVirtualToggleButtonProperty, value);
	}

	private static void OnIsVirtualToggleButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is IInputElement inputElement)
		{
			if ((bool)e.NewValue)
			{
				inputElement.MouseLeftButtonDown += OnMouseLeftButtonDown;
				inputElement.KeyDown += OnKeyDown;
			}
			else
			{
				inputElement.MouseLeftButtonDown -= OnMouseLeftButtonDown;
				inputElement.KeyDown -= OnKeyDown;
			}
		}
	}

	public static RoutedEventArgs RaiseCheckedEvent(UIElement target)
	{
		if (target == null)
		{
			return null;
		}
		RoutedEventArgs e = new RoutedEventArgs
		{
			RoutedEvent = ToggleButton.CheckedEvent
		};
		RaiseEvent(target, e);
		return e;
	}

	public static RoutedEventArgs RaiseUncheckedEvent(UIElement target)
	{
		if (target == null)
		{
			return null;
		}
		RoutedEventArgs e = new RoutedEventArgs
		{
			RoutedEvent = ToggleButton.UncheckedEvent
		};
		RaiseEvent(target, e);
		return e;
	}

	public static RoutedEventArgs RaiseIndeterminateEvent(UIElement target)
	{
		if (target == null)
		{
			return null;
		}
		RoutedEventArgs e = new RoutedEventArgs
		{
			RoutedEvent = ToggleButton.IndeterminateEvent
		};
		RaiseEvent(target, e);
		return e;
	}

	private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		e.Handled = true;
		UpdateIsChecked(sender as DependencyObject);
	}

	private static void OnKeyDown(object sender, KeyEventArgs e)
	{
		if (e.OriginalSource != sender)
		{
			return;
		}
		if (e.Key == Key.Space)
		{
			if ((Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.Alt)
			{
				UpdateIsChecked(sender as DependencyObject);
				e.Handled = true;
			}
		}
		else if (sender is DependencyObject dependencyObject && e.Key == Key.Return && (bool)dependencyObject.GetValue(KeyboardNavigation.AcceptsReturnProperty))
		{
			UpdateIsChecked(dependencyObject);
			e.Handled = true;
		}
	}

	private static void UpdateIsChecked(DependencyObject d)
	{
		bool? isChecked = GetIsChecked(d);
		if (isChecked == true)
		{
			SetIsChecked(d, GetIsThreeState(d) ? ((bool?)null) : new bool?(false));
		}
		else
		{
			SetIsChecked(d, isChecked.HasValue);
		}
	}

	private static void RaiseEvent(DependencyObject target, RoutedEventArgs args)
	{
		if (target is UIElement uIElement)
		{
			uIElement.RaiseEvent(args);
		}
		else
		{
			(target as ContentElement)?.RaiseEvent(args);
		}
	}
}
