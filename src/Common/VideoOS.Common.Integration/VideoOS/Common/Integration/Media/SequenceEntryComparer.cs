using System;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.Media;

public class SequenceEntryComparer : IComparer<SequenceEntry>
{
	public int Compare(SequenceEntry x, SequenceEntry y)
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
