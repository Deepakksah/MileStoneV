using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

public static class DependencyObjectExtensions
{
	public static T GetVisualParent<T>(this DependencyObject obj) where T : DependencyObject
	{
		DependencyObject dependencyObject;
		for (dependencyObject = obj; dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(dependencyObject))
		{
			if (dependencyObject.GetType() == typeof(T))
			{
				return dependencyObject as T;
			}
		}
		return dependencyObject as T;
	}

	public static IEnumerable<T> GetChildren<T>(this DependencyObject obj) where T : DependencyObject
	{
		for (int index = 0; index < VisualTreeHelper.GetChildrenCount(obj); index++)
		{
			if (VisualTreeHelper.GetChild(obj, index) is T val)
			{
				yield return val;
			}
		}
		for (int index = 0; index < VisualTreeHelper.GetChildrenCount(obj); index++)
		{
			DependencyObject child = VisualTreeHelper.GetChild(obj, index);
			foreach (T child2 in child.GetChildren<T>())
			{
				yield return child2;
			}
		}
	}
}
