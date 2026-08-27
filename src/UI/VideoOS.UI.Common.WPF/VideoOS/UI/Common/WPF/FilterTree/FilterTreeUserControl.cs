using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.FilterTree;

public partial class FilterTreeUserControl : UserControl, IComponentConnector
{
	public static DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(FilterTreeItem), typeof(FilterTreeUserControl), new FrameworkPropertyMetadata(null, SelectedItemPropertyChanged));

	public FilterTreeItem SelectedItem
	{
		get
		{
			return (FilterTreeItem)GetValue(SelectedItemProperty);
		}
		set
		{
			SetValue(SelectedItemProperty, value);
		}
	}

	public bool AutoSelectRoot { get; set; }

	public event RoutedPropertyChangedEventHandler<object> SelectedItemChanged;

	public FilterTreeUserControl()
	{
		InitializeComponent();
		AutoSelectRoot = true;
	}

	private static void SelectedItemPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
	{
		FilterTreeUserControl obj2 = obj as FilterTreeUserControl;
		obj2.SelectFilterTreeViewItem(e.NewValue as FilterTreeItem);
		obj2.OnSelectedItemChanged(new RoutedPropertyChangedEventArgs<object>(e.OldValue, e.NewValue));
	}

	private void _filterTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		SetCurrentValue(SelectedItemProperty, e.NewValue as FilterTreeItem);
	}

	protected virtual void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
	{
		this.SelectedItemChanged?.Invoke(this, e);
	}

	private void UserControl_Loaded(object sender, RoutedEventArgs e)
	{
		_filterTreeView.Focus();
		if (AutoSelectRoot && _filterTreeView.SelectedItem == null && _filterTreeView.ItemContainerGenerator.ContainerFromIndex(0) is TreeViewItem treeViewItem)
		{
			treeViewItem.IsExpanded = true;
			treeViewItem.Focus();
		}
	}

	protected void SelectFilterTreeViewItem(FilterTreeItem item)
	{
		bool flag = false;
		if (item != null)
		{
			flag = SelectFilterTreeViewItem(_filterTreeView.Items, _filterTreeView.ItemContainerGenerator, item);
		}
		if (!flag)
		{
			ClearTreeViewItemsControlSelection(_filterTreeView.Items, _filterTreeView.ItemContainerGenerator);
		}
	}

	private static bool SelectFilterTreeViewItem(ItemCollection ic, ItemContainerGenerator icg, FilterTreeItem item)
	{
		for (int i = 0; i < ic.Count; i++)
		{
			if (icg.ContainerFromItem(item) is TreeViewItem treeViewItem)
			{
				treeViewItem.IsSelected = true;
				return true;
			}
			if (icg.ContainerFromIndex(i) is TreeViewItem treeViewItem2 && SelectFilterTreeViewItem(treeViewItem2.Items, treeViewItem2.ItemContainerGenerator, item))
			{
				return true;
			}
		}
		return false;
	}

	private static void ClearTreeViewItemsControlSelection(ItemCollection ic, ItemContainerGenerator icg)
	{
		for (int i = 0; i < ic.Count; i++)
		{
			if (icg.ContainerFromIndex(i) is TreeViewItem treeViewItem)
			{
				ClearTreeViewItemsControlSelection(treeViewItem.Items, treeViewItem.ItemContainerGenerator);
				treeViewItem.IsSelected = false;
			}
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
