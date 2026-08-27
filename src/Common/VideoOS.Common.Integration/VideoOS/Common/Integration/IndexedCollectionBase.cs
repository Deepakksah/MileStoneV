using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Common.Integration;

public abstract class IndexedCollectionBase<TValue> : IEnumerable<TValue>, IEnumerable
{
	private interface IIndexEntry
	{
		LinkedListNode<TValue> Node { get; set; }
	}

	protected sealed class IndexEntry : IIndexEntry
	{
		private LinkedListNode<TValue> node;

		LinkedListNode<TValue> IIndexEntry.Node
		{
			get
			{
				return node;
			}
			set
			{
				node = value;
			}
		}

		public TValue Value { get; private set; }

		public IndexEntry(TValue value)
		{
			Value = value;
		}
	}

	protected sealed class Index<TKey>
	{
		private readonly Dictionary<TKey, List<IndexEntry>> _entries;

		private Func<IndexEntry, TKey> _keyProvider;

		public Index(Func<IndexEntry, TKey> keyProvider)
		{
			if (keyProvider == null)
			{
				throw new ArgumentNullException("keyProvider");
			}
			_entries = new Dictionary<TKey, List<IndexEntry>>();
			_keyProvider = keyProvider;
		}

		public Index(Func<IndexEntry, TKey> keyProvider, IEqualityComparer<TKey> comparer)
		{
			if (keyProvider == null)
			{
				throw new ArgumentNullException("keyProvider");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			_entries = new Dictionary<TKey, List<IndexEntry>>(comparer);
			_keyProvider = keyProvider;
		}

		public void AddEntry(IndexEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			TKey key = _keyProvider(entry);
			if (!_entries.TryGetValue(key, out var value))
			{
				value = new List<IndexEntry>();
				_entries.Add(key, value);
			}
			value.Add(entry);
		}

		public IEnumerable<IndexEntry> GetEntriesByKey(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (_entries.TryGetValue(key, out var value))
			{
				return value.ToList();
			}
			return Enumerable.Empty<IndexEntry>();
		}

		public bool RemoveEntry(IndexEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			TKey key = _keyProvider(entry);
			if (_entries.TryGetValue(key, out var value))
			{
				bool result = value.Remove(entry);
				if (value.Count == 0)
				{
					_entries.Remove(key);
				}
				return result;
			}
			return false;
		}

		public bool RemoveKey(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return _entries.Remove(key);
		}
	}

	protected sealed class UniqueIndex<TKey>
	{
		private readonly Dictionary<TKey, IndexEntry> _entries;

		private Func<IndexEntry, TKey> _keyProvider;

		public UniqueIndex(Func<IndexEntry, TKey> keyProvider)
		{
			if (keyProvider == null)
			{
				throw new ArgumentNullException("keyProvider");
			}
			_entries = new Dictionary<TKey, IndexEntry>();
			_keyProvider = keyProvider;
		}

		public UniqueIndex(Func<IndexEntry, TKey> keyProvider, IEqualityComparer<TKey> comparer)
		{
			if (keyProvider == null)
			{
				throw new ArgumentNullException("keyProvider");
			}
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			_entries = new Dictionary<TKey, IndexEntry>(comparer);
			_keyProvider = keyProvider;
		}

		public void AddEntry(IndexEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			TKey key = _keyProvider(entry);
			_entries.Add(key, entry);
		}

		public IndexEntry GetEntryByKey(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (_entries.TryGetValue(key, out var value))
			{
				return value;
			}
			return null;
		}

		public bool RemoveEntry(IndexEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			TKey key = _keyProvider(entry);
			return _entries.Remove(key);
		}
	}

	private LinkedList<TValue> items;

	protected IndexedCollectionBase()
	{
		items = new LinkedList<TValue>();
	}

	public void Add(TValue item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		LinkedListNode<TValue> node = items.AddLast(item);
		IndexEntry indexEntry = new IndexEntry(item);
		((IIndexEntry)indexEntry).Node = node;
		AddEntryToIndexes(indexEntry);
	}

	public bool Remove(TValue item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		IndexEntry indexEntry = FindEntry(item);
		if (indexEntry != null)
		{
			items.Remove(((IIndexEntry)indexEntry).Node);
			RemoveEntryFromIndexes(indexEntry);
			return true;
		}
		return false;
	}

	public IEnumerator<TValue> GetEnumerator()
	{
		return items.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return items.GetEnumerator();
	}

	protected abstract IndexEntry FindEntry(TValue item);

	protected abstract void AddEntryToIndexes(IndexEntry entry);

	protected abstract void RemoveEntryFromIndexes(IndexEntry entry);
}
