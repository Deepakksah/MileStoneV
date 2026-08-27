using System.Collections.Generic;

namespace VideoOS.Platform.AccessControl.Internal.Util;

public class CollectionComparisonUtil
{
	public static bool MultisetCompare<T>(IEnumerable<T> first, IEnumerable<T> second)
	{
		if (first == second)
		{
			return true;
		}
		IDictionary<T, int> dictionary = new Dictionary<T, int>();
		foreach (T item in first)
		{
			if (!dictionary.TryGetValue(item, out var value))
			{
				value = 0;
			}
			dictionary[item] = value + 1;
		}
		foreach (T item2 in second)
		{
			if (!dictionary.TryGetValue(item2, out var value2))
			{
				return false;
			}
			if (value2 == 1)
			{
				dictionary.Remove(item2);
			}
			else
			{
				dictionary[item2] = value2 - 1;
			}
		}
		return dictionary.Count == 0;
	}
}
