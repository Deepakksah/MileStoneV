using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VideoOS.Platform.Search.Errors;

namespace VideoOS.Platform.Search;

public abstract class SearchDefinition
{
	public Guid SearchAgentPluginId { get; public set; }

	public SearchScope SearchScope => SearchCriteria.SearchScope;

	public SearchCriteria SearchCriteria { get; }

	public event EventHandler<SearchAgentResultReadyEventArgs> SearchResultReady;

	public event EventHandler<SearchAgentErrorEventArgs> ErrorOccurred;

	protected SearchDefinition(SearchScope searchScope)
	{
		if (searchScope == null)
		{
			throw new ArgumentNullException("searchScope");
		}
		SearchCriteria = new SearchCriteria(searchScope);
	}

	protected virtual void FireSearchResultsReadyEvent(Guid sessionId, IEnumerable<SearchResultData> searchResults)
	{
		if (searchResults != null)
		{
			List<SearchResultData> list = searchResults.Select(delegate(SearchResultData r)
			{
				r.SearchDefinition = r.SearchDefinition ?? this;
				return r;
			}).ToList();
			if (list.Any())
			{
				EventHandler<SearchAgentResultReadyEventArgs> eventHandler = this.SearchResultReady;
				SearchAgentResultReadyEventArgs e = new SearchAgentResultReadyEventArgs
				{
					SessionId = sessionId,
					SearchResults = list
				};
				eventHandler?.Invoke(this, e);
			}
		}
	}

	public void InternalFireSearchResultsReadyEvent(Guid sessionId, IEnumerable<SearchResultData> searchResults)
	{
		FireSearchResultsReadyEvent(sessionId, searchResults);
	}

	protected virtual void FireErrorOccurredEvent(Guid sessionId, SearchError searchError)
	{
		EventHandler<SearchAgentErrorEventArgs> eventHandler = this.ErrorOccurred;
		SearchAgentErrorEventArgs e = new SearchAgentErrorEventArgs
		{
			SessionId = sessionId,
			SearchError = searchError
		};
		eventHandler?.Invoke(this, e);
	}

	public void InternalFireErrorOccuredEvent(Guid sessionId, SearchError searchError)
	{
		FireErrorOccurredEvent(sessionId, searchError);
	}

	public virtual void CallSearch(Guid sessionId, DateTime from, DateTime to, IEnumerable<Item> items, SearchSortOrder sortOrder, CancellationToken cancellationToken)
	{
		if (!cancellationToken.IsCancellationRequested)
		{
			SearchInput searchInput = new SearchInput(sessionId, from, to, items, sortOrder);
			Search(searchInput, cancellationToken);
		}
	}

	protected virtual void Search(Guid sessionId, DateTime from, DateTime to, IEnumerable<Item> items, CancellationToken cancellationToken)
	{
	}

	protected virtual void Search(SearchInput searchInput, CancellationToken cancellationToken)
	{
		if (searchInput == null)
		{
			throw new ArgumentNullException("searchInput");
		}
		Search(searchInput.SessionId, searchInput.From, searchInput.To, searchInput.Items, cancellationToken);
	}

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}
}
