using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace VideoOS.Utilities.Sorting;

public class AlphanumericComparatorFast : IComparer, IComparer<string>
{
	public static int CompareStrings(string value1, string value2)
	{
		if (value1 == null || value2 == null)
		{
			return 0;
		}
		int length = value1.Length;
		int length2 = value2.Length;
		int num = 0;
		int num2 = 0;
		while (num < length && num2 < length2)
		{
			char c = value1[num];
			char c2 = value2[num2];
			char[] array = new char[length];
			int num3 = 0;
			char[] array2 = new char[length2];
			int num4 = 0;
			do
			{
				array[num3++] = c;
				num++;
				if (num >= length)
				{
					break;
				}
				c = value1[num];
			}
			while (char.IsDigit(c) == char.IsDigit(array[0]));
			do
			{
				array2[num4++] = c2;
				num2++;
				if (num2 >= length2)
				{
					break;
				}
				c2 = value2[num2];
			}
			while (char.IsDigit(c2) == char.IsDigit(array2[0]));
			string text = new string(array);
			string text2 = new string(array2);
			int num5;
			if (!char.IsDigit(array[0]) || !char.IsDigit(array2[0]) || array[0] == '0' || array2[0] == '0')
			{
				num5 = ((array[0] != '0' && array2[0] != '0') ? string.Compare(text, text2, ignoreCase: false, CultureInfo.CurrentUICulture) : string.Compare(value1, value2, ignoreCase: false, CultureInfo.CurrentUICulture));
			}
			else
			{
				try
				{
					int num6 = int.Parse(text, CultureInfo.CurrentCulture);
					int value3 = int.Parse(text2, CultureInfo.CurrentCulture);
					num5 = num6.CompareTo(value3);
				}
				catch (FormatException)
				{
					num5 = string.Compare(text, text2, ignoreCase: false, CultureInfo.CurrentUICulture);
				}
				catch (OverflowException)
				{
					num5 = string.Compare(text, text2, ignoreCase: false, CultureInfo.CurrentUICulture);
				}
			}
			if (num5 != 0)
			{
				return num5;
			}
		}
		return length - length2;
	}

	public int Compare(object x, object y)
	{
		return CompareStrings(x as string, y as string);
	}

	public int Compare(string x, string y)
	{
		return CompareStrings(x, y);
	}
}
