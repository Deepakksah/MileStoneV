using System;
using System.Collections.Generic;
using VideoOS.MetadataSearch.Common.Resources;

namespace VideoOS.MetadataSearch.Common;

public class ClothingBottomGrains
{
	public static readonly Guid Stria = new Guid("C5076004-7458-48E5-B683-262FCCD5D948");

	public static readonly Guid Plaid = new Guid("3512B9CC-4421-498E-B625-09E9C1607A02");

	public static readonly Guid PureColour = new Guid("2E5412B7-D324-4677-AFA8-065D88CEFFD6");

	public static readonly Guid Decal = new Guid("CAA998F0-2067-47CC-A417-8FECA6382009");

	public static readonly Guid Other = new Guid("C9B318EB-B522-4781-8C71-DD9707597617");

	private static IReadOnlyDictionary<Guid, string> GrainStrings = new Dictionary<Guid, string>
	{
		{
			Stria,
			Strings.StriaSearchFilterItem
		},
		{
			Plaid,
			Strings.PlaidSearchFilterItem
		},
		{
			PureColour,
			Strings.PureColourSearchFilterItem
		},
		{
			Decal,
			Strings.DecalSearchFilterItem
		},
		{
			Other,
			Strings.OtherSearchFilterItem
		}
	};

	public static string GetValue(Guid bottomGrainId)
	{
		try
		{
			return GrainStrings[bottomGrainId];
		}
		catch (KeyNotFoundException innerException)
		{
			throw new InvalidOperationException("No bottom grain found for the ID", innerException);
		}
	}
}
