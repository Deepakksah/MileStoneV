using System;
using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class IntegerValueInfo : BaseValueInfo
{
	public const string IntegerEntry = "IntegerEntry";

	public int Value
	{
		get
		{
			return Convert.ToInt32(Values[0]);
		}
		set
		{
			Values = new List<string> { value.ToString() };
		}
	}

	public IntegerValueInfo()
		: base("IntegerEntry")
	{
	}

	public IntegerValueInfo(IEnumerable<ValueInfo> infos)
		: base("IntegerEntry", infos)
	{
	}
}
