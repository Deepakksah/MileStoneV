using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Management.VmoClient;

public sealed class ScopesCollection : ICollection<string>, IEnumerable<string>, IEnumerable
{
	private List<string> _values = new List<string>();

	public int Count => _values.Count;

	public bool IsReadOnly => false;

	public event EventHandler CollectionChanged;

	public ScopesCollection(IEnumerable<string> scopes)
	{
		_values.AddRange(scopes);
	}

	public void OnCollectionChanged()
	{
		this.CollectionChanged?.Invoke(this, EventArgs.Empty);
	}

	public void Add(string item)
	{
		_values.Add(item);
		OnCollectionChanged();
	}

	public void Clear()
	{
		if (_values.Any())
		{
			_values.Clear();
			OnCollectionChanged();
		}
	}

	public bool Contains(string item)
	{
		return _values.Contains(item);
	}

	public void CopyTo(string[] array, int arrayIndex)
	{
		_values.CopyTo(array, arrayIndex);
	}

	public bool Remove(string item)
	{
		if (_values.Remove(item))
		{
			OnCollectionChanged();
			return true;
		}
		return false;
	}

	public IEnumerator<string> GetEnumerator()
	{
		return _values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
