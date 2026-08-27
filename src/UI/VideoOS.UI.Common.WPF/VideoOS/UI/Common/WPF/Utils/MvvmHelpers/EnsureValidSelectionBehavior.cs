using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class EnsureValidSelectionBehavior : Behavior<Selector>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.Loaded += associatedObject_Loaded;
		base.AssociatedObject.SelectionChanged += associatedObject_SelectionChanged;
	}

	private void associatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		EnsureValidSelection();
	}

	private void associatedObject_Loaded(object sender, RoutedEventArgs e)
	{
		EnsureValidSelection();
	}

	private void EnsureValidSelection()
	{
		if (base.AssociatedObject.Items.Count > 0 && base.AssociatedObject.SelectedIndex == -1)
		{
			base.AssociatedObject.SelectedIndex = 0;
		}
	}
}
