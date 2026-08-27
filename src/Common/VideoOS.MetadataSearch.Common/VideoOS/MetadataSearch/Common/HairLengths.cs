using System;
using System.Collections.Generic;
using VideoOS.MetadataSearch.Common.Resources;

namespace VideoOS.MetadataSearch.Common;

public class HairLengths
{
	public static readonly Guid Long = new Guid("{90B13900-D646-4E90-A149-5FB53D4BEE05}");

	public static readonly Guid Medium = new Guid("{B950B23F-CD00-45ED-BC65-58240CBA9F29}");

	public static readonly Guid Short = new Guid("{60779DAE-2522-4925-B353-4BFDD5F3A020}");

	public const string HairLengthLong = "Long";

	public const string HairLengthMedium = "Medium";

	public const string HairLengthShort = "Short";

	private static IReadOnlyDictionary<Guid, string> HairLengthStrings = new Dictionary<Guid, string>
	{
		{
			Long,
			Strings.LongSearchFilterItem
		},
		{
			Medium,
			Strings.MediumSearchFilterItem
		},
		{
			Short,
			Strings.ShortSearchFilterItem
		}
	};

	public static string GetValue(Guid hairLengthId)
	{
		try
		{
			return HairLengthStrings[hairLengthId];
		}
		catch (KeyNotFoundException innerException)
		{
			throw new InvalidOperationException("No hair length type found for the ID", innerException);
		}
	}
}
