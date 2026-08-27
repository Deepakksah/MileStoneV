using System;
using System.Collections.Generic;
using VideoOS.MetadataSearch.Common.Resources;

namespace VideoOS.MetadataSearch.Common;

public class ClothingTopGrains
{
	public static readonly Guid Stria = new Guid("297079AD-8120-4366-9D00-5F3A164B8EE8");

	public static readonly Guid Plaid = new Guid("EE2F568C-5704-4909-8D52-F9EB5BA11C11");

	public static readonly Guid PureColour = new Guid("28026255-3273-41AE-BD8B-FBCBF3AE6B0B");

	public static readonly Guid Decal = new Guid("56815510-DB52-4E2F-AC4C-0EECF2A3FFD3");

	public static readonly Guid Other = new Guid("89B50A41-444B-4577-8275-30761242E2EE");

	private static IReadOnlyDictionary<Guid, string> TopGrainStrings = new Dictionary<Guid, string>
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

	public static string GetValue(Guid topGrainId)
	{
		try
		{
			return TopGrainStrings[topGrainId];
		}
		catch (KeyNotFoundException innerException)
		{
			throw new InvalidOperationException("No top grain found for the ID", innerException);
		}
	}
}
