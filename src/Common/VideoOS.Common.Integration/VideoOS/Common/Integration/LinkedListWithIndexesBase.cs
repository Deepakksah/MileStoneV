using System;
using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.Integration;

public abstract class LinkedListWithIndexesBase<T> : IEnumerable<T>, IEnumerable
{
	private LinkedList<T> items;

	protected LinkedListWithIndexesBase()
	{
		items = new LinkedList<T>();
	}

	public void Add(T item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		LinkedListNode<T> linkedListNode = items.AddLast(item);
		try
		{
			AddEntryToIndexes(linkedListNode);
		}
		catch (Exception)
		{
			items.Remove(linkedListNode);
			throw;
		}
	}

	public bool Remove(T item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		LinkedListNode<T> linkedListNode = FindEntry(item);
		if (linkedListNode != null)
		{
			items.Remove(linkedListNode);
			RemoveEntryFromIndexes(linkedListNode);
			return true;
		}
		return false;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return items.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return items.GetEnumerator();
	}

	protected abstract LinkedListNode<T> FindEntry(T item);

	protected abstract void AddEntryToIndexes(LinkedListNode<T> entry);

	protected abstract void RemoveEntryFromIndexes(LinkedListNode<T> entry);
}
