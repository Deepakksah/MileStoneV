using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using VideoOS.Platform.UI.Controls;
using VideoOS.Platform.UI.ItemPicker.Data;
using VideoOS.Platform.UI.ItemPicker.Utilities;
using VideoOS.Platform.UI.Resources;

namespace VideoOS.Platform.UI.ItemPicker.ViewModel;

public class ItemPickerViewModel : ViewModelBase, IDisposable
{
	public delegate void SelectedItemChangedEvent(object sender, EventArgs e);

	private IEnumerable<Item> _sourceItems;

	private ObservableCollection<ItemViewModel> _items;

	private List<Item> _preSelectedItemsCollection;

	private ObservableCollection<ItemViewModel> _selectedItems;

	private List<Guid> _kindsFilter;

	private bool _autoExpand;

	private Visibility _searchTextboxVisibility = Visibility.Collapsed;

	private Visibility _emptyListOverlayvisibility;

	private string _errorText = Strings.ItemPickerViewModel_DefaultErrorText;

	private Visibility _errorVisibility = Visibility.Collapsed;

	private bool? _expandBeforeSearch;

	private string _searchString = "";

	private string[] _searchSplit = new string[1] { "" };

	private bool _searchFilter;

	private Func<Item, bool> _isVisibleCallback;

	private SelectionModeOptions _selectionMode = SelectionModeOptions.SingleSelect;

	private string _tableHeader = Strings.ItemPickerViewModel_DefaultTableHeaderContent;

	private string _searchPlaceholderText = Strings.ItemPickerViewModel_DefaultSearchPlaceholderText;

	private string _removeAllButtonText = Strings.ItemPickerViewModel_RemoveAllButtonText;

	private string _removeActionTooltip = Strings.ItemPickerViewModel_RemoveButtonTooltip;

	private string _emptyListOverlayText = Strings.ItemPickerViewModel_DefaultEmptyListOverlayText;

	private CancellationTokenSource _lastCancellationTokenSource;

	private object _lockToken = new object();

	public IEnumerable<Item> Items
	{
		set
		{
			_sourceItems = new List<Item>(value);
			Populate();
		}
	}

	public ObservableCollection<ItemViewModel> ItemViewModels
	{
		get
		{
			if (_items == null)
			{
				_items = new ObservableCollection<ItemViewModel>();
			}
			return _items;
		}
		private set
		{
			_items = value;
			OnPropertyChanged("ItemViewModels");
		}
	}

	public List<Item> PreSelectedItemsCollection
	{
		get
		{
			if (_preSelectedItemsCollection == null)
			{
				_preSelectedItemsCollection = new List<Item>();
			}
			return _preSelectedItemsCollection;
		}
		set
		{
			_preSelectedItemsCollection = new List<Item>(value);
			SetPreSelectedItemsCollection();
		}
	}

	public ObservableCollection<ItemViewModel> SelectedItems
	{
		get
		{
			if (_selectedItems == null)
			{
				_selectedItems = new ObservableCollection<ItemViewModel>();
				_selectedItems.CollectionChanged += HandleSelectedItemChanged;
			}
			return _selectedItems;
		}
		set
		{
			if (_selectedItems != null)
			{
				_selectedItems.CollectionChanged -= HandleSelectedItemChanged;
			}
			_selectedItems = new ObservableCollection<ItemViewModel>(value);
			_selectedItems.CollectionChanged += HandleSelectedItemChanged;
			OnPropertyChanged("SelectedItems");
		}
	}

	public List<Guid> KindsFilter
	{
		get
		{
			if (_kindsFilter == null)
			{
				_kindsFilter = new List<Guid>();
			}
			return _kindsFilter;
		}
		set
		{
			_kindsFilter = new List<Guid>(value);
			OnPropertyChanged("KindsFilter");
			if (_sourceItems != null)
			{
				Populate();
			}
		}
	}

	public bool AutoExpand
	{
		get
		{
			return _autoExpand;
		}
		set
		{
			_autoExpand = value;
			OnPropertyChanged("AutoExpand");
		}
	}

	public Visibility SearchTextboxVisibility
	{
		get
		{
			return _searchTextboxVisibility;
		}
		set
		{
			_searchTextboxVisibility = value;
			OnPropertyChanged("SearchTextboxVisibility");
			OnPropertyChanged("SearchEnabled");
		}
	}

