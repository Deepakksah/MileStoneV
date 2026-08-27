using System;

namespace VideoOS.Management.Common;

public static class MarkedDataDefinitions
{
	[Flags]
	public enum MarkedDataTypes
	{
		MarkedData = 2,
		Bookmark = 8
	}

	public enum RetentionType
	{
		NotUsed,
		Days,
		Weeks,
		Months,
		Years,
		Indefinite,
		UserDefined
	}
}
