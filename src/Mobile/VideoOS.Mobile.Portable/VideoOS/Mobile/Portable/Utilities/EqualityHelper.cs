using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Mobile.Portable.Utilities;

public class EqualityHelper
{
	public static bool Equals<T>(IEnumerable<T> list1, IEnumerable<T> list2, IEqualityComparer<T> compare)
	{
		Dictionary<T, int> dictionary = new Dictionary<T, int>(compare);
		foreach (T item in list1)
		{
			if (dictionary.ContainsKey(item))
			{
				dictionary[item]++;
			}
			else
			{
				dictionary.Add(item, 1);
			}
		}
		foreach (T item2 in list2)
		{
			if (dictionary.ContainsKey(item2))
			{
				dictionary[item2]--;
				continue;
			}
			return false;
		}
		return dictionary.Values.All((int c) => c == 0);
	}
}
