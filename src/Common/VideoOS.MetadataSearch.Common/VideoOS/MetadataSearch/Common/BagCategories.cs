using System;
using System.Collections.Generic;
using VideoOS.MetadataSearch.Common.Resources;

namespace VideoOS.MetadataSearch.Common;

public class BagCategories
{
	public static readonly Guid SingleShoulderBag = new Guid("{C41A881A-A4B6-4122-85ED-A5612B669DCA}");

	public static readonly Guid Backpack = new Guid("{C11F8F7A-A8D3-4834-9691-EF25A7ACCBC7}");

	public static readonly Guid Other = new Guid("{686EFF47-7065-40E4-BA0C-DA23B8C50B9D}");

	public const string BagCategorySingleShoulderBag = "SingleShoulderBag";

	public const string BagCategoryBackpack = "Backpack";

	public const string BagCategoryOther = "Other";

	private static IReadOnlyDictionary<Guid, string> BagCategoryStrings = new Dictionary<Guid, string>
	{
		{
			SingleShoulderBag,
			Strings.SingleShoulderBagSearchFilterItem
		},
		{
			Backpack,
			Strings.BackpackSearchFilterItem
		},
		{
			Other,
			Strings.OtherSearchFilterItem
		}
	};

	public static string GetValue(Guid bagCategoryId)
	{
		try
		{
			return BagCategoryStrings[bagCategoryId];
		}
		catch (KeyNotFoundException innerException)
		{
			throw new InvalidOperationException("No bag category type found for the ID", innerException);
		}
	}
}
