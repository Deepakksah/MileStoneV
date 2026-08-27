using System.Collections.Generic;

namespace VideoOS.Common.Integration.Definition;

public class DeviceHwidComparer : IComparer<DeviceHwid>, IComparer<string>
{
	public int Compare(DeviceHwid x, DeviceHwid y)
	{
		if (x == null && y == null)
		{
			return 0;
		}
		if (y == null)
		{
			return 1;
		}
		if (x == null)
		{
			return -1;
		}
		int num = DeviceTypeComparer.DeviceTypeToSortOrder(x.Type);
		int value = DeviceTypeComparer.DeviceTypeToSortOrder(y.Type);
		int num2 = num.CompareTo(value);
		if (num2 == 0)
		{
			num2 = x.Index.CompareTo(y.Index);
		}
		return num2;
	}

	public int Compare(string x, string y)
	{
		DeviceHwid x2 = new DeviceHwid(x);
		DeviceHwid y2 = new DeviceHwid(y);
		return Compare(x2, y2);
	}
}
