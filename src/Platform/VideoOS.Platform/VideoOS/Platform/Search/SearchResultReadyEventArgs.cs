using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Search;

public class SearchResultReadyEventArgs : EventArgs
{
	public IEnumerable<SearchResultData> SearchResults { get; set; }

	public SearchDefinition SearchDefinition { get; set; }

	public Guid SessionId { get; set; }
}
