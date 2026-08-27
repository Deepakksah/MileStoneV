using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Mobile.Portable.Utilities;

public static class CollectionsHelper
{
	public static Dictionary<TKey, TValue> DeepCopy<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
	{
		Dictionary<TKey, TValue> dictionary2 = new Dictionary<TKey, TValue>();
		DeepCopy(dictionary, dictionary2);
		return dictionary2;
	}

	public static void DeepCopy<TKey, TValue>(Dictionary<TKey, TValue> from, Dictionary<TKey, TValue> to)
	{
		bool flag = default(TKey) is ICloneable;
		bool flag2 = default(TValue) is ICloneable;
		foreach (KeyValuePair<TKey, TValue> item in from)
		{
			TKey key = ((!flag) ? item.Key : ((TKey)((ICloneable)(object)item.Key).Clone()));
			TValue value = ((!flag2) ? item.Value : ((TValue)((ICloneable)(object)item.Value).Clone()));
			to.Add(key, value);
		}
	}

	public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		HashSet<TKey> seenKeys = new HashSet<TKey>();
		foreach (TSource item in source)
		{
			if (seenKeys.Add(keySelector(item)))
			{
				yield return item;
			}
		}
	}

	public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> source, IEnumerable<KeyValuePair<TKey, TValue>> range, bool overwrite = true)
	{
		foreach (KeyValuePair<TKey, TValue> item in range)
		{
			if (overwrite)
			{
				source[item.Key] = item.Value;
			}
			else if (!source.ContainsKey(item.Key))
			{
				source.Add(item.Key, item.Value);
			}
		}
		return source;
	}

	public static IEnumerable<T> AsDepthFirstEnumerable<T>(this T head, Func<T, IEnumerable<T>> childrenFunc)
	{
		yield return head;
		foreach (T item in childrenFunc(head))
		{
			foreach (T item2 in item.AsDepthFirstEnumerable(childrenFunc))
			{
				yield return item2;
			}
		}
	}

	public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
	{
		return source ?? Enumerable.Empty<T>();
	}

	public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T value)
	{
		source = source ?? throw new ArgumentNullException("source");
		return source.Where((T el) => !object.Equals(el, value));
	}
}
