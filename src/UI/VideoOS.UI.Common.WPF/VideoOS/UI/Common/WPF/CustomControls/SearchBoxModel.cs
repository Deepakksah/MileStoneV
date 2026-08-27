using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using VideoOS.UI.Common.WPF.Resources;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

namespace VideoOS.UI.Common.WPF.CustomControls;

public class SearchBoxModel : ViewModelBase, IDisposable
{
	private readonly Dispatcher _uiDispatcher;

	private Guid _currentSearchId;

	private bool _initialized;

	private Timer _timer;

	private int _index = -1;

	private const int _timeInterval = 500;

	private ICommand _openPopupCommand;

	private ICommand _commandDownPressed;

	private ICommand _commandUpPressed;

	private ICommand _commandEnterPressed;

	private ICommand _gotFocusCommand;

	private string _watermarkText = Strings.SearchBoxWatermark;

	private SearchItem _selectedItem;

	private ObservableCollection<SearchItem> _searchResults;

	private string _noItemsFound = Strings.NoItemsFound;

	private string _searchText;

	private bool _isPopupOpen;

	private ISearcher _searcher;

	private IFunctionObject<SearchItem> _refreshDetailsList;

	public ICommand OpenPopupCommand => _openPopupCommand;

	public ICommand CommandDownPressed => _commandDownPressed;

	public ICommand CommandUpPressed => _commandUpPressed;

	public ICommand CommandEnterPressed => _commandEnterPressed;

	public ICommand GotFocusCommand => _gotFocusCommand;

	public string WatermarkText
	{
		get
		{
			return _watermarkText;
		}
		set
		{
			_watermarkText = value;
		}
	}

	public SearchItem SelectedItem
	{
		get
		{
			return _selectedItem;
		}
		set
		{
			SelectItem(value);
			_index = -1;
			IsPopupOpen = false;
			RefreshDetailsList();
		}
	}

	public ObservableCollection<SearchItem> SearchResult
	{
		get
		{
			return _searchResults;
		}
		set
		{
			_index = -1;
			if (value != null && value.Count == 0)
			{
				_searchResults = new ObservableCollection<SearchItem>
				{
					new SearchItem
					{
						Id = string.Empty,
						Name = NoItemsFoundText,
						Path = string.Empty
					}
				};
			}
			else
			{
				_searchResults = value;
				_index = 0;
				SelectItem(_searchResults[_index]);
			}
			if (_currentSearchId != Guid.Empty)
			{
				IsPopupOpen = true;
			}
			OnPropertyChanged("SearchResult");
		}
	}

	public string NoItemsFoundText
	{
		get
		{
			return _noItemsFound;
		}
		set
		{
			_noItemsFound = value;
		}
	}

	public string SearchText
	{
		get
		{
			return _searchText;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				IsPopupOpen = false;
			}
			if (SetProperty(ref _searchText, value, "SearchText"))
			{
				InvalidateSearch();
			}
		}
	}

	public bool IsPopupOpen
	{
		get
		{
			return _isPopupOpen;
		}
		set
		{
			_isPopupOpen = value;
			OnPropertyChanged("IsPopupOpen");
		}
	}

	public SearchBoxModel(Dispatcher uiDispatcher, ISearcher searcher, IFunctionObject<SearchItem> refreshDetailsList)
	{
		_uiDispatcher = uiDispatcher;
		_openPopupCommand = new DelegateCommand(delegate
		{
			OpenClosePopup();
		});
		_commandDownPressed = new DelegateCommand(delegate
		{
			SelectDownItem();
		});
		_commandUpPressed = new DelegateCommand(delegate
		{
			SelectUpItem();
		});
		_commandEnterPressed = new DelegateCommand(delegate
		{
			SelectItemFromEnter();
		});
		_gotFocusCommand = new DelegateCommand(delegate
		{
			SelectItemFromEnter();
		});
		_refreshDetailsList = refreshDetailsList;
		SearchResult = new ObservableCollection<SearchItem>();
		_searcher = searcher;
		_searcher.SearchCompleted += _searcher_SearchCompleted;
		_timer = new Timer();
		_timer.Enabled = false;
		_timer.Interval = 500;
		_timer.Tick += _timer_Tick;
		_initialized = true;
	}

	private void SelectItemFromEnter()
	{
		if (_index != -1 && _index < SearchResult.Count)
		{
			SelectedItem = SearchResult[_index];
		}
	}

	private void SelectItem(SearchItem value)
	{
		_selectedItem = value;
		OnPropertyChanged("SelectedItem");
	}

	private void SelectDownItem()
	{
		_index++;
		if (_index > SearchResult.Count - 1)
		{
			_index = 0;
		}
		SelectItem(SearchResult[_index]);
	}

	private void SelectUpItem()
	{
		if (_index != -1)
		{
			_index--;
			if (_index < 0)
			{
				_index = SearchResult.Count - 1;
			}
			SelectItem(SearchResult[_index]);
		}
	}

	private void OpenClosePopup()
	{
		IsPopupOpen = SearchResult.Count > 0;
	}

	private void _searcher_SearchCompleted(object sender, SearchResult e)
	{
		if (_uiDispatcher != Dispatcher.CurrentDispatcher)
		{
			_uiDispatcher.BeginInvoke((MethodInvoker)delegate
			{
				_searcher_SearchCompleted(sender, e);
			});
		}
		else if (e.SearchId == _currentSearchId)
		{
			Mouse.OverrideCursor = null;
			SearchResult = new ObservableCollection<SearchItem>(e.Items);
		}
	}

	private void InvalidateSearch()
	{
		if (_initialized)
		{
			if (string.IsNullOrEmpty(SearchText))
			{
				_timer.Enabled = false;
				return;
			}
			_timer.Enabled = true;
			_timer.Stop();
			_timer.Start();
		}
	}

	private void _timer_Tick(object sender, EventArgs e)
	{
		Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
		_uiDispatcher.BeginInvoke((MethodInvoker)delegate
		{
			Search(SearchText);
		});
		_timer.Enabled = false;
	}

	public void Search(string searchText)
	{
		_currentSearchId = _searcher.Search(searchText);
	}

	public void ClearSearch()
	{
		SearchResult.Clear();
	}

	private void RefreshDetailsList()
	{
		if (_selectedItem != null && !string.IsNullOrEmpty(_selectedItem.Id))
		{
			_searchText = _selectedItem.Name;
			OnPropertyChanged("SearchText");
			_refreshDetailsList.Do(_selectedItem);
		}
	}

	public void Dispose()
	{
		_searcher.SearchCompleted -= _searcher_SearchCompleted;
		_timer.Dispose();
	}

	public void SetSearchText(string searchText)
	{
		_searchText = searchText;
		OnPropertyChanged("SearchText");
	}
}
