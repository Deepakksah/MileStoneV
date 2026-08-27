using System;
using System.Collections.Generic;
using VideoOS.MetadataSearch.Common.Resources;

namespace VideoOS.MetadataSearch.Common;

public class ClothingBottomStyles
{
	public static readonly Guid FormalPants = new Guid("{C7CBACAF-656A-42D7-A0CC-70CC4E603520}");

	public static readonly Guid Jeans = new Guid("{3252F69A-1CC6-43D3-B205-A09571ED5C0A}");

	public static readonly Guid Other = new Guid("{1A68A47F-C8CB-4901-A756-8F4C8DEB7175}");

	public const string BottomStyleFormalPants = "FornalPants";

	public const string BottomStyleJeans = "Jeans";

	public const string BottomStyleOther = "Other";

	private static IReadOnlyDictionary<Guid, string> BottomStyleStrings = new Dictionary<Guid, string>
	{
		{
			FormalPants,
			Strings.FormalPantsSearchFilterItem
		},
		{
			Jeans,
			Strings.JeansSearchFilterItem
		},
		{
			Other,
			Strings.OtherSearchFilterItem
		}
	};

	public static string GetValue(Guid bottomStyleId)
	{
		try
		{
			return BottomStyleStrings[bottomStyleId];
		}
		catch (KeyNotFoundException innerException)
		{
			throw new InvalidOperationException("No bottom style type found for the ID", innerException);
		}
	}
}
