using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using VideoOS.Platform.UI.ItemPicker.Data;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.UI.ItemPicker.Utilities;

public class PopulateThread
{
	public delegate void OnCompletion(PopulateThread populateThread);

	public const int MaxAutoExpandItemCount = 20;

	private List<ItemViewModel> _unfilteredTree = new List<ItemViewModel>();

	private int _shownItemCounter;

	public Func<Item, bool> IsVisibleCallback { get; set; }

	public string SearchString { get; set; }

	public string[] SearchSplit { get; set; }

	public bool SearchFilter { get; set; }

	public string RemoveActionTooltip { get; set; }

	public IEnumerable<Item> SourceItems { get; set; }

	public bool AutoExpand { get; set; }

	public bool OverwriteAutoExpand { get; set; }

	public List<Guid> KindsFilter { get; set; }

	public CancellationToken Token { get; set; }

	public ObservableCollection<ItemViewModel> ViewModels { get; set; }

	public OnCompletion OnCompletionDelegate { get; set; }

	private List<Item> SortInternal(IEnumerable<Item> items)
	{
		if (items == null)
		{
			return new List<Item>();
		}
		List<Item> list = items.Where((Item x) => x != null).ToList();
		list.Sort((Item i1, Item i2) => Sort.NumericStringCompare(i1.Name, i2.Name));
		return list;
	}

	public void Populate()
	{
		PopulateInternal();
		if (!Token.IsCancellationRequested)
		{
			OnCompletionDelegate(this);
		}
	}

	private void PopulateInternal()
	{
		_unfilteredTree = CreateUnfilteredTree();
		FilterTree();
		PruneTree();
	}

	private void PruneTree()
	{
		if (Token.IsCancellationRequested)
		{
			return;
		}
		ObservableCollection<ItemViewModel> observableCollection = new ObservableCollection<ItemViewModel>();
		foreach (ItemViewModel item in _unfilteredTree)
		{
			if (!item.Prune)
			{
				observableCollection.Add(Prune(item));
			}
		}
		ViewModels = observableCollection;
	}

	private ItemViewModel Prune(ItemViewModel model)
	{
		if (Token.IsCancellationRequested)
		{
			return null;
		}
		ItemViewModel itemViewModel = model.DeepCopy();
		if (_shownItemCounter > 20 && !OverwriteAutoExpand)
		{
			itemViewModel.AutoExpand = false;
		}
		ObservableCollection<ItemViewModel> childItems = itemViewModel.ChildItems;
		ObservableCollection<ItemViewModel> observableCollection = new ObservableCollection<ItemViewModel>();
		foreach (ItemViewModel item in childItems)
		{
			if (!item.Prune)
			{
				observableCollection.Add(Prune(item));
			}
		}
		itemViewModel.ChildItems = observableCollection;
		return itemViewModel;
	}

	private void FilterTree()
	{
		if (Token.IsCancellationRequested)
		{
			return;
		}
		foreach (ItemViewModel item in _unfilteredTree)
		{
			Filter(item);
		}
	}

	private void Filter(ItemViewModel item)
	{
		if (Token.IsCancellationRequested)
		{
			return;
		}
		foreach (ItemViewModel childItem in item.ChildItems)
		{
			Filter(childItem);
		}
		if (!item.ChildItems.Any((ItemViewModel x) => !x.Prune))
		{
			if (!IsIndependentlyVisible(item))
			{
				item.Prune = true;
				return;
			}
			item.Prune = false;
			_shownItemCounter++;
		}
		else
		{
			item.Prune = false;
		}
	}

	private bool IsIndependentlyVisible(ItemViewModel item)
	{
		if (Token.IsCancellationRequested)
		{
			return false;
		}
		bool flag = true;
		if (KindsFilter.Any() && !KindsFilter.Any((Guid x) => x.Equals(item.InternalItem?.FQID?.Kind)))
		{
			flag = false;
		}
		if (flag && IsVisibleCallback != null && !IsVisibleCallback(item.InternalItem))
		{
			flag = false;
		}
		if (flag && SearchFilter && SearchSplit.Any((string x) => !item.Name.ToLower(CultureInfo.CurrentCulture).Contains(x.ToLower(CultureInfo.CurrentCulture))))
		{
			flag = false;
		}
		return flag;
	}

	private List<ItemViewModel> CreateUnfilteredTree()
	{
		if (Token.IsCancellationRequested || SourceItems == null)
		{
			return null;
		}
		List<ItemViewModel> result = new List<ItemViewModel>();
		foreach (Item item in SortInternal(SourceItems))
		{
			AddUnfilteredItem(result, item);
		}
		return result;
	}

	private void AddUnfilteredItem(List<ItemViewModel> result, Item item)
	{
		if (Token.IsCancellationRequested || item == null)
		{
			return;
		}
		try
		{
			ItemViewModel itemViewModel = new ItemViewModel(RemoveActionTooltip);
			itemViewModel.InternalItem = item;
			itemViewModel.AutoExpand = AutoExpand;
			result.Add(itemViewModel);
			List<Item> list = SortInternal(item.GetChildren());
			if (list.Count <= 0)
			{
				return;
			}
			List<ItemViewModel> list2 = new List<ItemViewModel>();
			foreach (Item item2 in list)
			{
				AddUnfilteredItem(list2, item2);
			}
			itemViewModel.ChildItems = new ObservableCollection<ItemViewModel>(list2);
		}
		catch (ArgumentException)
		{
		}
		catch (NullReferenceException)
		{
		}
	}
}
