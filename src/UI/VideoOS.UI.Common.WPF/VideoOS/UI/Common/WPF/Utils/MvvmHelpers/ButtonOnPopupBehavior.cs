using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class ButtonOnPopupBehavior : Behavior<ButtonBase>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.Click += AssociatedObject_Click;
	}

	private void AssociatedObject_Click(object sender, RoutedEventArgs e)
	{
		Popup popup = (from ancestor in base.AssociatedObject.GetAncestors()
			select LogicalTreeHelper.GetParent(ancestor)).OfType<Popup>().FirstOrDefault();
		if (popup != null)
		{
			popup.IsOpen = false;
		}
	}
}
