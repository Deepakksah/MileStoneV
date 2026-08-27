using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Mobile.Portable.Utilities.Transaction;

public class ConsecutiveElementAggregator
{
	public static IEnumerable<T> Aggregate<T>(IEnumerable<IEnumerable<T>> collections)
	{
		List<T> list = new List<T>();
		int num = 0;
		List<List<T>> list2 = new List<List<T>>();
		foreach (IEnumerable<T> collection in collections)
		{
			List<T> list3 = new List<T>(collection);
			list2.Add(list3);
			num = System.Math.Max(num, list3.Count());
		}
		for (int i = 0; i < num; i++)
		{
			foreach (List<T> item in list2)
			{
				if (i < item.Count)
				{
					list.Add(item[i]);
				}
			}
		}
		return list;
	}
}
