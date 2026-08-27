using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.UI.Controls;
using VideoOS.Platform.UI.ItemPicker.Data;
using VideoOS.Platform.UI.ItemPicker.ViewModel;

namespace VideoOS.Platform.UI;

public partial class ItemPickerWpfUserControl : UserControl, IDisposable, IComponentConnector
{
	public delegate void SelectedItemChangedEvent(object sender, EventArgs e);

	private bool _isDisposed;

	private bool _telemetryDataSent;

	private readonly ItemPickerViewModel _viewModel;

	public string ErrorText
	{
		get
		{
			return _viewModel.ErrorText;
		}
		set
		{
			_viewModel.ErrorText = value;
		}
	}

	public IEnumerable<Item> Items
	{
		set
		{
			_viewModel.Items = value;
			if (!_telemetryDataSent)
			{
				new TrackPageViewData(typeof(ItemPickerWpfUserControl)).Send();
				_telemetryDataSent = true;
			}
		}
	}

	public IEnumerable<Item> SelectedItems
	{
		get
		{
			return _viewModel.SelectedItems.Select((ItemViewModel x) => x.InternalItem);
		}
		set
		{
			_viewModel.PreSelectedItemsCollection = value.ToList();
		}
	}

	public List<Guid> KindsFilter
	{
		get
		{
			return _viewModel.KindsFilter;
		}
		set
		{
			_viewModel.KindsFilter = value;
		}
	}

	public bool AutoExpand
	{
		get
		{
			return _viewModel.AutoExpand;
		}
		set
		{
			_viewModel.AutoExpand = value;
		}
	}

	public bool SearchEnabled
	{
		get
		{
			return _viewModel.SearchEnabled;
		}
		set
		{
			_viewModel.SearchEnabled = value;
		}
	}

	public Func<Item, bool> IsVisibleCallback
	{
		set
		{
			_viewModel.IsVisibleCallback = value;
		}
	}

	public Func<Item, bool> IsValidSelectionCallback
	{
		set
		{
			_viewModel.IsValidSelectionCallback = value;
		}
	}

	public Func<Item, Item> ReplaceSelectionCallback
	{
		set
		{
			_viewModel.ReplaceSelectionCallback = value;
		}
	}

	public bool IsMultiSelection
	{
		get
		{
			return _viewModel.IsMultiSelection;
		}
		set
		{
			_viewModel.IsMultiSelection = value;
		}
	}

	public bool AllowGroupSelection
	{
		get
		{
			return _viewModel.AllowGroupSelection;
		}
		set
		{
			_viewModel.AllowGroupSelection = value;
		}
	}

	public string TableHeader
	{
		get
		{
			return _viewModel.TableHeader;
		}
		set
		{
			_viewModel.TableHeader = value;
		}
	}

	public string SearchPlaceholderText
	{
		get
		{
			return _viewModel.SearchPlaceholderText;
		}
		set
		{
			_viewModel.SearchPlaceholderText = value;
		}
	}

	public string RemoveActionTooltipText
	{
		get
		{
			return _viewModel.RemoveActionTooltip;
		}
		set
		{
			_viewModel.RemoveActionTooltip = value;
		}
	}

	public string EmptyListOverlayText
	{
		get
		{
			return _viewModel.EmptyListOverlayText;
		}
		set
		{
			_viewModel.EmptyListOverlayText = value;
		}
	}

	public event SelectedItemChangedEvent SelectedItemChanged;

	public ItemPickerWpfUserControl()
	{
		InitializeComponent();
		_viewModel = new ItemPickerViewModel();
		_viewModel.SelectedItemChanged += HandleSelectedItemChanged;
		_viewModel.PropertyChanged += SetVisibilityViewMode;
		base.DataContext = _viewModel;
		ChangeVisibilityViewMode();
	}

	private void SetVisibilityViewMode(object sender, PropertyChangedEventArgs e)
	{
		if (!(e.PropertyName != "SelectionMode") || !(e.PropertyName != "IsMultiSelection"))
		{
			ChangeVisibilityViewMode();
		}
	}

	private void ChangeVisibilityViewMode()
	{
		if (_viewModel.SelectionMode != SelectionModeOptions.MultiSelect)
		{
			RemoveAll.Visibility = Visibility.Collapsed;
			MultiItemGrid.Visibility = Visibility.Collapsed;
			SingleItemGrid.Visibility = Visibility.Visible;
		}
		else
		{
			RemoveAll.Visibility = Visibility.Visible;
			MultiItemGrid.Visibility = Visibility.Visible;
			SingleItemGrid.Visibility = Visibility.Collapsed;
		}
	}

	private void ItemPicker_Selected(object sender, MouseButtonEventArgs e)
	{
		if (e.Source is VideoOSTreeView { SelectedItem: ItemViewModel { InternalItem: not null } selectedItem })
		{
			_viewModel.AppendItemPicked(selectedItem);
		}
	}

	private void TreeView_ScrollView(object sender, MouseWheelEventArgs e)
	{
		ScrollViewer obj = (ScrollViewer)sender;
		obj.ScrollToVerticalOffset(obj.VerticalOffset - (double)e.Delta);
		e.Handled = true;
	}

	private void Table_ScrollView(object sender, MouseWheelEventArgs e)
	{
		ScrollViewer obj = (ScrollViewer)sender;
		obj.ScrollToVerticalOffset(obj.VerticalOffset - (double)e.Delta);
		e.Handled = true;
	}

	private void RemoveAllTextBox_MouseUpClick(object sender, RoutedEventArgs e)
	{
		_viewModel.RemoveAllSelectedItems();
	}

	private void HandleSelectedItemChanged(object sender, EventArgs e)
	{
		if (!_viewModel.SelectedItems.Any() && SingleTreeView.SelectedItem != null)
		{
			((ItemViewModel)SingleTreeView.SelectedItem).IsSelected = false;
		}
		if (this.SelectedItemChanged != null)
		{
			this.SelectedItemChanged(this, EventArgs.Empty);
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_isDisposed && disposing)
		{
			_viewModel.SelectedItemChanged -= HandleSelectedItemChanged;
			_viewModel.PropertyChanged -= SetVisibilityViewMode;
			_viewModel.Dispose();
			_isDisposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
