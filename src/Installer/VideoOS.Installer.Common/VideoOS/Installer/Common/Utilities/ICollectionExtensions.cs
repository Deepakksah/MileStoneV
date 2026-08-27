using System;
using System.Collections.Generic;

namespace VideoOS.Installer.Common.Utilities;

public static class ICollectionExtensions
{
	public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (source == null)
		{
			return;
		}
		foreach (T item in source)
		{
			target.Add(item);
		}
	}
}
