using System.Collections.Generic;
using C5;

namespace VideoOS.UI.Common.WPF.Timeline.InterfaceData;

public static class SortedEnumerableExtensions
{
	public static ISortedEnumerable<T> ToSortedEnumerable<T>(this IEnumerable<T> enumerable)
	{
		return new SortedEnumerable<T>(enumerable);
	}

	public static ISortedEnumerable<T> ToSortedEnumerable<T>(this ISorted<T> array)
	{
		return new SortedEnumerable<T>((IEnumerable<T>)array);
	}

	public static ISortedEnumerable<T> ToSortedEnumerable<TKey, T>(this SortedList<TKey, T> list)
	{
		return new SortedEnumerable<T>(list.Values);
	}
}
