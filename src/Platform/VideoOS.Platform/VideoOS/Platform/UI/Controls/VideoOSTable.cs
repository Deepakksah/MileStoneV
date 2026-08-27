using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Xaml;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Commands;
using VideoOS.Platform.UI.Controls.Tables.Internal;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTable : Control
{
	public static class VideoOSTableInternals
	{
		public static IValueConverter ColumnHeaderAndCellIsTabStopConverter { get; } = new LambdaConverter1<ColumnDataGridTemplateColumn>((ColumnDataGridTemplateColumn column) => !(column?.Column is VideoOSTableMarkColumn), "ColumnHeaderAndCellIsTabStopConverter");

		public static IMultiValueConverter SortIndicatorIconSourceConverter { get; } = new LambdaConverter1<SortDirections>((SortDirections sortDirection) => (sortDirection != SortDirections.Ascending) ? new VideoOSIconBuiltInSource
		{
			Icon = VideoOSIconBuiltInSource.Icons.Arrow_Short_Down
		} : new VideoOSIconBuiltInSource
		{
			Icon = VideoOSIconBuiltInSource.Icons.Arrow_Short_Up
		}, "SortIndicatorIconSourceConverter");

		public static IMultiValueConverter SortIndicatorVisibilityConverter { get; } = new LambdaConverter2<object, VideoOSTableColumnBase>((object column, VideoOSTableColumnBase sortColumn) => (column is ColumnDataGridTemplateColumn columnDataGridTemplateColumn) ? ((object)((!columnDataGridTemplateColumn.Column.IsSortable || columnDataGridTemplateColumn.Column != sortColumn) ? Visibility.Hidden : Visibility.Visible)) : ((object)Visibility.Collapsed), "SortIndicatorVisibilityConverter");

		public static IMultiValueConverter HeaderBackgroundBrushConverter { get; } = new LambdaConverter6<DependencyObject, VideoOSTableColumnBase, bool, bool, bool, bool>(delegate(DependencyObject self, VideoOSTableColumnBase column, bool isMouseOver, bool isPressed, bool isKeyboardFocusWithinDataGrid, bool isEnabled)
		{
			if (column == null)
			{
				return (object)null;
			}
			string resourceKey = ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSTableHeaderBackgroundDisabledBrush" : ((isPressed && column.IsSortable) ? "VideoOSResourceKeysInternalVideoOSTableHeaderBackgroundPressedBrush" : ((!isMouseOver || !column.IsSortable) ? (isKeyboardFocusWithinDataGrid ? "VideoOSResourceKeysInternalVideoOSTableHeaderBackgroundIdleFocusedBrush" : "VideoOSResourceKeysInternalVideoOSTableHeaderBackgroundIdleUnfocusedBrush") : (isKeyboardFocusWithinDataGrid ? "VideoOSResourceKeysInternalVideoOSTableHeaderBackgroundHoveredFocusedBrush" : "VideoOSResourceKeysInternalVideoOSTableHeaderBackgroundHoveredUnfocusedBrush"))));
			return self.FindResource(resourceKey);
		}, "HeaderBackgroundBrushConverter");

		public static IMultiValueConverter CellMarginConverter { get; } = new LambdaConverter2<int, int>(delegate(int columnIndex, int columnsCount)
		{
			double left = ((columnIndex == 0) ? 16 : 8);
			double right = ((columnIndex == columnsCount - 1) ? 16 : 8);
			return new Thickness(left, 0.0, right, 0.0);
		}, "CellMarginConverter");

		public static IMultiValueConverter HeaderContentMarginConverter { get; } = new LambdaConverter4<int, int, VideoOSTableColumnBase, VideoOSTableColumnBase>(delegate(int columnDisplayIndex, int columnsCount, VideoOSTableColumnBase column, VideoOSTableColumnBase sortColumn)
		{
			double left = ((columnDisplayIndex == 0) ? 16 : 8);
			double num = ((columnDisplayIndex == columnsCount - 1) ? 16 : 8);
			if (column == sortColumn && column.IsSortable)
			{
				num += 24.0;
			}
			return new Thickness(left, 0.0, num, 0.0);
		}, "HeaderContentMarginConverter");

		public static IMultiValueConverter RowHeightConverter { get; } = new LambdaConverter1<RowAppearances>((RowAppearances rowAppearance) => rowAppearance switch
		{
			RowAppearances.Tall => 40.0, 
			RowAppearances.Normal => 32.0, 
			RowAppearances.Compact => 24.0, 
			_ => throw new ArgumentOutOfRangeException("rowAppearance", rowAppearance, null), 
		}, "RowHeightConverter");

		public static IMultiValueConverter RowBackgroundBrushConverter { get; } = new LambdaConverter7<DependencyObject, int, bool, bool, bool, SelectionModes, bool>(delegate(DependencyObject self, int alternationIndex, bool isMouseOver, bool isSelected, bool isKeyboardFocusWithinDataGrid, SelectionModes selectionMode, bool isEnabled)
		{
			string resourceKey = ((!isEnabled) ? ((!isSelected || selectionMode == SelectionModes.Disabled) ? ((alternationIndex == 0) ? "VideoOSResourceKeysInternalVideoOSTableRowBackgroundDeselectedDisabledBrush" : "VideoOSResourceKeysInternalVideoOSTableRowBackgroundDeselectedDisabledAlternateBrush") : ((alternationIndex == 0) ? "VideoOSResourceKeysInternalVideoOSTableRowBackgroundSelectedDisabledBrush" : "VideoOSResourceKeysInternalVideoOSTableRowBackgroundSelectedDisabledAlternateBrush")) : ((isMouseOver && selectionMode != SelectionModes.Disabled) ? (isSelected ? ((!isKeyboardFocusWithinDataGrid) ? ((alternationIndex == 0) ? "VideoOSResourceKeysInternalVideoOSTableRowBackgroundSelectedHoveredUnfocusedBrush" : "VideoOSResourceKeysInternalVideoOSTableRowBackgroundSelectedHoveredAlternateUnfocusedBrush") : ((alternationIndex == 0) ? "VideoOSResourceKeysInternalVideoOSTableRowBackgroundSelectedHoveredFocusedBrush" : "VideoOSResourceKeysInternalVideoOSTableRowBackgroundSelectedHoveredAlternateFocusedBrush")) : ((!isKeyboardFocusWithinDataGrid) ? ((alternationIndex == 0) ? "VideoOSResourceKeysInternalVideoOSTableRowBackgroundDeselectedHoveredUnfocusedBrush" : "VideoOSResourceKeysInternalVideoOSTableRowBackgroundDeselectedHoveredAlternateUnfocusedBrush") : ((alternationIndex == 0) ? "VideoOSResourceKeysInternalVideoOSTableRowBackgroundDeselectedHoveredFocusedBrush" : "VideoOSResourceKeysInternalVideoOSTableRowBackgroundDeselectedHoveredAlternateFocusedBrush"))) : ((isSelected && selectionMode != SelectionModes.Disabled) ? ((!isKeyboardFocusWithinDataGrid) ? ((alternationIndex == 0) ? "VideoOSResourceKeysInternalVideoOSTableRowBackgroundSelectedIdleUnfocusedBrush" : "VideoOSResourceKeysInternalVideoOSTableRowBackgroundSelectedIdleAlternateUnfocusedBrush") : ((alternationIndex == 0) ? "VideoOSResourceKeysInternalVideoOSTableRowBackgroundSelectedIdleFocusedBrush" : "VideoOSResourceKeysInternalVideoOSTableRowBackgroundSelectedIdleAlternateFocusedBrush")) : ((!isKeyboardFocusWithinDataGrid) ? ((alternationIndex == 0) ? "VideoOSResourceKeysInternalVideoOSTableRowBackgroundDeselectedIdleUnfocusedBrush" : "VideoOSResourceKeysInternalVideoOSTableRowBackgroundDeselectedIdleAlternateUnfocusedBrush") : ((alternationIndex == 0) ? "VideoOSResourceKeysInternalVideoOSTableRowBackgroundDeselectedIdleFocusedBrush" : "VideoOSResourceKeysInternalVideoOSTableRowBackgroundDeselectedIdleAlternateFocusedBrush")))));
			return self.FindResource(resourceKey);
		}, "RowBackgroundBrushConverter");
	}

	public enum RowAppearances
	{
		Tall,
		Normal,
		Compact
	}

	public enum SortDirections
	{
		Ascending,
		Descending
	}

	public enum RowSeparators
	{
		None,
		Line,
		Alternate
	}

	public enum SelectionModes
	{
		Disabled,
		Single,
		Multiple
	}

	public sealed class ColumnDataGridTemplateColumn : DataGridTemplateColumn
	{
		public VideoOSTableColumnBase Column { get; }

		public ColumnDataGridTemplateColumn(VideoOSTableColumnBase column)
		{
			Column = column;
			base.HeaderTemplate = column.GetHeaderTemplate();
			base.CellTemplate = column.GetCellTemplate();
			base.Width = column.Width;
			base.CanUserResize = column.CanUserResize;
			if (column.MinWidth >= 0.0)
			{
				base.MinWidth = column.MinWidth;
			}
			if (column.MaxWidth >= 0.0)
			{
				base.MaxWidth = column.MaxWidth;
			}
			base.Header = Guid.NewGuid().ToString();
			base.IsReadOnly = true;
		}
	}

	private const int ColumnEndsMargin = 16;

	private const int ColumnBetweenMargin = 8;

	public static readonly RoutedEvent SelectionChangedEvent;

	public static readonly RoutedEvent MarkedItemsChangedEvent;

	public static readonly DependencyProperty ColumnHeaderEventsCommandProperty;

	public static readonly DependencyProperty RowClickedCommandProperty;

	public static readonly DependencyProperty ColumnHeaderGripperClickedCommandProperty;

	public static readonly DependencyProperty RowAppearanceProperty;

	public static readonly DependencyProperty SelectionModeProperty;

	public static readonly DependencyProperty ColumnsProperty;

	public static readonly DependencyProperty ItemsSourceProperty;

	public static readonly DependencyProperty SelectedItemProperty;

	public static readonly DependencyPropertyKey SelectedItemsPropertyKey;

	public static readonly DependencyProperty IsBorderVisibleProperty;

	public static readonly DependencyProperty AreColumnHeadersVisibleProperty;

	public static readonly DependencyProperty RowSeparatorProperty;

	public static readonly DependencyProperty SortColumnProperty;

	public static readonly DependencyProperty SortDirectionProperty;

	public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty;

	public static readonly DependencyProperty VerticalScrollBarVisibilityProperty;

	private VideoOSTableDataGrid _dataGrid;

	public DelegateCommand ColumnHeaderEventsCommand
	{
		get
		{
			return (DelegateCommand)GetValue(ColumnHeaderEventsCommandProperty);
		}
		private set
		{
			SetValue(ColumnHeaderEventsCommandProperty, value);
		}
	}

	public DelegateCommand RowClickedCommand
	{
		get
		{
			return (DelegateCommand)GetValue(RowClickedCommandProperty);
		}
		private set
		{
			SetValue(RowClickedCommandProperty, value);
		}
	}

	public DelegateCommand ColumnHeaderGripperClickedCommand
	{
		get
		{
			return (DelegateCommand)GetValue(ColumnHeaderGripperClickedCommandProperty);
		}
		private set
		{
			SetValue(ColumnHeaderGripperClickedCommandProperty, value);
		}
	}

	public RowAppearances RowAppearance
	{
		get
		{
			return (RowAppearances)GetValue(RowAppearanceProperty);
		}
		set
		{
			SetValue(RowAppearanceProperty, value);
		}
	}

	public SelectionModes SelectionMode
	{
		get
		{
			return (SelectionModes)GetValue(SelectionModeProperty);
		}
		set
		{
			SetValue(SelectionModeProperty, value);
		}
	}

	public ICollection<VideoOSTableColumnBase> Columns
	{
		get
		{
			return (ICollection<VideoOSTableColumnBase>)GetValue(ColumnsProperty);
		}
		set
		{
			SetValue(ColumnsProperty, value);
		}
	}

	public IEnumerable ItemsSource
	{
		get
		{
			return (IEnumerable)GetValue(ItemsSourceProperty);
		}
		set
		{
			SetValue(ItemsSourceProperty, value);
		}
	}

	public object SelectedItem
	{
		get
		{
			return GetValue(SelectedItemProperty);
		}
		set
		{
			SetValue(SelectedItemProperty, value);
		}
	}

	public IList SelectedItems
	{
		get
		{
			return (IList)GetValue(SelectedItemsPropertyKey.DependencyProperty);
		}
		private set
		{
			SetValue(SelectedItemsPropertyKey, value);
		}
	}

	public IList MarkedItems { get; }

	public bool IsBorderVisible
	{
		get
		{
			return (bool)GetValue(IsBorderVisibleProperty);
		}
		set
		{
			SetValue(IsBorderVisibleProperty, value);
		}
	}

	public bool AreColumnHeadersVisible
	{
		get
		{
			return (bool)GetValue(AreColumnHeadersVisibleProperty);
		}
		set
		{
			SetValue(AreColumnHeadersVisibleProperty, value);
		}
	}

	public RowSeparators RowSeparator
	{
		get
		{
			return (RowSeparators)GetValue(RowSeparatorProperty);
		}
		set
		{
			SetValue(RowSeparatorProperty, value);
		}
	}

	public VideoOSTableColumnBase SortColumn
	{
		get
		{
			return (VideoOSTableColumnBase)GetValue(SortColumnProperty);
		}
		set
		{
			SetValue(SortColumnProperty, value);
		}
	}

	public SortDirections SortDirection
	{
		get
		{
			return (SortDirections)GetValue(SortDirectionProperty);
		}
		set
		{
			SetValue(SortDirectionProperty, value);
		}
	}

	public ScrollBarVisibility HorizontalScrollBarVisibility
	{
		get
		{
			return (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty);
		}
		set
		{
			SetValue(HorizontalScrollBarVisibilityProperty, value);
		}
	}

	public ScrollBarVisibility VerticalScrollBarVisibility
	{
		get
		{
			return (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty);
		}
		set
		{
			SetValue(VerticalScrollBarVisibilityProperty, value);
		}
	}

	public event RoutedEventHandler SelectionChanged
	{
		add
		{
			AddHandler(SelectionChangedEvent, value);
		}
		remove
		{
			RemoveHandler(SelectionChangedEvent, value);
		}
	}

	public event RoutedEventHandler MarkedItemsChanged
	{
		add
		{
			AddHandler(MarkedItemsChangedEvent, value);
		}
		remove
		{
			RemoveHandler(MarkedItemsChangedEvent, value);
		}
	}

	public event EventHandler ItemsChanged;

	static VideoOSTable()
	{
		SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSTable));
		MarkedItemsChangedEvent = EventManager.RegisterRoutedEvent("MarkedItemsChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSTable));
		ColumnHeaderEventsCommandProperty = DependencyProperty.Register("ColumnHeaderEventsCommand", typeof(DelegateCommand), typeof(VideoOSTable), new PropertyMetadata((object)null));
		RowClickedCommandProperty = DependencyProperty.Register("RowClickedCommand", typeof(DelegateCommand), typeof(VideoOSTable), new PropertyMetadata((object)null));
		ColumnHeaderGripperClickedCommandProperty = DependencyProperty.Register("ColumnHeaderGripperClickedCommand", typeof(DelegateCommand), typeof(VideoOSTable), new PropertyMetadata((object)null));
		RowAppearanceProperty = DependencyProperty.Register("RowAppearance", typeof(RowAppearances), typeof(VideoOSTable), new PropertyMetadata(RowAppearances.Normal));
		SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(SelectionModes), typeof(VideoOSTable), new PropertyMetadata(SelectionModes.Single, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSTable)o).SelectionModeChanged();
		}));
		ColumnsProperty = DependencyProperty.Register("Columns", typeof(ICollection<VideoOSTableColumnBase>), typeof(VideoOSTable), new PropertyMetadata(null, CollectionHelperBase.CollectionChangedCallback));
		ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(VideoOSTable), new PropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSTable)o).ItemsSourceChanged();
		}));
		SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(VideoOSTable), new FrameworkPropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSTable)o).SelectedItemChanged();
		}));
		SelectedItemsPropertyKey = DependencyProperty.RegisterReadOnly("SelectedItems", typeof(IList), typeof(VideoOSTable), new FrameworkPropertyMetadata(null));
		IsBorderVisibleProperty = DependencyProperty.Register("IsBorderVisible", typeof(bool), typeof(VideoOSTable), new PropertyMetadata(true));
		AreColumnHeadersVisibleProperty = DependencyProperty.Register("AreColumnHeadersVisible", typeof(bool), typeof(VideoOSTable), new PropertyMetadata(true));
		RowSeparatorProperty = DependencyProperty.Register("RowSeparator", typeof(RowSeparators), typeof(VideoOSTable), new PropertyMetadata(RowSeparators.Line));
		SortColumnProperty = DependencyProperty.Register("SortColumn", typeof(VideoOSTableColumnBase), typeof(VideoOSTable), new PropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSTable)o).SortChanged();
		}));
		SortDirectionProperty = DependencyProperty.Register("SortDirection", typeof(SortDirections), typeof(VideoOSTable), new PropertyMetadata(SortDirections.Ascending, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSTable)o).SortChanged();
		}));
		HorizontalScrollBarVisibilityProperty = DependencyProperty.Register("HorizontalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(VideoOSTable), new PropertyMetadata(ScrollBarVisibility.Hidden));
		VerticalScrollBarVisibilityProperty = DependencyProperty.Register("VerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(VideoOSTable), new PropertyMetadata(ScrollBarVisibility.Auto));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTable), new FrameworkPropertyMetadata(typeof(VideoOSTable)));
		ThemeManager.InitializeThemingSystem();
		UIElement.FocusableProperty.OverrideMetadata(typeof(VideoOSTable), new FrameworkPropertyMetadata(false));
	}

	public VideoOSTable()
	{
		SelectedItems = new ArrayList();
		MarkedItems = new ObservableCollection<object>();
		((ObservableCollection<object>)MarkedItems).CollectionChanged += MarkedItems_CollectionChanged;
		CollectionHelper<VideoOSTableColumnBase>.Initialize(this, ColumnsProperty, ColumnsChanged, new ObservableCollection<VideoOSTableColumnBase>());
		ColumnHeaderEventsCommand = new DelegateCommand(ColumnHeaderEventsCommandFunction);
		RowClickedCommand = new DelegateCommand(RowClickedCommandFunction);
		ColumnHeaderGripperClickedCommand = new DelegateCommand(ColumnHeaderGripperClickedCommandFunction);
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		if (_dataGrid != null)
		{
			throw new XamlException("Template has already been applied.");
		}
		_dataGrid = this.GetTemplateChild<VideoOSTableDataGrid>("PART_DataGrid");
		_dataGrid.Resources[typeof(ScrollViewer)] = ThemeManager.Instance.FindStaticResource("VideoOSStyleScrollBarScrollViewerStyleKey");
		UpdateColumns();
		UpdateItemsSource();
		UpdateSelectionMode();
		UpdateSelectedItem();
		foreach (object selectedItem in SelectedItems)
		{
			_dataGrid.SelectedItems.Add(selectedItem);
		}
		SelectedItems = _dataGrid.SelectedItems;
		UpdateSorting();
		_dataGrid.SelectionChanged += _dataGrid_SelectionChanged;
		_dataGrid.ItemsChanged += _dataGrid_ItemsChanged;
	}

	public void Select(IList items)
	{
		items = items ?? new List<object>();
		if (_dataGrid != null)
		{
			_dataGrid.Select(items);
			return;
		}
		SelectedItems.Clear();
		foreach (object item in items)
		{
			SelectedItems.Add(item);
		}
	}

	public void SelectAll()
	{
		if (_dataGrid != null)
		{
			_dataGrid.SelectAll();
			return;
		}
		SelectedItems.Clear();
		foreach (object item in ItemsSource)
		{
			SelectedItems.Add(item);
		}
	}

	public void UnselectAll()
	{
		if (_dataGrid != null)
		{
			_dataGrid.UnselectAll();
		}
		else
		{
			SelectedItems.Clear();
		}
	}

	public void Mark(IList items)
	{
		items = items ?? new List<object>();
		ObservableCollection<object> observableCollection = (ObservableCollection<object>)MarkedItems;
		bool flag = false;
		if (items.Count != observableCollection.Count)
		{
			flag = true;
		}
		else
		{
			HashSet<object> hashSet = new HashSet<object>();
			foreach (object item in observableCollection)
			{
				hashSet.Add(item);
			}
			foreach (object item2 in items)
			{
				if (!hashSet.Contains(item2))
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			return;
		}
		observableCollection.CollectionChanged -= MarkedItems_CollectionChanged;
		observableCollection.Clear();
		foreach (object item3 in items)
		{
			observableCollection.Add(item3);
		}
		observableCollection.CollectionChanged += MarkedItems_CollectionChanged;
		RaiseEvent(new RoutedEventArgs(MarkedItemsChangedEvent));
	}

	public void MarkAll()
	{
		if (ItemsSource == null || MarkedItems.Count >= ItemsSource.OfType<object>().Count())
		{
			return;
		}
		ObservableCollection<object> observableCollection = (ObservableCollection<object>)MarkedItems;
		observableCollection.CollectionChanged -= MarkedItems_CollectionChanged;
		observableCollection.Clear();
		foreach (object item in ItemsSource)
		{
			observableCollection.Add(item);
		}
		observableCollection.CollectionChanged += MarkedItems_CollectionChanged;
		RaiseEvent(new RoutedEventArgs(MarkedItemsChangedEvent));
	}

	public void UnmarkAll()
	{
		if (MarkedItems.Count > 0)
		{
			((ObservableCollection<object>)MarkedItems).Clear();
		}
	}

	public void ApplyPrimarySortOrder()
	{
		if (SortColumn != null)
		{
			SortDirection = SortColumn.PrimarySortDirection;
		}
	}

	public void ScrollIntoView(object item)
	{
		_dataGrid?.ScrollIntoView(item);
	}

	public void ScrollToTop()
	{
		this.FindDescendant<ScrollViewer>()?.ScrollToTop();
	}

	public void ScrollToBottom()
	{
		this.FindDescendant<ScrollViewer>()?.ScrollToBottom();
	}

	public void Refresh()
	{
		_dataGrid?.Items.Refresh();
		UpdateSorting();
	}

	private void _dataGrid_ItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		this.ItemsChanged?.Invoke(this, EventArgs.Empty);
		if (MarkedItems.Count <= 0 || e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Move)
		{
			return;
		}
		List<object> list = new List<object>();
		if (ItemsSource != null)
		{
			HashSet<object> hashSet = new HashSet<object>();
			foreach (object item in ItemsSource)
			{
				hashSet.Add(item);
			}
			if (hashSet.Count > 0)
			{
				foreach (object markedItem in MarkedItems)
				{
					if (hashSet.Contains(markedItem))
					{
						list.Add(markedItem);
					}
				}
			}
		}
		Mark(list);
	}

	private void _dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		SelectedItem = _dataGrid.SelectedItem;
		RaiseEvent(new RoutedEventArgs(SelectionChangedEvent));
	}

	private void MarkedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		RaiseEvent(new RoutedEventArgs(MarkedItemsChangedEvent));
	}

	private void ColumnHeaderEventsCommandFunction(object args)
	{
		EventsToCommandMappingArgs eventsToCommandMappingArgs = (EventsToCommandMappingArgs)args;
		DataGridColumnHeader dataGridColumnHeader = (DataGridColumnHeader)eventsToCommandMappingArgs.Sender;
		if (eventsToCommandMappingArgs.EventName == "Loaded")
		{
			if (dataGridColumnHeader.Column == null)
			{
				dataGridColumnHeader.Focusable = false;
			}
		}
		else
		{
			if (!(eventsToCommandMappingArgs.EventName == "Click"))
			{
				return;
			}
			VideoOSTableColumnBase column = ((ColumnDataGridTemplateColumn)dataGridColumnHeader.Column).Column;
			if (column.IsSortable)
			{
				if (SortColumn != column)
				{
					SortColumn = column;
					SortDirection = column.PrimarySortDirection;
				}
				else
				{
					SortDirection = ((SortDirection == SortDirections.Ascending) ? SortDirections.Descending : SortDirections.Ascending);
				}
			}
		}
	}

	private void RowClickedCommandFunction(object args)
	{
		if (Columns != null && Columns.Count != 0)
		{
			EventsToCommandMappingArgs eventsToCommandMappingArgs = (EventsToCommandMappingArgs)args;
			DataGridCellsPresenter dataGridCellsPresenter = ((DataGridRow)eventsToCommandMappingArgs.Sender).FindDescendant<DataGridCellsPresenter>();
			if (dataGridCellsPresenter != null)
			{
				DataGridCell dataGridCell = (DataGridCell)dataGridCellsPresenter.ItemContainerGenerator.ContainerFromIndex(Columns.Count - 1);
				dataGridCell.GetType().GetMethod("OnAnyMouseLeftButtonDown", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(dataGridCell, new object[1] { eventsToCommandMappingArgs.OriginalEventArgs });
			}
		}
	}

	private void ColumnHeaderGripperClickedCommandFunction(object args)
	{
		_dataGrid.Focus();
	}

	private void SortChanged()
	{
		UpdateSorting();
	}

	private void ColumnsChanged(CollectionChangedInfo<VideoOSTableColumnBase> info)
	{
		Refresh();
		UpdateColumns();
	}

	private void SelectionModeChanged()
	{
		UpdateSelectionMode();
	}

	private void SelectedItemChanged()
	{
		UpdateSelectedItem();
	}

	private void ItemsSourceChanged()
	{
		UpdateItemsSource();
		UpdateSorting();
	}

	private void UpdateColumns()
	{
		if (_dataGrid == null)
		{
			return;
		}
		_dataGrid.Columns.Clear();
		bool flag = true;
		if (Columns != null)
		{
			foreach (VideoOSTableColumnBase column in Columns)
			{
				_dataGrid.Columns.Add(new ColumnDataGridTemplateColumn(column));
				if (SortColumn == column)
				{
					flag = false;
				}
			}
		}
		if (flag)
		{
			SortColumn = null;
		}
	}

	private void UpdateItemsSource()
	{
		if (_dataGrid != null)
		{
			_dataGrid.ItemsSource = ItemsSource;
		}
	}

	private void UpdateSelectionMode()
	{
		if (_dataGrid != null)
		{
			_dataGrid.SelectionMode = ((SelectionMode == SelectionModes.Multiple) ? DataGridSelectionMode.Extended : DataGridSelectionMode.Single);
			if (SelectionMode == SelectionModes.Disabled)
			{
				SelectedItem = null;
			}
		}
	}

	private void UpdateSelectedItem()
	{
		if (_dataGrid != null)
		{
			_dataGrid.SelectedItem = SelectedItem;
		}
	}

	private void UpdateSorting()
	{
		if (_dataGrid == null)
		{
			return;
		}
		_dataGrid.Items.SortDescriptions.Clear();
		if (SortColumn != null && SortColumn.IsSortable)
		{
			string text = (string.IsNullOrWhiteSpace(SortColumn.ItemSortPropertyName) ? SortColumn.FallbackItemSortPropertyName : SortColumn.ItemSortPropertyName);
			text = (string.IsNullOrWhiteSpace(text) ? "." : text);
			string[] array = text.Split(new char[2] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
			ListSortDirection direction = ((SortDirection != SortDirections.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				_dataGrid.Items.SortDescriptions.Add(new SortDescription(text2.Trim(), direction));
			}
		}
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSTableAutomationPeer(this);
	}
}
