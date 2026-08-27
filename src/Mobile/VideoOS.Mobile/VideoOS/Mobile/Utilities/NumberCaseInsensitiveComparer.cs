using System;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

namespace VideoOS.Mobile.Utilities;

public class NumberCaseInsensitiveComparer : CaseInsensitiveComparer
{
	public new int Compare(object x, object y)
	{
		try
		{
			if (!(x is string) || !(y is string))
			{
				throw new InvalidCastException();
			}
			double x2 = Convert.ToDouble(x, CultureInfo.InvariantCulture);
			double y2 = Convert.ToDouble(y, CultureInfo.InvariantCulture);
			return Compare(x2, y2);
		}
		catch (Exception)
		{
		}
		return base.Compare(x, y);
	}

	private int Compare(double x, double y)
	{
		if (x > y)
		{
			return 1;
		}
		if (x < y)
		{
			return -1;
		}
		return 0;
	}

	private bool IsWholeNumber(string strNumber)
	{
		if (string.IsNullOrWhiteSpace(strNumber))
		{
			return false;
		}
		return !new Regex("[^0-9]").IsMatch(strNumber);
	}
}
