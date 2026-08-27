using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class ButtonDropDownBehavior : Behavior<ComboBox>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
	}

	private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (base.AssociatedObject.SelectedValue is ICommand command && command.CanExecute(null))
		{
			command.Execute(null);
		}
	}
}
