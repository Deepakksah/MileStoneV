using System;
using System.Globalization;

namespace VideoOS.Common.Integration.LicenseInfo;

public sealed class WhitelistMatchHelper
{
	public SearchType SearchMethod { get; }

	public string SearchTerm { get; }

	public int MaximumChannelCount { get; }

	public WhitelistMatchHelper(string searchTerm, string searchType, int maximumChannelCount)
	{
		if (string.IsNullOrEmpty(searchTerm))
		{
			throw new ArgumentNullException("searchTerm");
		}
		if (maximumChannelCount <= 0)
		{
			throw new ArgumentNullException("maximumChannelCount");
		}
		if (string.IsNullOrEmpty(searchType))
		{
			searchType = "Partial";
		}
		object obj;
		try
		{
			obj = Enum.Parse(typeof(SearchType), searchType, ignoreCase: true);
		}
		catch (ArgumentException)
		{
			obj = SearchType.Unknown;
		}
		SearchMethod = (SearchType)obj;
		SearchTerm = searchTerm.ToUpper(CultureInfo.InvariantCulture);
		MaximumChannelCount = maximumChannelCount;
	}

	public bool IsMatch(string data)
	{
		return SearchMethod switch
		{
			SearchType.Unknown => false, 
			SearchType.Exact => SearchTerm.Equals(data, StringComparison.Ordinal), 
			SearchType.Partial => data.IndexOf(SearchTerm, StringComparison.Ordinal) >= 0, 
			_ => false, 
		};
	}
}
