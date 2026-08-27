using System;

namespace VideoOS.Platform.Search;

public class SearchDoneEventArgs : EventArgs
{
	public SearchDefinition SearchDefinition { get; set; }
}
