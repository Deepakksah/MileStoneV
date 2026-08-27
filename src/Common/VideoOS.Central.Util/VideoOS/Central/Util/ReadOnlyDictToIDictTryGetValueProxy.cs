using System.Collections.Generic;

namespace VideoOS.Central.Util;

public class ReadOnlyDictToIDictTryGetValueProxy<TKey, TValue> : IDictTryGetValue<TKey, TValue>
{
	private readonly IReadOnlyDictionary<TKey, TValue> _obj;

	public ReadOnlyDictToIDictTryGetValueProxy(IReadOnlyDictionary<TKey, TValue> obj)
	{
		_obj = obj;
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		return _obj.TryGetValue(key, out value);
	}
}
