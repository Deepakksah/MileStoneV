using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using VideoOS.Platform.UI.ItemPicker.Data;
using VideoOS.Platform.UI.ItemPicker.Utilities;
using VideoOS.Platform.UI.Resources;

namespace VideoOS.Platform.UI.ItemPicker.ViewModel;

public class ItemPickerWindowViewModel : ViewModelBase, IDisposable
{
	public delegate void SelectedItemChangedEvent(object sender, EventArgs e);

	private ItemPickerViewModel _itemPickerVM;

	private string _description = Strings.ItemPickerWindow_DefaultDescriptionText;

	private string _header = Strings.ItemPickerWindow_DefaultTitle;

	public bool AutoCloseOnSelectModeReady => _itemPickerVM.AutoCloseOnSelectModeReady;

	public IEnumerable<Item> Items
	{
		set
		{
			_itemPickerVM.Items = value;
		}
	}

	public IEnumerable<Item> SelectedItems
	{
		get
		{
			return _itemPickerVM.SelectedItems.Select((ItemViewModel x) => x.InternalItem);
		}
		set
		{
			_itemPickerVM.PreSelectedItemsCollection = new List<Item>(value);
		}
	}

	public List<Guid> KindsFilter
	{
		get
		{
			return _itemPickerVM?.KindsFilter;
		}
		set
		{
			_itemPickerVM.KindsFilter = value;
		}
	}

	public bool AutoExpand
	{
		get
		{
			return _itemPickerVM.AutoExpand;
		}
		set
		{
			_itemPickerVM.AutoExpand = value;
		}
	}

	public bool SearchEnabled
	{
		get
		{
			return _itemPickerVM.SearchEnabled;
		}
		set
		{
			_itemPickerVM.SearchEnabled = value;
		}
	}

	public Func<Item, bool> IsVisibleCallback
	{
		set
		{
			_itemPickerVM.IsVisibleCallback = value;
		}
	}

	public Func<Item, bool> IsValidSelectionCallback
	{
		set
		{
			_itemPickerVM.IsValidSelectionCallback = value;
		}
	}

	public Func<Item, Item> ReplaceSelectionCallback
	{
		set
		{
			_itemPickerVM.ReplaceSelectionCallback = value;
		}
	}

	public SelectionModeOptions SelectionMode
	{
		get
		{
			return _itemPickerVM.SelectionMode;
		}
		set
		{
			_itemPickerVM.SelectionMode = value;
		}
	}

	public bool AllowGroupSelection
	{
		get
		{
			return _itemPickerVM.AllowGroupSelection;
		}
		set
		{
			_itemPickerVM.AllowGroupSelection = value;
		}
	}

	public string TableHeader
	{
		get
		{
			return _itemPickerVM.TableHeader;
		}
		set
		{
			_itemPickerVM.TableHeader = value;
		}
	}

	public string SearchPlaceholderText
	{
		get
		{
			return _itemPickerVM.SearchPlaceholderText;
		}
		set
		{
			_itemPickerVM.SearchPlaceholderText = value;
		}
	}

	public string RemoveActionTooltip
	{
		get
		{
			return _itemPickerVM.RemoveActionTooltip;
		}
		set
		{
			_itemPickerVM.RemoveActionTooltip = value;
		}
	}

	public string EmptyListOverlayText
	{
		get
		{
			return _itemPickerVM.EmptyListOverlayText;
		}
		set
		{
			_itemPickerVM.EmptyListOverlayText = value;
		}
	}

	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value;
			OnPropertyChanged("Description");
		}
	}

	public string Header
	{
		get
		{
			return _header;
		}
		set
		{
			_header = value;
			OnPropertyChanged("Header");
		}
	}

	public bool Close { get; set; }

	public ICommand OkCommand { get; set; }

	public ICommand CancelCommand { get; set; }

	public event SelectedItemChangedEvent SelectedItemChanged;

	public ItemPickerWindowViewModel()
	{
		OkCommand = new RelayCommand(OkHandler);
		CancelCommand = new RelayCommand(CancelHandler);
		Close = false;
	}

	public void Initialize(ItemPickerViewModel itemPickerVM)
	{
		_itemPickerVM = itemPickerVM;
		_itemPickerVM.SelectedItemChanged += HandleSelectedItemChanged;
	}

	private void OkHandler(object sender)
	{
		if (!OkCanExecute(sender))
		{
			_itemPickerVM.ErrorVisibility = Visibility.Visible;
			return;
		}
		Close = true;
		if (this.SelectedItemChanged != null)
		{
			this.SelectedItemChanged(this, EventArgs.Empty);
		}
	}

	public bool OkCanExecute(object action)
	{
		return _itemPickerVM.SelectedItems.Any();
	}

	private void CancelHandler(object sender)
	{
		_itemPickerVM.SelectedItems.Clear();
		Close = true;
		if (this.SelectedItemChanged != null)
		{
			this.SelectedItemChanged(this, EventArgs.Empty);
		}
	}

	private void HandleSelectedItemChanged(object sender, EventArgs args)
	{
		if (_itemPickerVM.SelectedItems.Any())
		{
			_itemPickerVM.ErrorVisibility = Visibility.Collapsed;
		}
		if (this.SelectedItemChanged != null)
		{
			this.SelectedItemChanged(this, args);
		}
	}

	public void Dispose()
	{
		_itemPickerVM.SelectedItemChanged -= HandleSelectedItemChanged;
	}
}
