using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class MultiSelectionBehavior : Behavior<ListBox>
{
	public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(IList), typeof(MultiSelectionBehavior), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

	public IList SelectedItems
	{
		get
		{
			return (IList)((DependencyObject)(object)this).GetValue(SelectedItemsProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(SelectedItemsProperty, (object)value);
		}
	}

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		((DependencyObject)(object)this).SetCurrentValue(SelectedItemsProperty, (object)base.AssociatedObject.SelectedItems);
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		((DependencyObject)(object)this).SetCurrentValue(SelectedItemsProperty, (object)null);
	}
}
