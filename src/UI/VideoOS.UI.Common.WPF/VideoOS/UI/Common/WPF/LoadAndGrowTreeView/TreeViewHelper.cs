using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.LoadAndGrowTreeView;

public class TreeViewHelper
{
	public TreeViewItem FindTreeViewItem(object searchStart)
	{
		TreeViewItem result = null;
		if (searchStart is FrameworkElement frameworkElement)
		{
			if (frameworkElement is TreeViewItem)
			{
				return frameworkElement as TreeViewItem;
			}
			for (FrameworkElement frameworkElement2 = VisualTreeHelper.GetParent(frameworkElement) as FrameworkElement; frameworkElement2 != null; frameworkElement2 = VisualTreeHelper.GetParent(frameworkElement2) as FrameworkElement)
			{
				if (frameworkElement2 is TreeViewItem)
				{
					return frameworkElement2 as TreeViewItem;
				}
			}
		}
		return result;
	}
}
