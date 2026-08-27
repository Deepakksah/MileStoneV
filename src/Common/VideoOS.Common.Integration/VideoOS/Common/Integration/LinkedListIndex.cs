using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Common.Integration;

public class LinkedListIndex<TKey, TValue>
{
	private readonly object _lockObject = new object();

	private readonly Dictionary<TKey, List<LinkedListNode<TValue>>> _items;

	private Func<TValue, TKey> _keyProvider;

	public LinkedListIndex(Func<TValue, TKey> keyProvider)
	{
		if (keyProvider == null)
		{
			throw new ArgumentNullException("keyProvider");
		}
		_items = new Dictionary<TKey, List<LinkedListNode<TValue>>>();
		_keyProvider = keyProvider;
	}

	public LinkedListIndex(Func<TValue, TKey> keyProvider, IEqualityComparer<TKey> comparer)
	{
		if (keyProvider == null)
		{
			throw new ArgumentNullException("keyProvider");
		}
		if (comparer == null)
		{
			throw new ArgumentNullException("comparer");
		}
		_items = new Dictionary<TKey, List<LinkedListNode<TValue>>>(comparer);
		_keyProvider = keyProvider;
	}

	public void AddItem(LinkedListNode<TValue> item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		TKey key = _keyProvider(item.Value);
		lock (_lockObject)
		{
			if (!_items.TryGetValue(key, out var value))
			{
				value = new List<LinkedListNode<TValue>>();
				_items.Add(key, value);
			}
			value.Add(item);
		}
	}

	public IEnumerable<LinkedListNode<TValue>> GetItemsByKey(TKey key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (_items.TryGetValue(key, out var value))
		{
			return value;
		}
		return Enumerable.Empty<LinkedListNode<TValue>>();
	}

	public void RemoveItem(LinkedListNode<TValue> item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		TKey key = _keyProvider(item.Value);
		lock (_lockObject)
		{
			if (_items.TryGetValue(key, out var value))
			{
				value.Remove(item);
				if (value.Count == 0)
				{
					_items.Remove(key);
				}
			}
		}
	}

	public bool RemoveKey(TKey key)
	{
		lock (_lockObject)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return _items.Remove(key);
		}
	}
}
