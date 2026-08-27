using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Search;

public class SearchAgentResultReadyEventArgs : EventArgs
{
	public Guid SessionId { get; set; }

	public IEnumerable<SearchResultData> SearchResults { get; set; }
}
