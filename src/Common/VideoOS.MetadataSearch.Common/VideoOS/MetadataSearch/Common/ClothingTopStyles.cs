using System;
using System.Collections.Generic;
using VideoOS.MetadataSearch.Common.Resources;

namespace VideoOS.MetadataSearch.Common;

public class ClothingTopStyles
{
	public static readonly Guid Tailor = new Guid("64F7826D-FB57-43B2-A124-87798CD3BDEC");

	public static readonly Guid Jacket = new Guid("66652593-85BC-4325-894A-9222D232C2D0");

	public static readonly Guid Sweater = new Guid("840C3CB2-3300-46A3-B165-A5DB19F9CC2D");

	public static readonly Guid Overcoat = new Guid("8CEECA71-D67C-41B9-BC5F-658EB509EAAB");

	public static readonly Guid Dress = new Guid("80B0B807-A066-47C5-8015-6275F9C98BD9");

	public static readonly Guid Other = new Guid("737333D6-1F24-4408-8445-472DF460B474");

	public const string TopStyleTailor = "Tailor";

	public const string TopStyleJacket = "Jacket";

	public const string TopStyleSweater = "Sweater";

	public const string TopStyleOvercoat = "Overcoat";

	public const string TopStyleDress = "Dress";

	public const string TopStyleOther = "Other";

	private static IReadOnlyDictionary<Guid, string> TopStyleStrings = new Dictionary<Guid, string>
	{
		{
			Tailor,
			Strings.TailorSearchFilterItem
		},
		{
			Jacket,
			Strings.JacketSearchFilterItem
		},
		{
			Sweater,
			Strings.SweaterSearchFilterItem
		},
		{
			Overcoat,
			Strings.OvercoatSearchFilterItem
		},
		{
			Dress,
			Strings.DressSearchFilterItem
		},
		{
			Other,
			Strings.OtherSearchFilterItem
		}
	};

	public static string GetValue(Guid topStyleId)
	{
		try
		{
			return TopStyleStrings[topStyleId];
		}
		catch (KeyNotFoundException innerException)
		{
			throw new InvalidOperationException("No top style type found for the ID", innerException);
		}
	}
}
