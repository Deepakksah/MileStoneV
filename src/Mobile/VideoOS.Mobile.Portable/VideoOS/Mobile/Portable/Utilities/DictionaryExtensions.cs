using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Mobile.Portable.Utilities;

public static class DictionaryExtensions
{
	public static TValue GetValueByKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
	{
		if (dictionary.ContainsKey(key))
		{
			return dictionary[key];
		}
		return default(TValue);
	}

	public static Dictionary<TKey, TValue> Combine<TKey, TValue>(this IEnumerable<Dictionary<TKey, TValue>> dictionaries)
	{
		return dictionaries.SelectMany((Dictionary<TKey, TValue> dict) => dict).ToLookup((KeyValuePair<TKey, TValue> pair) => pair.Key, (KeyValuePair<TKey, TValue> pair) => pair.Value).ToDictionary((IGrouping<TKey, TValue> group) => group.Key, (IGrouping<TKey, TValue> group) => group.First());
	}

	public static IEnumerable<TKey> SafeExcept<TKey, TValue>(this Dictionary<TKey, TValue> source1, Dictionary<TKey, TValue> source2, IEqualityComparer<TKey> comparer = null)
	{
		if (source1 == null)
		{
			return Enumerable.Empty<TKey>();
		}
		if (source2 == null)
		{
			return source1.Keys;
		}
		return source1.Keys.Except(source2.Keys, comparer);
	}

	public static IEnumerable<TKey> SafeIntersect<TKey, TValue>(this Dictionary<TKey, TValue> source1, Dictionary<TKey, TValue> source2, IEqualityComparer<TKey> comparer = null)
	{
		if (source1 == null || source2 == null)
		{
			return Enumerable.Empty<TKey>();
		}
		return source1.Keys.Intersect(source2.Keys, comparer);
	}
}
