using System.Windows;
using System.Windows.Media;

namespace VideoOS.Platform.UI.Extensions;

public static class DependencyObjectExtensionMethods
{
	public static T FindDescendant<T>(this DependencyObject self) where T : DependencyObject
	{
		if (self == null)
		{
			return null;
		}
		int childrenCount = VisualTreeHelper.GetChildrenCount(self);
		for (int i = 0; i < childrenCount; i++)
		{
			DependencyObject child = VisualTreeHelper.GetChild(self, i);
			T val = (child as T) ?? child.FindDescendant<T>();
			if (val != null)
			{
				return val;
			}
		}
		return null;
	}

	public static T FindAncestor<T>(this DependencyObject self, bool allowFallbackToLogicalTree = true) where T : DependencyObject
	{
		if (self == null)
		{
			return null;
		}
		DependencyObject parent = VisualTreeHelper.GetParent(self);
		if (parent == null && allowFallbackToLogicalTree && self is FrameworkElement frameworkElement)
		{
			parent = frameworkElement.Parent;
		}
		T val = (parent as T) ?? parent.FindAncestor<T>();
		if (val != null)
		{
			return val;
		}
		return null;
	}

	public static object FindResource(this DependencyObject self, object resourceKey)
	{
		if (self is FrameworkElement frameworkElement)
		{
			return frameworkElement.FindResource(resourceKey);
		}
		if (self is FrameworkContentElement frameworkContentElement)
		{
			return frameworkContentElement.FindResource(resourceKey);
		}
		return null;
	}

	public static bool IsDefaultValueApplied(this DependencyObject self, DependencyProperty property)
	{
		return DependencyPropertyHelper.GetValueSource(self, property).BaseValueSource == BaseValueSource.Default;
	}
}
