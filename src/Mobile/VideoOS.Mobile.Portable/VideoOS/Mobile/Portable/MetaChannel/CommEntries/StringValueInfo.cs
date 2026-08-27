using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class StringValueInfo : BaseValueInfo
{
	public const string StringEntry = "StringEntry";

	public StringValueInfo()
		: base("StringEntry")
	{
	}

	public StringValueInfo(IEnumerable<ValueInfo> infos)
		: base("StringEntry", infos)
	{
	}
}
