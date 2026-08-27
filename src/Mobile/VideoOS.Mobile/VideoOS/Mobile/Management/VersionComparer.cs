using System.Collections.Generic;

namespace VideoOS.Mobile.Management;

public class VersionComparer : IComparer<Version>
{
	public int Compare(Version x, Version y)
	{
		return CompareS(x, y);
	}

	public static int CompareS(Version x, Version y)
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
		int num = (int)(x.Major - y.Major);
		if (num != 0)
		{
			return num;
		}
		num = (int)(x.Minor - y.Minor);
		if (num != 0)
		{
			return num;
		}
		num = (int)(x.Build - y.Build);
		if (num != 0)
		{
			return num;
		}
		return (int)(x.Revision - y.Revision);
	}
}
