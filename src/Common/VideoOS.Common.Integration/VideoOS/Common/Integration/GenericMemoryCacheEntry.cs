using System;

namespace VideoOS.Common.Integration;

public class GenericMemoryCacheEntry<TKey, TValue>
{
	private TKey _key;

	private TValue _value;

	private GenericMemoryCacheEntry<TKey, TValue> _next;

	private DateTime _timestamp;

	public TKey Key => _key;

	public TValue Value => _value;

	public GenericMemoryCacheEntry<TKey, TValue> Next
	{
		get
		{
			return _next;
		}
		set
		{
			_next = value;
		}
	}

	public DateTime Timestamp => _timestamp;

	public GenericMemoryCacheEntry(TKey key, TValue value)
	{
		_key = key;
		_value = value;
		_timestamp = DateTime.Now;
	}
}
