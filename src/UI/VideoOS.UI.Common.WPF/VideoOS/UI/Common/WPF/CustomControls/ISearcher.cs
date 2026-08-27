using System;

namespace VideoOS.UI.Common.WPF.CustomControls;

public interface ISearcher
{
	event EventHandler<SearchResult> SearchCompleted;

	Guid Search(string searchText);
}
