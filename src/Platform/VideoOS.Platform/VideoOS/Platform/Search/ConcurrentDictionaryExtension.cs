using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace VideoOS.Platform.Search;

public static class ConcurrentDictionaryExtension
{
	public static bool TryRemove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue value)
	{
		if (dictionary == null)
		{
			throw new ArgumentNullException("dictionary");
		}
		return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Remove(new KeyValuePair<TKey, TValue>(key, value));
	}
}
