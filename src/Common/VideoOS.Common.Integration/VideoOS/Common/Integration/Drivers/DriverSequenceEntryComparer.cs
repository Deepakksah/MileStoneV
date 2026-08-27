using System;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.Drivers;

public class DriverSequenceEntryComparer : IComparer<DriverSequenceEntry>
{
	public int Compare(DriverSequenceEntry x, DriverSequenceEntry y)
	{
		if (x == null)
		{
			throw new ArgumentNullException("x");
		}
		if (y == null)
		{
			throw new ArgumentNullException("y");
		}
		return x.StartTime.CompareTo(y.StartTime);
	}
}
