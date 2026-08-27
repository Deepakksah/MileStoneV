using System;
using System.Collections.Generic;
using VideoOS.MetadataSearch.Common.Resources;

namespace VideoOS.MetadataSearch.Common;

public static class Categories
{
	public static readonly Guid People = new Guid("7C4765C6-BD67-45A1-9E0B-84313207379C");

	public static readonly Guid Vehicles = new Guid("DE799EAA-7564-4882-8E86-B0FA85919559");

	public static readonly Guid Location = new Guid("4E22B171-B878-4751-8184-DD87B2DD65F1");

	private static IReadOnlyDictionary<Guid, string> CategoryStrings = new Dictionary<Guid, string>
	{
		{
			People,
			Strings.PeopleSearchCategoryName
		},
		{
			Vehicles,
			Strings.VehiclesSearchCategoryName
		},
		{
			Location,
			Strings.LocationSearchCategoryName
		}
	};

	public static string GetValue(Guid categoryId)
	{
		try
		{
			return CategoryStrings[categoryId];
		}
		catch (KeyNotFoundException innerException)
		{
			throw new InvalidOperationException("No category found for the ID", innerException);
		}
	}
}
