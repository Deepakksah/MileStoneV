using System;
using System.Text;

namespace VideoOS.EventServer.Communication.Rest;

public static class StringExtension
{
	public static string Replace(this string str, string oldValue, string newValue, StringComparison comparisonType)
	{
		if (oldValue.Length == 0)
		{
			throw new ArgumentException("String cannot be of zero length.");
		}
		if (str.Length == 0)
		{
			return str;
		}
		StringBuilder stringBuilder = new StringBuilder(str.Length);
		bool flag = string.IsNullOrEmpty(newValue);
		int num = 0;
		int num2;
		while ((num2 = str.IndexOf(oldValue, num, comparisonType)) != -1)
		{
			int num3 = num2 - num;
			if (num3 != 0)
			{
				stringBuilder.Append(str, num, num3);
			}
			if (!flag)
			{
				stringBuilder.Append(newValue);
			}
			num = num2 + oldValue.Length;
			if (num == str.Length)
			{
				return stringBuilder.ToString();
			}
		}
		int count = str.Length - num;
		stringBuilder.Append(str, num, count);
		return stringBuilder.ToString();
	}
}
