using System;
using System.Collections.Generic;
using VideoOS.MetadataSearch.Common.Resources;

namespace VideoOS.MetadataSearch.Common;

public class ClothingBottomCategories
{
	public static readonly Guid Trousers = new Guid("18A0D9FE-D97A-4E3E-9B33-21F8636B2F36");

	public static readonly Guid Shorts = new Guid("B9C99B54-DBCE-460E-ABCC-AC0A78C6CA42");

	public static readonly Guid Skirt = new Guid("130F582C-D713-4280-B50C-C09BC3F0F5FA");

	public static readonly Guid Other = new Guid("BE808C69-1BD0-452B-A8C5-D8EC28B47DD7");

	public const string BottomCategoryTrousers = "Trousers";

	public const string BottomCategoryShorts = "Shorts";

	public const string BottomCategorySkirt = "Skirt";

	public const string BottomCategoryOther = "Other";

	private static IReadOnlyDictionary<Guid, string> BottomCategoryStrings = new Dictionary<Guid, string>
	{
		{
			Trousers,
			Strings.TrousersSearchFilterItem
		},
		{
			Shorts,
			Strings.ShortsSearchFilterItem
		},
		{
			Skirt,
			Strings.SkirtSearchFilterItem
		},
		{
			Other,
			Strings.OtherSearchFilterItem
		}
	};

	public static string GetValue(Guid bottomCategoryId)
	{
		try
		{
			return BottomCategoryStrings[bottomCategoryId];
		}
		catch (KeyNotFoundException innerException)
		{
			throw new InvalidOperationException("No bottom category type found for the ID", innerException);
		}
	}
}
