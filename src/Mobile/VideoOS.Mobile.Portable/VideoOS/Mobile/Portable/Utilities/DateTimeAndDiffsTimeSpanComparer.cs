using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.Utilities;

public class DateTimeAndDiffsTimeSpanComparer : IComparer<DateTimeAndDiffs>
{
	public int Compare(DateTimeAndDiffs x, DateTimeAndDiffs y)
	{
		if (x == null && y == null)
		{
			return 0;
		}
		if (x == null)
		{
			return -1;
		}
		if (y == null)
		{
			return 1;
		}
		long num = x.TimeSpan.Ticks - y.TimeSpan.Ticks;
		if (num > 0)
		{
			return 1;
		}
		if (num < 0)
		{
			return -1;
		}
		return 0;
	}
}
