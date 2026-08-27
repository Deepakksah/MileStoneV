using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Search;

public class SearchInput
{
	public Guid SessionId { get; }

	public DateTime From { get; }

	public DateTime To { get; }

	public IEnumerable<Item> Items { get; }

	public SearchSortOrder SortOrder { get; }

	public SearchInput(Guid sessionId, DateTime from, DateTime to, IEnumerable<Item> items, SearchSortOrder sortOrder)
	{
		SessionId = sessionId;
		From = from;
		To = to;
		Items = items;
		SortOrder = sortOrder;
	}

	public SearchInput(Guid sessionId, DateTime from, DateTime to, IEnumerable<Item> items)
		: this(sessionId, from, to, items, SearchSortOrder.Unspecified)
	{
	}
}
