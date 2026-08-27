using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public sealed class IgnoreMouseWheelBehavior : Behavior<UIElement>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;
	}

	protected override void OnDetaching()
	{
		base.AssociatedObject.PreviewMouseWheel -= AssociatedObject_PreviewMouseWheel;
		((Behavior)this).OnDetaching();
	}

	private void AssociatedObject_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
	{
		e.Handled = true;
		MouseWheelEventArgs e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
		{
			RoutedEvent = UIElement.MouseWheelEvent
		};
		base.AssociatedObject.RaiseEvent(e2);
	}
}
