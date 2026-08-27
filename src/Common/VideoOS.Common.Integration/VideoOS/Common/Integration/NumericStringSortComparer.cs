using System.Collections;

namespace VideoOS.Common.Integration;

public class NumericStringSortComparer : IComparer
{
	int IComparer.Compare(object x, object y)
	{
		return StringLogicalComparer.Compare(x as string, y as string);
	}
}
