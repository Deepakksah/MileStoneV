using System;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.Extensions;

public static class ExtensionMethods
{
	public static void AddRange<T>(this IList<T> list, IEnumerable<T> range)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		if (range == null)
		{
			throw new ArgumentNullException("range");
		}
		foreach (T item in range)
		{
			list.Add(item);
		}
	}
}
