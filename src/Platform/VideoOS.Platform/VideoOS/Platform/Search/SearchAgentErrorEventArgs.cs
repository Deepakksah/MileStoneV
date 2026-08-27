using System;
using VideoOS.Platform.Search.Errors;

namespace VideoOS.Platform.Search;

public class SearchAgentErrorEventArgs : EventArgs
{
	public Guid SessionId { get; set; }

	public SearchError SearchError { get; set; }
}
