using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class DirectionalNavigationFix : Behavior<UIElement>
{
	public bool HandlePreview { get; set; }

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		if (HandlePreview)
		{
			base.AssociatedObject.PreviewKeyDown += AssociatedObject_KeyDown;
		}
		else
		{
			base.AssociatedObject.KeyDown += AssociatedObject_KeyDown;
		}
	}

	private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
	{
		if (Keyboard.FocusedElement is UIElement uIElement)
		{
			if (e.Key == Key.Down)
			{
				uIElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
				e.Handled = true;
			}
			else if (e.Key == Key.Up)
			{
				uIElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
				e.Handled = true;
			}
		}
	}
}
