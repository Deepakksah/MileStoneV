using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Platform.SDK.Config;

public class InternalList<T> : IEnumerable<T>, IEnumerable
{
	private List<T> _listOfTypes;

	public int Count
	{
		get
		{
			lock (this)
			{
				return _listOfTypes.Count;
			}
		}
	}

	public T this[int index]
	{
		get
		{
			lock (this)
			{
				return _listOfTypes[index];
			}
		}
	}

	public bool Contains(T item)
	{
		lock (this)
		{
			return _listOfTypes.Contains(item);
		}
	}

	public T[] ToArray()
	{
		lock (this)
		{
			return _listOfTypes.ToArray();
		}
	}

	public InternalList()
	{
		_listOfTypes = new List<T>();
	}

	public void Add(T item)
	{
		lock (this)
		{
			_listOfTypes.Add(item);
		}
	}

	public bool Remove(T item)
	{
		lock (this)
		{
			return _listOfTypes.Remove(item);
		}
	}

	public void Clear()
	{
		lock (this)
		{
			_listOfTypes.Clear();
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		lock (this)
		{
			return _listOfTypes.GetEnumerator();
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		lock (this)
		{
			return _listOfTypes.GetEnumerator();
		}
	}
}
