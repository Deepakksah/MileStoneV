using System.Collections.Generic;

namespace VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

public static class IReadOnlyListExtensions
{
	public static int IndexOf<T>(this IReadOnlyList<T> self, T elementToFind)
	{
		int num = 0;
		foreach (T item in self)
		{
			if (object.Equals(item, elementToFind))
			{
				return num;
			}
			num++;
		}
		return -1;
	}
}
