using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.Utilities;

public static class EnumerableExtensions
{
	private enum MinMaxByOperation
	{
		Min,
		Max
	}

	public static IEnumerable<T[]> Partition<T>(this T[] arr, int partSize)
	{
		if (arr.Length < partSize)
		{
			return new List<T[]> { arr };
		}
		List<T[]> list = new List<T[]>();
		int num = arr.Length / partSize;
		for (int i = 0; i < num; i++)
		{
			T[] array = new T[partSize];
			Array.Copy(arr, i * partSize, array, 0, partSize);
			list.Add(array);
		}
		int num2 = arr.Length - num * partSize;
		if (num2 > 0)
		{
			T[] array2 = new T[num2];
			Array.Copy(arr, num * partSize, array2, 0, num2);
			list.Add(array2);
		}
		return list;
	}

	public static IEnumerable<T> Foreach<T>(this IEnumerable<T> items, Action<T> action, IProgressSet progressSet = null)
	{
		uint total = (uint)items.Count();
		progressSet?.SetProgressCurrentTotal(0u, total);
		foreach (T item in items)
		{
			action(item);
			progressSet?.SetProgressCurrentTotal(progressSet.CurrentIndex + 1, total);
		}
		return items;
	}

	public static bool Contains<T>(this IEnumerable<T> items, Func<T, bool> comparer)
	{
		return items.Any(comparer);
	}

	public static void ForEachCatched<T>(this IEnumerable<T> enumerable, Action<T> action)
	{
		foreach (T item in enumerable)
		{
			try
			{
				action(item);
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, ex.GetType()?.ToString() + ": " + ex.Message, ex.StackTrace);
			}
		}
	}

	public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
	{
		return source.MinBy(selector, null);
	}

	public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
	{
		return source.MinMaxBy(selector, comparer, MinMaxByOperation.Min);
	}

	public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
	{
		return source.MaxBy(selector, null);
	}

	public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
	{
		return source.MinMaxBy(selector, comparer, MinMaxByOperation.Max);
	}

	private static TSource MinMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, MinMaxByOperation operation)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (selector == null)
		{
			throw new ArgumentNullException("selector");
		}
		comparer = comparer ?? Comparer<TKey>.Default;
		using IEnumerator<TSource> enumerator = source.GetEnumerator();
		if (!enumerator.MoveNext())
		{
			throw new InvalidOperationException("Sequence contains no elements");
		}
		TSource val = enumerator.Current;
		TKey y = selector(val);
		while (enumerator.MoveNext())
		{
			TSource current = enumerator.Current;
			TKey val2 = selector(current);
			int num = comparer.Compare(val2, y);
			if ((operation == MinMaxByOperation.Min && num < 0) || (operation == MinMaxByOperation.Max && num > 0))
			{
				val = current;
				y = val2;
			}
		}
		return val;
	}

	public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dict)
	{
		return new ReadOnlyDictionary<TKey, TValue>(dict);
	}

	public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
	{
		if (!dictionary.TryGetValue(key, out var value))
		{
			return defaultValue;
		}
		return value;
	}
}
