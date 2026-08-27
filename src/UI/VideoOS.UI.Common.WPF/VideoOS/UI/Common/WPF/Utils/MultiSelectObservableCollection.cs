using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace VideoOS.UI.Common.WPF.Utils;

public class MultiSelectObservableCollection<T> : ObservableCollection<T> where T : class, ISelectable
{
	private readonly ObservableCollection<T> _selectedItems = new ObservableCollection<T>();

	public ObservableCollection<T> SelectedItems => _selectedItems;

	protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
	{
		base.OnCollectionChanged(e);
		switch (e.Action)
		{
		case NotifyCollectionChangedAction.Add:
			UpdateSelectionWithNewItems(e.NewItems.OfType<T>());
			break;
		case NotifyCollectionChangedAction.Remove:
			UpdateSelectionWithOldItems(e.OldItems.OfType<T>());
			break;
		case NotifyCollectionChangedAction.Replace:
			UpdateSelectionWithOldItems(e.OldItems.OfType<T>());
			UpdateSelectionWithNewItems(e.NewItems.OfType<T>());
			break;
		case NotifyCollectionChangedAction.Reset:
			_selectedItems.Clear();
			break;
		case NotifyCollectionChangedAction.Move:
			break;
		}
	}

	private void UpdateSelectionWithNewItems(IEnumerable<T> newItems)
	{
		foreach (T newItem in newItems)
		{
			if (newItem.IsSelected)
			{
				_selectedItems.Add(newItem);
			}
			IsSelectedChangedEventManager.AddHandler(newItem, selectable_IsSelectedChanged);
		}
	}

	private void UpdateSelectionWithOldItems(IEnumerable<T> oldItems)
	{
		foreach (T oldItem in oldItems)
		{
			if (oldItem.IsSelected)
			{
				_selectedItems.Remove(oldItem);
			}
			IsSelectedChangedEventManager.RemoveHandler(oldItem, selectable_IsSelectedChanged);
		}
	}

	private void selectable_IsSelectedChanged(object sender, EventArgs e)
	{
		T val = sender as T;
		if (val.IsSelected)
		{
			_selectedItems.Add(val);
		}
		else
		{
			_selectedItems.Remove(val);
		}
	}
}
