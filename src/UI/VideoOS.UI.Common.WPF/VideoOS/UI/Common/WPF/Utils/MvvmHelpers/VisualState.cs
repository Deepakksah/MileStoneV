using System;
using System.Windows;
using System.Windows.Controls;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class VisualState : DependencyObject
{
	public static readonly DependencyProperty ElementStateProperty = DependencyProperty.RegisterAttached("ElementState", typeof(string), typeof(VisualState), new PropertyMetadata(null, OnElementStateChanged));

	public static readonly DependencyProperty ControlStateProperty = DependencyProperty.RegisterAttached("ControlState", typeof(string), typeof(VisualState), new PropertyMetadata(null, OnControlStateChanged));

	public static void SetElementState(DependencyObject instance, string value)
	{
		instance.SetValue(ElementStateProperty, value);
	}

	private static void OnElementStateChanged(DependencyObject instance, DependencyPropertyChangedEventArgs e)
	{
		if (!(instance is FrameworkElement stateGroupsRoot))
		{
			throw new InvalidOperationException("Property 'ElementState' can only be attached to objects of type 'FrameworkElement'.");
		}
		if (e.NewValue != null)
		{
			VisualStateManager.GoToElementState(stateGroupsRoot, e.NewValue.ToString(), useTransitions: true);
		}
	}

	public static void SetControlState(DependencyObject instance, string value)
	{
		instance.SetValue(ControlStateProperty, value);
	}

	private static void OnControlStateChanged(DependencyObject instance, DependencyPropertyChangedEventArgs e)
	{
		Control control = instance as Control;
		if (control == null)
		{
			throw new InvalidOperationException("Property 'ControlState' can only be attached to objects of type 'Control'.");
		}
		if (e.NewValue == null)
		{
			return;
		}
		if (control.IsLoaded)
		{
			VisualStateManager.GoToState(control, e.NewValue.ToString(), useTransitions: true);
			return;
		}
		control.Loaded += delegate
		{
			VisualStateManager.GoToState(control, e.NewValue.ToString(), useTransitions: true);
		};
	}
}
