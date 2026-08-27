using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using VideoOS.UI.Common.WPF.Resources;
using VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

namespace VideoOS.UI.Common.WPF.LoadAndGrowTreeView;

public partial class LoadAndGrowTreeView : TreeView, IComponentConnector, IStyleConnector
{
	public delegate List<TreeViewItemData> ItemHasBeenExpandedHandler(object sender, ItemHasBeenExpandedSynchronousArgs args);

	public delegate void ItemHasBeenExpandedHandler2(object sender, ItemHasBeenExpandedSynchronousArgs args);

	public delegate void ItemASyncHasBeenExpandedHandler(object sender, ItemHasBeenExpandedASynchronousArgs args);

	public delegate void SelectedItemsDraggedHandler(object sender, SelectedItemsDraggedArgs args);

	public delegate void PreviewItemCollapseHandler(object sender, PreviewItemCollapseArgs args);

	public delegate void ItemDoubleClickedHandler(object sender, ItemDoubleClickedArgs args);

	private bool _isSynchronous = true;

	private Point _dragDropOrigin;

	private object _dragObject;

	private readonly List<TreeViewItem> _selectedItems = new List<TreeViewItem>();

	private bool _selectNodeOnDrag = true;

	private bool _isDragging = true;

	private TreeViewItem _dragSelectedItem;

	private TreeViewItem _lastSelectedItem;

	private TreeViewItem _currentSelectedItem;

	private bool _dragSelectedItemLocked;

