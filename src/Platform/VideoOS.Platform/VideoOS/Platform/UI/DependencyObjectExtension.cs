using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace VideoOS.Platform.UI;

public static class DependencyObjectExtension
{
	public static IEnumerable<T> GetAncestors<T>(this DependencyObject self, Func<T, bool> predicate, bool includeSelf) where T : DependencyObject
	{
		bool flag = true;
		DependencyObject current = self;
		while (current != null)
		{
			if ((!flag || includeSelf) && current is T val && (predicate == null || predicate(val)))
			{
				yield return val;
			}
			current = ((!(current is Visual) && !(current is Visual3D)) ? ((!(current is FrameworkElement) && !(current is FrameworkContentElement)) ? null : LogicalTreeHelper.GetParent(current)) : VisualTreeHelper.GetParent(current));
			flag = false;
		}
	}

	public static IEnumerable<T> GetAncestors<T>(this DependencyObject self) where T : DependencyObject
	{
		return self.GetAncestors<T>(null, includeSelf: false);
	}

	public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject self, Func<DependencyObject, bool> predicate, bool includeSelf)
	{
		return self.GetAncestors<DependencyObject>(predicate, includeSelf);
	}

	public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject self)
	{
		return self.GetAncestors<DependencyObject>(null, includeSelf: false);
	}

	public static IEnumerable<T> GetDescendants<T>(this DependencyObject self) where T : DependencyObject
	{
		for (int index = 0; index < VisualTreeHelper.GetChildrenCount(self); index++)
		{
			DependencyObject child = VisualTreeHelper.GetChild(self, index);
			if (child is T val)
			{
				yield return val;
			}
			foreach (T descendant in child.GetDescendants<T>())
			{
				yield return descendant;
			}
		}
	}
}
