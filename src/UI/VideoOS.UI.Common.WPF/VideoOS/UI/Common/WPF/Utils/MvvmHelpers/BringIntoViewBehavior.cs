using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class BringIntoViewBehavior : Behavior<ItemsControl>
{
	public static readonly RoutedCommand BringIntoViewCommand = new RoutedCommand("BringIntoView", typeof(BringIntoViewBehavior));

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.CommandBindings.Add(new CommandBinding(BringIntoViewCommand, BringIntoViewCommandExecuted));
	}

	private void BringIntoViewCommandExecuted(object sender, ExecutedRoutedEventArgs e)
	{
		int num = base.AssociatedObject.Items.IndexOf(e.Parameter);
		if (num >= 0 && base.AssociatedObject.ItemContainerGenerator.ContainerFromIndex(num) is ListBoxItem listBoxItem)
		{
			listBoxItem.BringIntoView();
			e.Handled = true;
		}
	}
}
