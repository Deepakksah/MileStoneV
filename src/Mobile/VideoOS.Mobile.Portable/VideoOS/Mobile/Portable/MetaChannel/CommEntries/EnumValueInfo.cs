using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class EnumValueInfo : BaseValueInfo
{
	public const string EnumEntry = "EnumEntry";

	public EnumValueInfo()
		: base("EnumEntry")
	{
	}

	public EnumValueInfo(IEnumerable<ValueInfo> infos)
		: base("EnumEntry", infos)
	{
	}
}