	public Visibility EmptyListOverlayVisibility
	{
		get
		{
			return _emptyListOverlayvisibility;
		}
		set
		{
			_emptyListOverlayvisibility = value;
			OnPropertyChanged("EmptyListOverlayVisibility");
		}
	}

	public string ErrorText
	{
		get
		{
			return _errorText;
		}
		set
		{
			_errorText = value;
			OnPropertyChanged("ErrorText");
		}
	}

	public Visibility ErrorVisibility
	{
		get
		{
			return _errorVisibility;
		}
		set
		{
			_errorVisibility = value;
			OnPropertyChanged("ErrorVisibility");
		}
	}

	public bool SearchEnabled
	{
		get
		{
			return SearchTextboxVisibility == Visibility.Visible;
		}
		set
		{
			SearchTextboxVisibility = ((!value) ? Visibility.Collapsed : Visibility.Visible);
		}
	}

	public string SearchString
	{
		get
		{
			return _searchString;
		}
		set
		{
			_searchString = value;
			_searchFilter = !string.IsNullOrEmpty(_searchString);
			if (_searchFilter)
			{
				_searchSplit = _searchString.Split(' ');
				if (!_expandBeforeSearch.HasValue)
				{
					_expandBeforeSearch = AutoExpand;
				}
				AutoExpand = true;
				if (_sourceItems != null)
				{
					Populate();
				}
			}
			else
			{
				if (_expandBeforeSearch.HasValue)
				{
					AutoExpand = _expandBeforeSearch.Value;
				}
				Populate();
			}
			OnPropertyChanged("SearchString");
		}
	}

	public bool AutoCloseOnSelectModeReady { get; private set; }

	public Func<Item, bool> IsVisibleCallback
	{
		private get
		{
			return _isVisibleCallback;
		}
		set
		{
			_isVisibleCallback = value;
			if (_sourceItems != null)
			{
				Populate();
			}
		}
	}

	public Func<Item, bool> IsValidSelectionCallback { private get; set; }

	public Func<Item, Item> ReplaceSelectionCallback { private get; set; }

	public SelectionModeOptions SelectionMode
	{
		get
		{
			return _selectionMode;
		}
		set
		{
			if (value == SelectionModeOptions.AutoCloseOnSelect || (value != SelectionModeOptions.MultiSelect && _selectionMode == SelectionModeOptions.MultiSelect))
			{
				SelectedItems.ForEach(delegate(ItemViewModel item)
				{
					item.ParentViewModel = null;
				});
				SelectedItems.Clear();
			}
			_selectionMode = value;
			OnPropertyChanged("SelectionMode");
		}
	}

	public string TableHeader
	{
		get
		{
			return _tableHeader;
		}
		set
		{
			_tableHeader = value;
			OnPropertyChanged("TableHeader");
			OnPropertyChanged("TableColumns");
		}
	}

	public string SearchPlaceholderText
	{
		get
		{
			return _searchPlaceholderText;
		}
		set
		{
			_searchPlaceholderText = value;
			OnPropertyChanged("SearchPlaceholderText");
		}
	}

	public string RemoveAllButtonText
	{
		get
		{
			return _removeAllButtonText;
		}
		set
		{
			_removeAllButtonText = value;
			OnPropertyChanged("RemoveAllButtonText");
		}
	}

	public string RemoveActionTooltip
	{
		get
		{
			return _removeActionTooltip;
		}
		set
		{
			_removeActionTooltip = value;
			OnPropertyChanged("RemoveActionTooltip");
		}
	}

	public string EmptyListOverlayText
	{
		get
		{
			return _emptyListOverlayText;
		}
		set
		{
			_emptyListOverlayText = value;
			OnPropertyChanged("EmptyListOverlayText");
		}
	}

	public Collection<VideoOSTableColumnBase> TableColumns => new ObservableCollection<VideoOSTableColumnBase>
	{
		new VideoOSTableIconAndTextColumn
		{
			ItemIconSourcePropertyName = "IconSource",
			Header = _tableHeader,
			IsSortable = true,
			ItemSortPropertyName = "Name"
		},
		new VideoOSTableActionsColumn
		{
			ItemActionsPropertyName = "Actions",
			Header = "",
			Width = DataGridLength.Auto,
			CanUserResize = false,
			HorizontalAlignment = HorizontalAlignment.Right
		}
	};

