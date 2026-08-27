using System;
using VideoOS.Platform.Search.Errors;

namespace VideoOS.Platform.Search;

public class SearchErrorEventArgs : EventArgs
{
	public SearchError SearchError { get; set; }

	public SearchDefinition SearchDefinition { get; set; }

	public Guid SessionId { get; set; }
}
