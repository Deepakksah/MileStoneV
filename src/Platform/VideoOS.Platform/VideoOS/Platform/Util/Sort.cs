using System;
using System.Text.RegularExpressions;

namespace VideoOS.Platform.Util;

public static class Sort
{
	public static int NumericStringCompare(string str1, string str2)
	{
		string[] array = Regex.Split(str1, "([0-9]+)");
		string[] array2 = Regex.Split(str2, "([0-9]+)");
		int num = Math.Min(array.Length, array2.Length);
		for (int i = 0; i < num; i++)
		{
			int result;
			int result2;
			int num2 = ((i % 2 != 0 && int.TryParse(array[i], out result) && int.TryParse(array2[i], out result2)) ? result.CompareTo(result2) : string.Compare(array[i], array2[i], StringComparison.OrdinalIgnoreCase));
			if (num2 != 0)
			{
				return num2;
			}
		}
		return array.Length.CompareTo(array2.Length);
	}

	public static int LongValueCompare(long l1, long l2)
	{
		if (l1 < l2)
		{
			return -1;
		}
		if (l1 > l2)
		{
			return 1;
		}
		return 0;
	}
}
