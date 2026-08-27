using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace VideoOS.Platform.Util;

public class StringSplitter : IStringSplitter
{
	public const char Escaper = '\\';

	public string Escape(string input, char[] separators)
	{
		NullCheck(input, "input");
		NullCheck(separators, "separators");
		StringBuilder stringBuilder = new StringBuilder(input.Length * 2);
		foreach (char c in input)
		{
			if (c == '\\' || IsSeparator(c, separators))
			{
				stringBuilder.Append('\\');
			}
			stringBuilder.Append(c);
		}
		return stringBuilder.ToString();
	}

	public string Unescape(string input, char[] separators)
	{
		NullCheck(input, "input");
		NullCheck(separators, "separators");
		int num = 0;
		StringBuilder stringBuilder = new StringBuilder(input.Length);
		while (num < input.Length - 1)
		{
			char c = input[num++];
			char c2 = input[num];
			if (c == '\\' && (c2 == '\\' || IsSeparator(c2, separators)))
			{
				stringBuilder.Append(c2);
				num++;
			}
			else
			{
				stringBuilder.Append(c);
			}
		}
		if (num == input.Length - 1)
		{
			stringBuilder.Append(input[num]);
		}
		return stringBuilder.ToString();
	}

	public string JoinString(IEnumerable enumerable, char[] separators)
	{
		return JoinString(enumerable, 0, separators);
	}

	private string JoinString(IEnumerable enumerable, int level, char[] separators)
	{
		NullCheck(enumerable, "enumerable");
		NullCheck(separators, "separators");
		ValidateLevel(level, separators);
		List<string> list = new List<string>();
		foreach (object item2 in enumerable)
		{
			if (!(item2 is string item))
			{
				list.Add(JoinString(item2 as IEnumerable, level + 1, separators));
			}
			else
			{
				list.Add(item);
			}
		}
		return Join(list, level, separators);
	}

	public IList SplitString(string input, char[] separators)
	{
		return SplitString(input, 0, separators);
	}

	private IList SplitString(string input, int level, char[] separators)
	{
		NullCheck(input, "input");
		NullCheck(separators, "separators");
		ValidateLevel(level, separators);
		input = input.Trim();
		if (string.IsNullOrEmpty(input))
		{
			return new ArrayList();
		}
		bool flag = false;
		StringBuilder stringBuilder = new StringBuilder();
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < input.Length; i++)
		{
			char c = input[i];
			bool flag2 = !flag && c == separators[level];
			bool flag3 = i == input.Length - 1;
			if (!flag2)
			{
				stringBuilder.Append(c);
				flag = !flag && c == '\\';
			}
			if (flag2 || flag3)
			{
				arrayList.Add(BuildSequenceObject(stringBuilder, level, separators));
				stringBuilder = (flag2 ? new StringBuilder() : null);
			}
		}
		if (stringBuilder != null)
		{
			arrayList.Add(BuildSequenceObject(stringBuilder, level, separators));
		}
		return arrayList;
	}

	private object BuildSequenceObject(StringBuilder sequence, int level, char[] separators)
	{
		string text = sequence.ToString();
		if (level != separators.Length - 1)
		{
			return SplitString(text, level + 1, separators);
		}
		return text.Trim();
	}

	private static string Join(List<string> list, int level, char[] separators)
	{
		return string.Join(separators[level].ToString(), list);
	}

	private static bool IsSeparator(char c, char[] separators)
	{
		for (int i = 0; i < separators.Length; i++)
		{
			if (c == separators[i])
			{
				return true;
			}
		}
		return false;
	}

	private static void ValidateLevel(int level, char[] separators)
	{
		if (level < 0 || level >= separators.Length)
		{
			throw new ArgumentOutOfRangeException(string.Format(CultureInfo.InvariantCulture, "{0} == {1} is less than 0 or exceeds the length of {2} == {3}", "level", level, "separators", separators.Length));
		}
	}

	private static void NullCheck(object data, string name)
	{
		if (data == null)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "{0} cannot be null", name));
		}
	}
}
