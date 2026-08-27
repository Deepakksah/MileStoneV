using System;
using System.Collections.Generic;

namespace VideoOS.Mobile.Helpers;

public static class StringHelper
{
	public static readonly int MaxHintLength = 128;

	public static string GetFixedString(string s, int len)
	{
		s = s ?? throw new ArgumentNullException("s");
		if (s.Length <= len)
		{
			return s;
		}
		return s.Substring(0, len);
	}

	public static string GetFixedStringHint(string s)
	{
		return GetFixedString(s, MaxHintLength);
	}

	public static IList<KeyValuePair<string, string>> ParseOutput(string splitString, string input)
	{
		input = input ?? throw new ArgumentNullException("input");
		List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
		string[] array = input.Split(new string[1] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(new string[1] { splitString }, StringSplitOptions.RemoveEmptyEntries);
			if (array2.Length == 2)
			{
				KeyValuePair<string, string> item = new KeyValuePair<string, string>(array2[0].Trim(), array2[1].Trim());
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		return list;
	}
}
