using System;
using System.Collections.Generic;
using VideoOS.MetadataSearch.Common.Resources;

namespace VideoOS.MetadataSearch.Common;

public class ClothingTopCategories
{
	public static readonly Guid LongSleeve = new Guid("7496EFB4-EB38-42C7-8702-573005E554FA");

	public static readonly Guid ShortSleeve = new Guid("2884FFDC-E415-43F3-B6F5-C51FB5BEC239");

	public static readonly Guid Other = new Guid("7C4B08BF-AD89-4FE2-9C9E-9791ACD6C306");

	public const string TopCategoryLongSleeve = "LongSleeve";

	public const string TopCategoryShortSleeve = "ShortSleeve";

	public const string TopCategoryOther = "Other";

	private static IReadOnlyDictionary<Guid, string> TopCategoryStrings = new Dictionary<Guid, string>
	{
		{
			LongSleeve,
			Strings.LongSleeveSearchFilterItem
		},
		{
			ShortSleeve,
			Strings.ShortSleeveSearchFilterItem
		},
		{
			Other,
			Strings.OtherSearchFilterItem
		}
	};

	public static string GetValue(Guid topCategoryId)
	{
		try
		{
			return TopCategoryStrings[topCategoryId];
		}
		catch (KeyNotFoundException innerException)
		{
			throw new InvalidOperationException("No top category type found for the ID", innerException);
		}
	}
}
