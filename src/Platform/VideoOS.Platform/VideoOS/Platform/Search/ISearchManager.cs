using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.Platform.Search;

public interface ISearchManager : IDisposable
{
	ReadOnlyCollection<SearchFilterCategory> SearchFilterCategories { get; }

	event EventHandler<SearchResultReadyEventArgs> SearchResultReady;

	event EventHandler<SearchErrorEventArgs> ErrorOccurred;

	event EventHandler<SearchDoneEventArgs> SearchDone;

	SearchDefinition CreateSearchDefinition(SearchFilterCategory searchFilterCategory, SearchScope searchScope);

	void DestroySearchDefinition(SearchDefinition searchDefinition);

	void StartSearch(SearchDefinition definition, DateTime from, DateTime to, IEnumerable<Item> items);

	void StartSearch(SearchDefinition definition, DateTime from, DateTime to, IEnumerable<Item> items, Guid sessionId);

	void StartSearch(SearchDefinition definition, DateTime from, DateTime to, IEnumerable<Item> items, SearchSortOrder sortOrder);

	void StartSearch(SearchDefinition definition, DateTime from, DateTime to, IEnumerable<Item> items, SearchSortOrder sortOrder, Guid sessionId);

	bool CancelSearch(IEnumerable<SearchDefinition> definitions);
}
