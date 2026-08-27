using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VideoOS.Platform.Search.FilterValues;

namespace VideoOS.Platform.Search;

public sealed class SearchCriteria
{
	private readonly Dictionary<SearchFilter, List<FilterValueBase>> _filterToFilterValues = new Dictionary<SearchFilter, List<FilterValueBase>>();

	public SearchScope SearchScope { get; }

	public IEnumerable<KeyValuePair<SearchFilter, IEnumerable<FilterValueBase>>> AllFilterValues => _filterToFilterValues.Keys.Select((SearchFilter filter) => new KeyValuePair<SearchFilter, IEnumerable<FilterValueBase>>(filter, GetFilterValues(filter, removeEmptyValues: false)));

	public event EventHandler<SearchCriteriaChangedEventArgs> Changed;

	public SearchCriteria(SearchScope searchScope)
	{
		if (searchScope == null)
		{
			throw new ArgumentNullException("searchScope");
		}
		SearchScope = searchScope;
		SearchScope.Changed += SearchScopeOnChanged;
	}

	private void SearchScopeOnChanged(object sender, EventArgs e)
	{
		FireChangedEvent(SearchCriteriaChangeType.SearchScopeUpdated);
	}

	private void FireChangedEvent(SearchCriteriaChangeType changeType, SearchFilter SearchFilter = null, FilterValueBase SearchFilterValue = null)
	{
		this.Changed?.Invoke(this, new SearchCriteriaChangedEventArgs(changeType, SearchFilter, SearchFilterValue));
	}

	private void SearchFilterValueOnChanged(object sender, EventArgs e)
	{
		FilterValueBase filterValueBase = (FilterValueBase)sender;
		FireChangedEvent(SearchCriteriaChangeType.FilterValueUpdated, filterValueBase.SearchFilter, filterValueBase);
	}

	public void AddFilterValue(SearchFilter searchFilter)
	{
		if (searchFilter == null)
		{
			throw new ArgumentNullException("searchFilter");
		}
		FilterValueBase filterValue = searchFilter.CreateValue();
		AddFilterValue(searchFilter, filterValue);
	}

	public void AddFilterValue(SearchFilter searchFilter, FilterValueBase filterValue)
	{
		if (searchFilter == null)
		{
			throw new ArgumentNullException("searchFilter");
		}
		if (filterValue == null)
		{
			throw new ArgumentNullException("filterValue");
		}
		if (!_filterToFilterValues.ContainsKey(searchFilter))
		{
			_filterToFilterValues[searchFilter] = new List<FilterValueBase>();
		}
		filterValue.SearchFilter = searchFilter;
		filterValue.Changed += SearchFilterValueOnChanged;
		_filterToFilterValues[searchFilter].Add(filterValue);
		FireChangedEvent(SearchCriteriaChangeType.FilterValueAdded, searchFilter, filterValue);
	}

	public void RemoveFilterValue(SearchFilter searchFilter, FilterValueBase filterValue)
	{
		if (searchFilter == null)
		{
			throw new ArgumentNullException("searchFilter");
		}
		if (filterValue == null)
		{
			throw new ArgumentNullException("filterValue");
		}
		if (_filterToFilterValues.ContainsKey(searchFilter))
		{
			if (_filterToFilterValues[searchFilter].Contains(filterValue))
			{
				_filterToFilterValues[searchFilter].Remove(filterValue);
				filterValue.Changed -= SearchFilterValueOnChanged;
				FireChangedEvent(SearchCriteriaChangeType.FilterValueRemoved, searchFilter, filterValue);
			}
			if (!_filterToFilterValues[searchFilter].Any())
			{
				_filterToFilterValues.Remove(searchFilter);
			}
		}
	}

	public void ClearFilterValues(SearchFilter searchFilter)
	{
		if (searchFilter == null)
		{
			throw new ArgumentNullException("searchFilter");
		}
		if (_filterToFilterValues.ContainsKey(searchFilter))
		{
			while (_filterToFilterValues[searchFilter].Any())
			{
				FilterValueBase filterValueBase = _filterToFilterValues[searchFilter].First();
				_filterToFilterValues[searchFilter].Remove(filterValueBase);
				filterValueBase.Changed -= SearchFilterValueOnChanged;
				FireChangedEvent(SearchCriteriaChangeType.FilterValueRemoved, searchFilter, filterValueBase);
			}
			_filterToFilterValues.Remove(searchFilter);
		}
	}

	public ReadOnlyCollection<FilterValueBase> GetFilterValues(SearchFilter searchFilter)
	{
		return GetFilterValues(searchFilter, removeEmptyValues: true);
	}

	public ReadOnlyCollection<T> GetFilterValues<T>(SearchFilter searchFilter)
	{
		return GetFilterValues(searchFilter, removeEmptyValues: true).OfType<T>().ToList().AsReadOnly();
	}

	public ReadOnlyCollection<FilterValueBase> GetFilterValues(SearchFilter searchFilter, bool removeEmptyValues)
	{
		if (searchFilter == null)
		{
			throw new ArgumentNullException("searchFilter");
		}
		if (_filterToFilterValues.ContainsKey(searchFilter))
		{
			List<FilterValueBase> list = _filterToFilterValues[searchFilter];
			if (removeEmptyValues)
			{
				list = list.Where((FilterValueBase v) => !searchFilter.IsEmptyValue(v)).ToList();
			}
			return list.AsReadOnly();
		}
		return new List<FilterValueBase>().AsReadOnly();
	}

	public void Close()
	{
		if (SearchScope != null)
		{
			SearchScope.Changed -= SearchScopeOnChanged;
		}
	}
}
