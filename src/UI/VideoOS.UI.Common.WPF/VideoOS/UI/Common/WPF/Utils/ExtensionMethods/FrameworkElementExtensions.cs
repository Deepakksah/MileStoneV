using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

public static class FrameworkElementExtensions
{
	public static bool IsCompletelyVisibleWithin(this FrameworkElement element, FrameworkElement container)
	{
		if (container == null || !container.IsAncestorOf(element))
		{
			return false;
		}
		Rect rect = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
		return new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight).Contains(rect);
	}

	public static bool IsMouseOverVisiblePart(this FrameworkElement element)
	{
		if (element.Visibility != Visibility.Visible || !element.IsHitTestVisible)
		{
			return false;
		}
		if (element is Panel && ((Panel)element).Background == null)
		{
			return false;
		}
		if (element is Shape && ((Shape)element).Fill == null)
		{
			return false;
		}
		if (element is Control && ((Control)element).Background == null)
		{
			return false;
		}
		Point position = Mouse.GetPosition(element);
		if (position.X >= 0.0 && position.X < element.ActualWidth && position.Y >= 0.0)
		{
			return position.Y < element.ActualHeight;
		}
		return false;
	}
}