	public bool IsMultiSelection
	{
		get
		{
			return SelectionModeOptions.MultiSelect == SelectionMode;
		}
		set
		{
			SelectionMode = ((!value) ? SelectionModeOptions.SingleSelect : SelectionModeOptions.MultiSelect);
		}
	}

	public bool AllowGroupSelection { get; set; }

	public event SelectedItemChangedEvent SelectedItemChanged;

	public void Populate()
	{
		if (ItemViewModels != null)
		{
			ItemViewModels.ForEach(delegate(ItemViewModel item)
			{
				UnsubscribeAllInChain(item);
			});
			ItemViewModels.Clear();
		}
		lock (_lockToken)
		{
			if (_lastCancellationTokenSource != null)
			{
				_lastCancellationTokenSource.Cancel();
			}
		}
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		PopulateThread populateThread = new PopulateThread();
		populateThread.SearchString = _searchString;
		populateThread.SearchSplit = _searchSplit;
		populateThread.SearchFilter = _searchFilter;
		populateThread.RemoveActionTooltip = _removeActionTooltip;
		populateThread.SourceItems = _sourceItems;
		populateThread.AutoExpand = AutoExpand;
		populateThread.IsVisibleCallback = IsVisibleCallback;
		if ((_expandBeforeSearch.HasValue && _expandBeforeSearch.Value) || (!_expandBeforeSearch.HasValue && AutoExpand))
		{
			populateThread.OverwriteAutoExpand = true;
		}
		populateThread.KindsFilter = KindsFilter;
		populateThread.Token = cancellationTokenSource.Token;
		lock (_lockToken)
		{
			if (_lastCancellationTokenSource != null)
			{
				_lastCancellationTokenSource.Dispose();
			}
			_lastCancellationTokenSource = cancellationTokenSource;
		}
		populateThread.OnCompletionDelegate = NotifyOnCompletion;
		new Thread(populateThread.Populate).Start();
	}

	private void NotifyOnCompletion(PopulateThread populateThread)
	{
		lock (_lockToken)
		{
			if (_lastCancellationTokenSource.Token == populateThread.Token)
			{
				_lastCancellationTokenSource.Dispose();
				_lastCancellationTokenSource = null;
			}
		}
		ObservableCollection<ItemViewModel> populatedListOfItems = populateThread.ViewModels;
		ItemViewModels.ForEach(delegate(ItemViewModel item)
		{
			UnsubscribeAllInChain(item);
		});
		populatedListOfItems.ForEach(delegate(ItemViewModel item)
		{
			SubscribeAllInChain(item);
		});
		SetMouseCursor(busy: true);
		Application.Current.Dispatcher.Invoke(delegate
		{
			ItemViewModels = new ObservableCollection<ItemViewModel>(populatedListOfItems);
		});
		SetMouseCursor(busy: false);
		SetPreSelectedItemsCollection();
		if (!AutoCloseOnSelectModeReady)
		{
			AutoCloseOnSelectModeReady = true;
		}
	}

