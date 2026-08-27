using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.Mobile.UI.Behaviors;

public class BindableSelectedItemBehavior : Behavior<TreeView>
{
	public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(BindableSelectedItemBehavior), new UIPropertyMetadata(null, OnSelectedItemChanged));

	public object SelectedItem
	{
		get
		{
			return ((DependencyObject)(object)this).GetValue(SelectedItemProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(SelectedItemProperty, value);
		}
	}

	private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is TreeViewItem treeViewItem)
		{
			treeViewItem.SetValue(TreeViewItem.IsSelectedProperty, true);
		}
	}

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		if (base.AssociatedObject != null)
		{
			base.AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
		}
	}

	private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		SelectedItem = e.NewValue;
	}
}
