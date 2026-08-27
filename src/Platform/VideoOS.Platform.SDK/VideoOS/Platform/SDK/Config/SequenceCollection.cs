using System;
using System.Collections;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

namespace VideoOS.Platform.SDK.Config;

public class SequenceCollection : IList<Sequence>, ICollection<Sequence>, IEnumerable<Sequence>, IEnumerable
{
	private List<Sequence> _list = new List<Sequence>();

	public Sequence this[int index]
	{
		get
		{
			return _list[index];
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public int Count => _list.Count;

	bool ICollection<Sequence>.IsReadOnly => true;

	public void CreateSequenceCollection(SequenceEntry[] sequences)
	{
		foreach (SequenceEntry sequenceEntry in sequences)
		{
			Sequence item = new Sequence(EnsureUtc(sequenceEntry.TimeBegin), EnsureUtc(sequenceEntry.TimeTrigged), EnsureUtc(sequenceEntry.TimeEnd));
			_list.Add(item);
		}
	}

	private static DateTime EnsureUtc(DateTime time)
	{
		if (time.Kind == DateTimeKind.Utc)
		{
			return new DateTime(time.Ticks, DateTimeKind.Utc);
		}
		return new DateTime(time.ToUniversalTime().Ticks, DateTimeKind.Utc);
	}

	public int IndexOf(Sequence item)
	{
		return _list.IndexOf(item);
	}

	void IList<Sequence>.Insert(int index, Sequence item)
	{
		throw new NotSupportedException();
	}

	void IList<Sequence>.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	void ICollection<Sequence>.Add(Sequence item)
	{
		_list.Add(item);
	}

	void ICollection<Sequence>.Clear()
	{
		_list.Clear();
	}

	public void AddRange(List<Sequence> range)
	{
		_list.AddRange(range);
	}

	public bool Contains(Sequence item)
	{
		return _list.Contains(item);
	}

	void ICollection<Sequence>.CopyTo(Sequence[] array, int arrayIndex)
	{
		_list.CopyTo(array, arrayIndex);
	}

	bool ICollection<Sequence>.Remove(Sequence item)
	{
		throw new NotSupportedException();
	}

	public void Remove(int index)
	{
		_list.RemoveAt(index);
	}

	public IEnumerator<Sequence> GetEnumerator()
	{
		return _list.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _list.GetEnumerator();
	}
}
