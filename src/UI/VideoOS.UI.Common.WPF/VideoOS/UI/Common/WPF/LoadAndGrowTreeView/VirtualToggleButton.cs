using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace VideoOS.UI.Common.WPF.LoadAndGrowTreeView;

public static class VirtualToggleButton
{
	public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.RegisterAttached("IsChecked", typeof(bool?), typeof(VirtualToggleButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, OnIsCheckedChanged));

	public static readonly DependencyProperty IsThreeStateProperty = DependencyProperty.RegisterAttached("IsThreeState", typeof(bool), typeof(VirtualToggleButton), new FrameworkPropertyMetadata(false));

	public static readonly DependencyProperty IsVirtualToggleButtonProperty = DependencyProperty.RegisterAttached("IsVirtualToggleButton", typeof(bool), typeof(VirtualToggleButton), new FrameworkPropertyMetadata(false, OnIsVirtualToggleButtonChanged));

	public static bool? GetIsChecked(DependencyObject dependencyObject)
	{
		return (bool?)dependencyObject.GetValue(IsCheckedProperty);
	}

	public static void SetIsChecked(DependencyObject dependencyObject, bool? value)
	{
		dependencyObject.SetValue(IsCheckedProperty, value);
	}

	private static void OnIsCheckedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
	{
		if (dependencyObject is UIElement target)
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

	public static bool GetIsThreeState(DependencyObject dependencyObject)
	{
		return (bool)dependencyObject.GetValue(IsThreeStateProperty);
	}

	public static void SetIsThreeState(DependencyObject dependencyObject, bool value)
	{
		dependencyObject.SetValue(IsThreeStateProperty, value);
	}

	public static bool GetIsVirtualToggleButton(DependencyObject dependencyObject)
	{
		return (bool)dependencyObject.GetValue(IsVirtualToggleButtonProperty);
	}

	public static void SetIsVirtualToggleButton(DependencyObject dependencyObject, bool value)
	{
		dependencyObject.SetValue(IsVirtualToggleButtonProperty, value);
	}

	private static void OnIsVirtualToggleButtonChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
	{
		if (dependencyObject is IInputElement inputElement)
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
		RoutedEventArgs e = new RoutedEventArgs();
		e.RoutedEvent = ToggleButton.CheckedEvent;
		RaiseEvent(target, e);
		return e;
	}

	public static RoutedEventArgs RaiseUncheckedEvent(UIElement target)
	{
		if (target == null)
		{
			return null;
		}
		RoutedEventArgs e = new RoutedEventArgs();
		e.RoutedEvent = ToggleButton.UncheckedEvent;
		RaiseEvent(target, e);
		return e;
	}

	public static RoutedEventArgs RaiseIndeterminateEvent(UIElement target)
	{
		if (target == null)
		{
			return null;
		}
		RoutedEventArgs e = new RoutedEventArgs();
		e.RoutedEvent = ToggleButton.IndeterminateEvent;
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
		DependencyObject dependencyObject = sender as DependencyObject;
		if (e.OriginalSource != sender)
		{
			return;
		}
		if (e.Key == Key.Space)
		{
			if ((Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.Alt)
			{
				UpdateIsChecked(dependencyObject);
				e.Handled = true;
			}
		}
		else if (e.Key == Key.Return && (bool)dependencyObject.GetValue(KeyboardNavigation.AcceptsReturnProperty))
		{
			UpdateIsChecked(dependencyObject);
			e.Handled = true;
		}
	}

	private static void UpdateIsChecked(DependencyObject dependencyObject)
	{
		bool? isChecked = GetIsChecked(dependencyObject);
		if (isChecked == true)
		{
			SetIsChecked(dependencyObject, GetIsThreeState(dependencyObject) ? ((bool?)null) : new bool?(false));
		}
		else
		{
			SetIsChecked(dependencyObject, isChecked.HasValue);
		}
	}

	private static void RaiseEvent(DependencyObject target, RoutedEventArgs args)
	{
		if (target is UIElement uIElement)
		{
			uIElement.RaiseEvent(args);
		}
		else if (target is ContentElement contentElement)
		{
			contentElement.RaiseEvent(args);
		}
	}
}
