using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Utilities;

public class ItemManager<TKey, TItem> : IItemManagerAddRemove<TKey, TItem>
{
	private readonly Dictionary<TKey, TItem> _items = new Dictionary<TKey, TItem>();

	private readonly object _itemsLocker = new object();

	public Action OnChange { get; set; }

	private void ChangeHappened()
	{
		try
		{
			if (OnChange != null)
			{
				OnChange();
			}
		}
		catch (Exception ex)
		{
			Log.Error("ItemManager<TKey, TItem>.ChangeHappened()", ex.Message);
		}
	}

	public void ClearItems(Action<TKey, TItem> action = null)
	{
		int num = 0;
		lock (_itemsLocker)
		{
			num = _items.Count;
			if (action != null)
			{
				foreach (KeyValuePair<TKey, TItem> item in _items)
				{
					action(item.Key, item.Value);
				}
			}
			_items.Clear();
		}
		if (num > 0)
		{
			ChangeHappened();
		}
	}

	public void Add(TKey key, TItem item)
	{
		lock (_itemsLocker)
		{
			_items[key] = item;
		}
		ChangeHappened();
	}

	public TItem RemoveItem(TKey key)
	{
		TItem item = GetItem(key, delegate(TKey keyT, TItem val)
		{
			_items.Remove(keyT);
		});
		ChangeHappened();
		return item;
	}

	public TItem GetItem(TKey key)
	{
		return GetItem(key, null);
	}

	private TItem GetItem(TKey key, Action<TKey, TItem> action)
	{
		TItem val = default(TItem);
		lock (_itemsLocker)
		{
			if (_items.ContainsKey(key))
			{
				val = _items[key];
				action?.Invoke(key, val);
			}
		}
		return val;
	}

	public void IterateItems(Action<TKey, TItem> callback)
	{
		Dictionary<TKey, TItem> dictionary;
		lock (_itemsLocker)
		{
			dictionary = new Dictionary<TKey, TItem>(_items);
		}
		foreach (KeyValuePair<TKey, TItem> item in dictionary)
		{
			try
			{
				callback(item.Key, item.Value);
			}
			catch (Exception ex)
			{
				Log.Error("ItemsManager.IterateEntries", ex.Message);
			}
		}
	}

	public void UpdateItems(Action<Dictionary<TKey, TItem>> updater)
	{
		lock (_itemsLocker)
		{
			try
			{
				updater(_items);
			}
			catch (Exception ex)
			{
				Log.Error("ItemManager.UpdateItems", ex.Message);
			}
		}
		ChangeHappened();
	}

	public void UpdateItems(IDictionary<TKey, TItem> other, Action<Dictionary<TKey, TItem>, IDictionary<TKey, TItem>> updater)
	{
		UpdateItems(delegate(Dictionary<TKey, TItem> items)
		{
			updater(items, other);
		});
	}
}
