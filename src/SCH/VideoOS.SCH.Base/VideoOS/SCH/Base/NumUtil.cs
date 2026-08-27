using System;
using System.Globalization;

namespace VideoOS.SCH.Base;

public class NumUtil
{
	public static bool IsDouble(object expression)
	{
		double result;
		return double.TryParse(Convert.ToString(expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out result);
	}

	public static bool IsInt(object expression)
	{
		if (IsDouble(expression))
		{
			double num = double.Parse(Convert.ToString(expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
			string text = num.ToString(NumberFormatInfo.InvariantInfo);
			if (text.IndexOf(".", StringComparison.InvariantCulture) < 0)
			{
				char c = char.Parse("-");
				bool flag = false;
				if (text.ToCharArray()[0] == c)
				{
					flag = true;
					text = text.TrimStart(c);
				}
				if (text.Length == 10)
				{
					if (text.StartsWith("1", ignoreCase: true, CultureInfo.InvariantCulture) || text.StartsWith("2", ignoreCase: true, CultureInfo.InvariantCulture))
					{
						if (flag)
						{
							if (num >= -2147483648.0)
							{
								return true;
							}
						}
						else if (num <= 2147483647.0)
						{
							return true;
						}
					}
				}
				else if (text.Length < 10)
				{
					return true;
				}
			}
		}
		return false;
	}
}