	private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "AutoExpand")
		{
			ReinforceHighlight();
		}
	}

	private void SubscribeAllInChain(ItemViewModel item)
	{
		item.PropertyChanged += ItemPropertyChanged;
		foreach (ItemViewModel childItem in item.ChildItems)
		{
			SubscribeAllInChain(childItem);
		}
	}

	private void UnsubscribeAllInChain(ItemViewModel item)
	{
		item.PropertyChanged -= ItemPropertyChanged;
		foreach (ItemViewModel childItem in item.ChildItems)
		{
			UnsubscribeAllInChain(childItem);
		}
	}

	private void SetPreSelectedItemsCollection()
	{
		if (!PreSelectedItemsCollection.Any() || !ItemViewModels.Any())
		{
			return;
		}
		foreach (Item item in PreSelectedItemsCollection)
		{
			if (item != null)
			{
				HelpSetSelectedItems(item, ItemViewModels);
			}
		}
	}

	private void HelpSetSelectedItems(Item item, ObservableCollection<ItemViewModel> ivm)
	{
		foreach (ItemViewModel item2 in ivm)
		{
			if (item2.ChildItems.Any())
			{
				HelpSetSelectedItems(item, item2.ChildItems);
			}
			AddItemToSelectedItems(item2, item);
		}
	}

	private void AddItemToSelectedItems(ItemViewModel ivm, Item item)
	{
		if (SelectedItems.Select((ItemViewModel x) => x.InternalItem.FQID.ObjectId).Contains(ivm.InternalItem.FQID.ObjectId) || !(ivm.InternalItem.FQID == item.FQID))
		{
			return;
		}
		Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)delegate
		{
			SelectedItems.Add(ivm);
			ivm.IsSelected = true;
			ivm.ParentViewModel = this;
			if (IsMultiSelection)
			{
				EmptyListOverlayVisibility = Visibility.Collapsed;
			}
		});
	}

	public bool IsVisibleAfterSearch(Item item)
	{
		if (ValidateSearch(item, _searchFilter, _searchSplit))
		{
			return true;
		}
		if (HasVisibleChildren(item))
		{
			return true;
		}
		return false;
	}

	public bool IsVisibleAfterSearch(ItemViewModel ivm)
	{
		return IsVisibleAfterSearch(ivm.InternalItem);
	}

	public bool HasVisibleChildren(Item item)
	{
		if (item.HasChildren == HasChildren.No)
		{
			return false;
		}
		foreach (Item child in item.GetChildren())
		{
			if (IsVisibleCallback == null || IsVisibleCallback(item))
			{
				if (ValidateSearch(child, _searchFilter, _searchSplit))
				{
					return true;
				}
				if (HasVisibleChildren(child))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsCriteriaAccepted(ItemViewModel selectedItem)
	{
		if (ValidateFolderState(selectedItem) && ValidateItemKind(selectedItem))
		{
			return ValidateCustomValidationState(selectedItem);
		}
		return false;
	}

	private bool ValidateFolderState(ItemViewModel selectedItem)
	{
		if (!selectedItem.IsFolder())
		{
			return selectedItem.InternalItem.FQID.Kind != Kind.Folder;
		}
		return false;
	}

	private bool ValidateItemKind(ItemViewModel selectedItem)
	{
		if (KindsFilter.Count == 0)
		{
			return true;
		}
		return selectedItem.IsKindValid(KindsFilter);
	}

	private bool ValidateCustomValidationState(ItemViewModel selectedItem)
	{
		if (IsValidSelectionCallback != null)
		{
			return IsValidSelectionCallback(selectedItem.InternalItem);
		}
		return true;
	}

	private ItemViewModel ReplacementFromCallback(ItemViewModel selectedItem)
	{
		if (ReplaceSelectionCallback == null)
		{
			return selectedItem;
		}
		return ReplaceItemViewModel(selectedItem);
	}

	private ItemViewModel ReplaceItemViewModel(ItemViewModel selectedItem)
	{
		Item item = ReplaceSelectionCallback(selectedItem.InternalItem);
		if (item != null)
		{
			if (AllowGroupSelection && selectedItem.IsFolder())
			{
				return new ItemViewModel(RemoveActionTooltip)
				{
					InternalItem = item
				};
			}
			return new ItemViewModel(RemoveActionTooltip)
			{
				InternalItem = item,
				ChildItems = selectedItem.ChildItems
			};
		}
		return null;
	}

	private bool ValidateSearch(Item item, bool searchValid, string[] searchSplit)
	{
		if (searchValid)
		{
			foreach (string text in searchSplit)
			{
				if (!item.Name.ToLower(CultureInfo.CurrentCulture).Contains(text.ToLower(CultureInfo.CurrentCulture)))
				{
					return false;
				}
			}
		}
		return true;
	}

	public void AppendItemPicked(ItemViewModel selectedItem)
	{
		switch (SelectionMode)
		{
		case SelectionModeOptions.MultiSelect:
			MultiSelectionItemSelected(selectedItem);
			break;
		case SelectionModeOptions.SingleSelect:
			SingleSelectionItemSelected(selectedItem);
			break;
		case SelectionModeOptions.AutoCloseOnSelect:
			AutoSelectionItemSelected(selectedItem);
			break;
		default:
			throw new Exception(SelectionMode.ToString());
		}
	}

	private void AutoSelectionItemSelected(ItemViewModel selectedItem)
	{
		selectedItem = ReplacementFromCallback(selectedItem);
		if (selectedItem != null)
		{
			if (IsCriteriaAccepted(selectedItem))
			{
				AddSingleValidSelectedItem(selectedItem);
			}
			else if (AllowGroupSelection && selectedItem.IsFolder() && ValidateCustomValidationState(selectedItem))
			{
				AddSingleValidSelectedItem(selectedItem);
			}
		}
	}

	private void SingleSelectionItemSelected(ItemViewModel selectedItem)
	{
		selectedItem = ReplacementFromCallback(selectedItem);
		if (selectedItem != null)
		{
			if (IsCriteriaAccepted(selectedItem) || (AllowGroupSelection && selectedItem.IsFolder() && ValidateCustomValidationState(selectedItem)))
			{
				AddSingleValidSelectedItem(selectedItem);
			}
			else
			{
				ReinforceHighlight();
			}
		}
	}

	public void RemoveSelectedItem(ItemViewModel itemViewModel)
	{
		itemViewModel.IsSelected = false;
		itemViewModel.ParentViewModel = null;
		SelectedItems.Remove(itemViewModel);
		if (!SelectedItems.Any())
		{
			EmptyListOverlayVisibility = Visibility.Visible;
		}
	}

	private void MultiSelectionItemSelected(ItemViewModel selectedItem)
	{
		selectedItem = ReplacementFromCallback(selectedItem);
		if (selectedItem == null)
		{
			return;
		}
		if (IsCriteriaAccepted(selectedItem))
		{
			if (!SelectedItems.Any((ItemViewModel x) => x.InternalItem.FQID.ObjectId == selectedItem.InternalItem.FQID.ObjectId))
			{
				AddSingleValidSelectedItem(selectedItem);
			}
			selectedItem.IsSelected = true;
		}
		else if (selectedItem.IsFolder())
		{
			if (AllowGroupSelection)
			{
				AddSingleValidSelectedItem(selectedItem);
			}
			else
			{
				AddAllChildren(selectedItem);
			}
		}
	}

	private void AddAllChildren(ItemViewModel selectedItem)
	{
		foreach (ItemViewModel childItem in selectedItem.ChildItems)
		{
			ItemViewModel internalChild = new ItemViewModel(RemoveActionTooltip)
			{
				InternalItem = childItem.InternalItem
			};
			internalChild = ReplacementFromCallback(internalChild);
			if (internalChild != null && IsCriteriaAccepted(internalChild) && IsVisibleAfterSearch(internalChild) && !SelectedItems.Any((ItemViewModel x) => x.InternalItem.FQID.ObjectId == internalChild.InternalItem.FQID.ObjectId))
			{
				AddSingleValidSelectedItem(internalChild);
			}
		}
	}

	private void AddSingleValidSelectedItem(ItemViewModel selectedItem)
	{
		selectedItem.ParentViewModel = this;
		if (!IsMultiSelection && SelectedItems.Any())
		{
			SelectedItems.ForEach(delegate(ItemViewModel item)
			{
				item.ParentViewModel = null;
			});
			SelectedItems.Clear();
		}
		SelectedItems.Add(selectedItem);
		selectedItem.IsSelected = true;
		if (IsMultiSelection)
		{
			EmptyListOverlayVisibility = Visibility.Collapsed;
		}
	}

	public void ReinforceHighlight()
	{
		if (SelectedItems.Count > 0)
		{
			SelectedItems.Last().IsSelected = true;
		}
	}

	private void HandleSelectedItemChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		if (this.SelectedItemChanged != null)
		{
			this.SelectedItemChanged(this, EventArgs.Empty);
		}
	}

	public void RemoveAllSelectedItems()
	{
		while (SelectedItems.Any())
		{
			ItemViewModel itemViewModel = SelectedItems[0];
			itemViewModel.IsSelected = false;
			SelectedItems.Remove(itemViewModel);
			EmptyListOverlayVisibility = Visibility.Visible;
		}
	}

	public void SetMouseCursor(bool busy)
	{
		Cursor cursor = (busy ? Cursors.Wait : null);
		Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			Mouse.OverrideCursor = cursor;
		});
	}

	public void Dispose()
	{
		_selectedItems.CollectionChanged -= HandleSelectedItemChanged;
		_lastCancellationTokenSource?.Dispose();
	}
}
