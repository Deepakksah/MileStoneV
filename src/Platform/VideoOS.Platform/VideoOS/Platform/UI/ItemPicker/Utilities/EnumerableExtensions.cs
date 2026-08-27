using System;
using System.Collections.Generic;

namespace VideoOS.Platform.UI.ItemPicker.Utilities;

public static class EnumerableExtensions
{
	public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
	{
		foreach (T item in enumeration)
		{
			action(item);
		}
		return enumeration;
	}
}