	public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(LoadAndGrowTreeView), new PropertyMetadata(null));

	public static readonly DependencyProperty IsMultiselectProperty = DependencyProperty.Register("IsMultiselect", typeof(bool), typeof(LoadAndGrowTreeView), new PropertyMetadata(false));

	public static readonly DependencyProperty IsMultiSelectedProperty = DependencyProperty.RegisterAttached("IsMultiSelected", typeof(bool), typeof(LoadAndGrowTreeView), new PropertyMetadata(false));

	public static readonly DependencyProperty ItemContainerTemplateProperty = DependencyProperty.Register("ItemContainerTemplate", typeof(ControlTemplate), typeof(LoadAndGrowTreeView), new PropertyMetadata(null, OnItemContainerTemplateChanged));

	public static readonly DependencyProperty IsInvalidSelectionProperty = DependencyProperty.RegisterAttached("IsInvalidSelection", typeof(bool), typeof(LoadAndGrowTreeView), new PropertyMetadata(false));

	public static readonly DependencyProperty IsSecondaryKeyboardSelectionProperty = DependencyProperty.RegisterAttached("IsSecondaryKeyboardSelection", typeof(bool), typeof(LoadAndGrowTreeView), new PropertyMetadata(false));

	public static readonly DependencyProperty IsContextMenuOpeningInTreeProperty = DependencyProperty.RegisterAttached("IsContextMenuOpeningInTree", typeof(bool), typeof(LoadAndGrowTreeView), new PropertyMetadata(false));

	public static readonly DependencyProperty IsDragTargetInTreeProperty = DependencyProperty.RegisterAttached("IsDragTargetInTree", typeof(bool), typeof(LoadAndGrowTreeView), new PropertyMetadata(false));

	public static readonly DependencyProperty HorizontalMouseWheelScrollingEnabledProperty = DependencyProperty.Register("HorizontalMouseWheelScrollingEnabled", typeof(bool), typeof(LoadAndGrowTreeView), new PropertyMetadata(true));

	public DataTemplate HeaderTemplate
	{
		get
		{
			return (DataTemplate)GetValue(HeaderTemplateProperty);
		}
		set
		{
			SetValue(HeaderTemplateProperty, value);
		}
	}

	public bool IsMultiSelect
	{
		get
		{
			return (bool)GetValue(IsMultiselectProperty);
		}
		set
		{
			SetValue(IsMultiselectProperty, value);
		}
	}

	public ControlTemplate ItemContainerTemplate
	{
		get
		{
			return (ControlTemplate)GetValue(ItemContainerTemplateProperty);
		}
		set
		{
			SetValue(ItemContainerTemplateProperty, value);
		}
	}

	public bool IgnoreMouseDrag { get; set; }

	public bool HorizontalMouseWheelScrollingEnabled
	{
		get
		{
			return (bool)GetValue(HorizontalMouseWheelScrollingEnabledProperty);
		}
		set
		{
			SetValue(HorizontalMouseWheelScrollingEnabledProperty, value);
		}
	}

	[Obsolete("This method is deprecated use SelectedTreeViewItem instead.")]
	public TreeViewItem LastSelectedItem
	{
		get
		{
			return _lastSelectedItem;
		}
		private set
		{
			_lastSelectedItem = value;
		}
	}

	public TreeViewItem SelectedTreeViewItem => FindVisualChild(this);

	public List<TreeViewItemData> SelectedItems
	{
		get
		{
			List<TreeViewItemData> list = new List<TreeViewItemData>();
			foreach (TreeViewItem selectedItem in _selectedItems)
			{
				if (VisualTreeHelper.GetParent(selectedItem) != null && selectedItem.DataContext is TreeViewItemData item)
				{
					list.Add(item);
				}
			}
			return list;
		}
	}

	public bool IsSynchronous
	{
		get
		{
			return _isSynchronous;
		}
		set
		{
			_isSynchronous = value;
		}
	}

	public bool SelectNodeOnDrag
	{
		get
		{
			return _selectNodeOnDrag;
		}
		set
		{
			_selectNodeOnDrag = value;
		}
	}

	[Obsolete("Use instead ItemHasBeenExpandedSynchronousEvent2.")]
	public event ItemHasBeenExpandedHandler ItemHasBeenExpandedSynchronousEvent;

	public event ItemHasBeenExpandedHandler2 ItemHasBeenExpandedSynchronousEvent2;

	public event ItemASyncHasBeenExpandedHandler ItemHasBeenExpandedASynchronousEvent;

	public event SelectedItemsDraggedHandler SelectedItemsDraggedEvent;

	public event PreviewItemCollapseHandler PreviewItemCollapseEvent;

	public event ItemDoubleClickedHandler ItemDoubleClickedEvent;

	private List<TreeViewItemData> FireItemHasBeenExpandedHandler(TreeViewItemData itemData)
	{
		if (this.ItemHasBeenExpandedSynchronousEvent != null)
		{
			return this.ItemHasBeenExpandedSynchronousEvent(this, new ItemHasBeenExpandedSynchronousArgs(itemData));
		}
		return null;
	}

	protected virtual List<TreeViewItemData> FireItemHasBeenExpandedHandler2(TreeViewItemData itemData)
	{
		ItemHasBeenExpandedSynchronousArgs itemHasBeenExpandedSynchronousArgs = new ItemHasBeenExpandedSynchronousArgs(itemData);
		this.ItemHasBeenExpandedSynchronousEvent2?.Invoke(this, itemHasBeenExpandedSynchronousArgs);
		return itemHasBeenExpandedSynchronousArgs.ChildItemDatas;
	}

	private void FireASyncItemHasBeenExpandedHandler(TreeViewItemData itemData)
	{
		if (this.ItemHasBeenExpandedASynchronousEvent != null)
		{
			this.ItemHasBeenExpandedASynchronousEvent(this, new ItemHasBeenExpandedASynchronousArgs(itemData, AsyncItemExpansion));
		}
	}

	private void FireSelectedItemsDragged(List<TreeViewItemData> datas)
	{
		_isDragging = true;
		if (this.SelectedItemsDraggedEvent != null)
		{
			this.SelectedItemsDraggedEvent(this, new SelectedItemsDraggedArgs(datas));
		}
	}

	private void FirePreviewItemCollapse(TreeViewItemData item)
	{
		if (this.PreviewItemCollapseEvent != null)
		{
			this.PreviewItemCollapseEvent(this, new PreviewItemCollapseArgs(item));
		}
	}

	private void FireItemDoubleClicked(TreeViewItemData item)
	{
		if (this.ItemDoubleClickedEvent != null)
		{
			this.ItemDoubleClickedEvent(this, new ItemDoubleClickedArgs(item));
		}
	}

	public static bool GetIsMultiSelected(DependencyObject obj)
	{
		return (bool)obj.GetValue(IsMultiSelectedProperty);
	}

	public static void SetIsMultiSelected(DependencyObject obj, bool value)
	{
		obj.SetValue(IsMultiSelectedProperty, value);
	}

	private static void OnItemContainerTemplateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue != null && obj is LoadAndGrowTreeView loadAndGrowTreeView)
		{
			loadAndGrowTreeView.ItemContainerStyle = loadAndGrowTreeView.FindResource("TreeViewItemStyle") as Style;
		}
	}

	public static bool GetIsInvalidSelection(DependencyObject obj)
	{
		return (bool)obj.GetValue(IsInvalidSelectionProperty);
	}

	public static void SetIsInvalidSelection(DependencyObject obj, bool value)
	{
		obj.SetValue(IsInvalidSelectionProperty, value);
	}

	public static bool GetIsSecondaryKeyboardSelection(DependencyObject depObj)
	{
		return (bool)depObj.GetValue(IsSecondaryKeyboardSelectionProperty);
	}

	public static void SetIsSecondaryKeyboardSelection(DependencyObject depObj, bool value)
	{
		depObj.SetValue(IsSecondaryKeyboardSelectionProperty, value);
	}

	public static bool GetIsContextMenuOpeningInTree(DependencyObject depObj)
	{
		return (bool)depObj.GetValue(IsContextMenuOpeningInTreeProperty);
	}

	public static void SetIsContextMenuOpeningInTree(DependencyObject depObj, bool value)
	{
		depObj.SetValue(IsContextMenuOpeningInTreeProperty, value);
	}

	public static bool GetIsDragTargetInTree(DependencyObject depObj)
	{
		return (bool)depObj.GetValue(IsDragTargetInTreeProperty);
	}

	public static void SetIsDragTargetInTree(DependencyObject depObj, bool value)
	{
		depObj.SetValue(IsDragTargetInTreeProperty, value);
	}

	public LoadAndGrowTreeView()
	{
		InitializeComponent();
	}

	public void ShutDown()
	{
	}

	private void ItemSelected(object sender, RoutedEventArgs e)
	{
		if (e.OriginalSource is TreeViewItem lastSelectedItem)
		{
			_lastSelectedItem = lastSelectedItem;
		}
	}

	private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		_currentSelectedItem = FindVisualChild(this);
		if (_selectedItems == null)
		{
			return;
		}
		if (IsMultiSelect && Keyboard.Modifiers == ModifierKeys.Control)
		{
			AddItemToMultiSelection(_currentSelectedItem);
		}
		else
		{
			if (!IsMultiSelect)
			{
				return;
			}
			foreach (TreeViewItem selectedItem in _selectedItems)
			{
				ResetItemColor(selectedItem);
			}
			_selectedItems.Clear();
			if (_currentSelectedItem != null && _currentSelectedItem.IsSelected)
			{
				AddItemToMultiSelection(_currentSelectedItem);
			}
		}
	}

	private void ItemHasExpanded(object sender, RoutedEventArgs e)
	{
		if (!(e.OriginalSource is TreeViewItem { DataContext: TreeViewItemData dataContext }))
		{
			return;
		}
		if (_isSynchronous)
		{
			dataContext.SubItems = FireItemHasBeenExpandedHandler(dataContext);
			if (dataContext.SubItems == null || !dataContext.SubItems.Any())
			{
				dataContext.SubItems = FireItemHasBeenExpandedHandler2(dataContext);
			}
		}
		else
		{
			FireASyncItemHasBeenExpandedHandler(dataContext);
		}
	}

	private void AsyncItemExpansion(TreeViewItemData expandedItem, List<TreeViewItemData> subNodes)
	{
		if (expandedItem != null)
		{
			expandedItem.SubItems = subNodes;
		}
	}

	private void ItemHasCollapsed(object sender, RoutedEventArgs e)
	{
		if (e.OriginalSource is TreeViewItem { DataContext: TreeViewItemData dataContext })
		{
			FirePreviewItemCollapse(dataContext);
			dataContext.SubItems = new List<TreeViewItemData>
			{
				new TreeViewItemData(null, Strings.TreeViewItemLoading, hasPossibleSubItems: false, dataContext)
			};
		}
	}

	private void LoadAndGrowTreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (_dragSelectedItemLocked)
		{
			return;
		}
		try
		{
			_dragSelectedItemLocked = true;
			if (sender is LoadAndGrowTreeView loadAndGrowTreeView)
			{
				_dragDropOrigin = e.GetPosition(loadAndGrowTreeView);
				_dragObject = loadAndGrowTreeView;
				if (!_selectNodeOnDrag && e.ClickCount == 1 && InputHitTest(_dragDropOrigin) is FrameworkElement element)
				{
					_dragSelectedItem = GetVisualTreeViewItemParent(element);
					if (_dragSelectedItem != null)
					{
						if (!_dragSelectedItem.IsSelected)
						{
							e.Handled = true;
						}
						_isDragging = false;
					}
				}
			}
			if (e.ClickCount >= 2 && e.OriginalSource is FrameworkElement { DataContext: TreeViewItemData dataContext })
			{
				if (dataContext.SubItems == null || dataContext.SubItems.Count == 0)
				{
					e.Handled = true;
				}
				FireItemDoubleClicked(dataContext);
			}
		}
		finally
		{
			_dragSelectedItemLocked = false;
		}
	}

	private void LoadAndGrowTreeView_PreviewMouseMove(object sender, MouseEventArgs e)
	{
		if (IgnoreMouseDrag || !(sender is LoadAndGrowTreeView loadAndGrowTreeView) || loadAndGrowTreeView != _dragObject)
		{
			return;
		}
		Point position = e.GetPosition(loadAndGrowTreeView);
		if (e.LeftButton != MouseButtonState.Pressed || (!(Math.Abs(position.X - _dragDropOrigin.X) > SystemParameters.MinimumHorizontalDragDistance) && !(Math.Abs(position.Y - _dragDropOrigin.Y) > SystemParameters.MinimumVerticalDragDistance)) || loadAndGrowTreeView.Visibility != Visibility.Visible)
		{
			return;
		}
		if (IsMultiSelect)
		{
			if (_selectedItems.Count > 0)
			{
				List<TreeViewItemData> list = new List<TreeViewItemData>();
				foreach (TreeViewItem selectedItem in _selectedItems)
				{
					if (VisualTreeHelper.GetParent(selectedItem) != null && selectedItem.DataContext is TreeViewItemData item)
					{
						list.Add(item);
					}
				}
				if (loadAndGrowTreeView.SelectedItem is TreeViewItemData treeViewItemData && loadAndGrowTreeView.InputHitTest(_dragDropOrigin) is FrameworkElement frameworkElement && frameworkElement.DataContext == treeViewItemData)
				{
					FireSelectedItemsDragged(list);
				}
			}
			else
			{
				StartSingleDrag(loadAndGrowTreeView);
			}
		}
		else
		{
			StartSingleDrag(loadAndGrowTreeView);
		}
	}

	private void LoadAndGrowTreeView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (_dragSelectedItemLocked)
		{
			return;
		}
		try
		{
			_dragSelectedItemLocked = true;
			if (!_selectNodeOnDrag && !_isDragging && _dragSelectedItem != null)
			{
				_dragSelectedItem.IsSelected = true;
				bool flag = false;
				if (_dragSelectedItem.IsKeyboardFocusWithin)
				{
					if ((Keyboard.FocusedElement as DependencyObject).GetVisualParent<TreeViewItem>() != _dragSelectedItem)
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					_dragSelectedItem.Focus();
				}
			}
			_isDragging = false;
			_dragSelectedItem = null;
			_dragObject = null;
		}
		finally
		{
			_dragSelectedItemLocked = false;
		}
	}

	protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
	{
		PropertyInfo property = GetType().GetProperty("ScrollHost", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetProperty);
		if (property != null)
		{
			ScrollViewer scrollViewer = (ScrollViewer)property.GetValue(this);
			if (HorizontalMouseWheelScrollingEnabled && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
			{
				if (IsScrollingDown(e))
				{
					scrollViewer.LineRight();
				}
				else
				{
					scrollViewer.LineLeft();
				}
				e.Handled = true;
			}
			else if (scrollViewer != null && ((IsScrollingDown(e) && scrollViewer.VerticalOffset >= scrollViewer.ExtentHeight - scrollViewer.ViewportHeight) || (!IsScrollingDown(e) && scrollViewer.VerticalOffset <= 0.0)))
			{
				e.Handled = true;
				RouteMouseWheelToParent(this, e);
			}
		}
		base.OnPreviewMouseWheel(e);
	}

	private TreeViewItem GetVisualTreeViewItemParent(FrameworkElement element)
	{
		if (element is ToggleButton)
		{
			return null;
		}
		if (element is TreeViewItem)
		{
			return (TreeViewItem)element;
		}
		for (FrameworkElement frameworkElement = VisualTreeHelper.GetParent(element) as FrameworkElement; frameworkElement != null; frameworkElement = VisualTreeHelper.GetParent(frameworkElement) as FrameworkElement)
		{
			if (frameworkElement is TreeViewItem)
			{
				return (TreeViewItem)frameworkElement;
			}
			if (frameworkElement is ToggleButton)
			{
				return null;
			}
		}
		return null;
	}

	private void SetItemMultiselectColor(TreeViewItem itemToChange)
	{
		if (itemToChange != null)
		{
			SetIsMultiSelected(itemToChange, value: true);
		}
	}

	private void ResetItemColor(TreeViewItem itemToChange)
	{
		if (itemToChange != null)
		{
			SetIsMultiSelected(itemToChange, value: false);
		}
	}

	private void AddItemToMultiSelection(TreeViewItem item)
	{
		if (_selectedItems != null && item != null && !_selectedItems.Contains(item))
		{
			_selectedItems.Add(item);
			SetItemMultiselectColor(item);
		}
	}

	private void StartSingleDrag(LoadAndGrowTreeView tree)
	{
		if (tree != null)
		{
			TreeViewItemData treeViewItemData = ((_selectNodeOnDrag || _dragSelectedItem == null) ? (tree.SelectedItem as TreeViewItemData) : (_dragSelectedItem.DataContext as TreeViewItemData));
			if (treeViewItemData != null && tree.InputHitTest(_dragDropOrigin) is FrameworkElement frameworkElement && frameworkElement.DataContext == treeViewItemData)
			{
				FireSelectedItemsDragged(new List<TreeViewItemData> { treeViewItemData });
			}
		}
	}

	private TreeViewItem FindVisualChild(FrameworkElement visual)
	{
		for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
		{
			if (VisualTreeHelper.GetChild(visual, i) is FrameworkElement frameworkElement)
			{
				if (frameworkElement is TreeViewItem { IsSelected: not false } treeViewItem)
				{
					return treeViewItem;
				}
				TreeViewItem treeViewItem2 = FindVisualChild(frameworkElement);
				if (treeViewItem2 != null)
				{
					return treeViewItem2;
				}
			}
		}
		return null;
	}

	private static void RouteMouseWheelToParent(Control sender, MouseWheelEventArgs e)
	{
		MouseWheelEventArgs e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
		{
			RoutedEvent = UIElement.MouseWheelEvent,
			Source = sender
		};
		if (sender.Parent is UIElement uIElement)
		{
			uIElement.RaiseEvent(e2);
		}
	}

	private bool IsScrollingDown(MouseWheelEventArgs args)
	{
		return args.Delta < 0;
	}

	private void TreeViewItem_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
	{
		if (e.TargetObject != null)
		{
			e.Handled = true;
			try
			{
				FrameworkElement frameworkElement = (FrameworkElement)e.TargetObject;
				Point point = frameworkElement.TransformToAncestor(this).Transform(new Point(0.0, 0.0));
				Rect targetRectangle = new Rect(new Point(0.0, point.Y), new Point(point.X + frameworkElement.ActualWidth, point.Y + frameworkElement.ActualHeight));
				BringIntoView(targetRectangle);
			}
			catch
			{
			}
		}
	}
}
