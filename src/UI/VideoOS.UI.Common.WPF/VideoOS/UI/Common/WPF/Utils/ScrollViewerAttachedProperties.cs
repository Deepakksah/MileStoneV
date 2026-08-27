using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.Utils;

public static class ScrollViewerAttachedProperties
{
	public static readonly DependencyProperty ShiftWheelScrollsHorizontallyProperty = DependencyProperty.RegisterAttached("ShiftWheelScrollsHorizontally", typeof(bool), typeof(ScrollViewerAttachedProperties), new PropertyMetadata(false, UseHorizontalScrollingChangedCallback));

	private static void UseHorizontalScrollingChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (!(d is UIElement uIElement))
		{
			throw new Exception("Attached property must be used with UIElement.");
		}
		if ((bool)e.NewValue)
		{
			uIElement.PreviewMouseWheel += OnPreviewMouseWheel;
		}
		else
		{
			uIElement.PreviewMouseWheel -= OnPreviewMouseWheel;
		}
	}

	private static void OnPreviewMouseWheel(object sender, MouseWheelEventArgs args)
	{
		ScrollViewer scrollViewer = (sender as UIElement).FindDescendant<ScrollViewer>();
		if (scrollViewer != null && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
		{
			if (args.Delta < 0)
			{
				scrollViewer.LineRight();
			}
			else
			{
				scrollViewer.LineLeft();
			}
			args.Handled = true;
		}
	}

	public static void SetShiftWheelScrollsHorizontally(UIElement element, bool value)
	{
		element.SetValue(ShiftWheelScrollsHorizontallyProperty, value);
	}

	public static bool GetShiftWheelScrollsHorizontally(UIElement element)
	{
		return (bool)element.GetValue(ShiftWheelScrollsHorizontallyProperty);
	}

	private static T FindDescendant<T>(this DependencyObject d) where T : DependencyObject
	{
		if (d == null)
		{
			return null;
		}
		if (d is T)
		{
			return (T)d;
		}
		int childrenCount = VisualTreeHelper.GetChildrenCount(d);
		for (int i = 0; i < childrenCount; i++)
		{
			DependencyObject child = VisualTreeHelper.GetChild(d, i);
			T val = (child as T) ?? child.FindDescendant<T>();
			if (val != null)
			{
				return val;
			}
		}
		return null;
	}
}
