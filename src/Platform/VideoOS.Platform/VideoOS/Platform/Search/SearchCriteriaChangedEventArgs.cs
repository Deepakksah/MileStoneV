using System;
using VideoOS.Platform.Search.FilterValues;

namespace VideoOS.Platform.Search;

public class SearchCriteriaChangedEventArgs : EventArgs
{
	public SearchCriteriaChangeType ChangeType { get; }

	public SearchFilter SearchFilter { get; }

	public FilterValueBase SearchFilterValue { get; }

	public SearchCriteriaChangedEventArgs(SearchCriteriaChangeType changeType, SearchFilter searchFilter, FilterValueBase searchFilterValue)
	{
		ChangeType = changeType;
		SearchFilter = searchFilter;
		SearchFilterValue = searchFilterValue;
	}
}
