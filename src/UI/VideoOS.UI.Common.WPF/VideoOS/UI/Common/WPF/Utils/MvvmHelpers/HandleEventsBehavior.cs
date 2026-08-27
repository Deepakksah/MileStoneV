using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class HandleEventsBehavior : Behavior<FrameworkElement>
{
	public bool HandleMouseDown { get; set; }

	public bool HandleMouseUp { get; set; }

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		if (HandleMouseDown)
		{
			base.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
		}
		if (HandleMouseUp)
		{
			base.AssociatedObject.MouseUp += AssociatedObject_MouseUp;
		}
	}

	private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
	{
		e.Handled = true;
	}

	private void AssociatedObject_MouseUp(object sender, MouseButtonEventArgs e)
	{
		e.Handled = true;
	}
}
