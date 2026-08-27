using System.Globalization;

namespace VideoOS.Common.Integration;

public class StringLogicalComparer
{
	public static int Compare(string s1, string s2)
	{
		if (s1 == null && s2 == null)
		{
			return 0;
		}
		if (s1 == null)
		{
			return -1;
		}
		if (s2 == null)
		{
			return 1;
		}
		if (s1.Equals(string.Empty) && s2.Equals(string.Empty))
		{
			return 0;
		}
		if (s1.Equals(string.Empty))
		{
			return -1;
		}
		if (s2.Equals(string.Empty))
		{
			return -1;
		}
		bool flag = char.IsLetterOrDigit(s1, 0);
		bool flag2 = char.IsLetterOrDigit(s2, 0);
		if (flag && !flag2)
		{
			return 1;
		}
		if (!flag && flag2)
		{
			return -1;
		}
		int i = 0;
		int i2 = 0;
		int num = 0;
		do
		{
			bool flag3 = char.IsDigit(s1, i);
			bool flag4 = char.IsDigit(s2, i2);
			if (!flag3 && !flag4)
			{
				bool flag5 = char.IsLetter(s1, i);
				bool flag6 = char.IsLetter(s2, i2);
				if ((flag5 && flag6) || (!flag5 && !flag6))
				{
					num = ((!(flag5 && flag6)) ? s1[i].CompareTo(s2[i2]) : char.ToLower(s1[i], CultureInfo.InvariantCulture).CompareTo(char.ToLower(s2[i2], CultureInfo.InvariantCulture)));
					if (num != 0)
					{
						return num;
					}
				}
				else
				{
					if (!flag5 && flag6)
					{
						return -1;
					}
					if (flag5 && !flag6)
					{
						return 1;
					}
				}
			}
			else if (flag3 && flag4)
			{
				num = CompareNum(s1, ref i, s2, ref i2);
				if (num != 0)
				{
					return num;
				}
			}
			else
			{
				if (flag3)
				{
					return -1;
				}
				if (flag4)
				{
					return 1;
				}
			}
			i++;
			i2++;
			if (i >= s1.Length && i2 >= s2.Length)
			{
				return 0;
			}
			if (i >= s1.Length)
			{
				return -1;
			}
		}
		while (i2 < s2.Length);
		return -1;
	}

	private static int CompareNum(string s1, ref int i1, string s2, ref int i2)
	{
		int nzStart = i1;
		int nzStart2 = i2;
		int end = i1;
		int end2 = i2;
		ScanNumEnd(s1, i1, ref end, ref nzStart);
		ScanNumEnd(s2, i2, ref end2, ref nzStart2);
		int num = i1;
		i1 = end - 1;
		int num2 = i2;
		i2 = end2 - 1;
		int num3 = end - nzStart;
		int num4 = end2 - nzStart2;
		if (num3 < num4)
		{
			return -1;
		}
		if (num3 > num4)
		{
			return 1;
		}
		int num5 = nzStart;
		int num6 = nzStart2;
		while (num5 <= i1)
		{
			int num7 = s1[num5].CompareTo(s2[num6]);
			if (num7 != 0)
			{
				return num7;
			}
			num5++;
			num6++;
		}
		int num8 = end - num;
		int num9 = end2 - num2;
		if (num8 == num9)
		{
			return 0;
		}
		if (num8 > num9)
		{
			return -1;
		}
		return 1;
	}

	private static void ScanNumEnd(string s, int start, ref int end, ref int nzStart)
	{
		nzStart = start;
		end = start;
		bool flag = true;
		while (char.IsDigit(s, end))
		{
			if (flag && s[end].Equals('0'))
			{
				nzStart++;
			}
			else
			{
				flag = false;
			}
			end++;
			if (end >= s.Length)
			{
				break;
			}
		}
	}
}
