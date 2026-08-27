using System;
using System.Collections.Generic;
using VideoOS.MetadataSearch.Common.Resources;

namespace VideoOS.MetadataSearch.Common;

public static class Faces
{
	public static readonly Guid Face = new Guid("E7D7F6BE-2A1B-4844-87F8-23BEC0DD09A7");

	private static IReadOnlyDictionary<Guid, string> FaceStrings = new Dictionary<Guid, string> { 
	{
		Face,
		Strings.FaceSearchFilterItem
	} };

	public static string GetValue()
	{
		return FaceStrings[Face];
	}
}
